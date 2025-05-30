using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using vv.Domain.Models;

namespace vv.Infrastructure.Repositories.Extensions
{
    /// <summary>
    /// Advanced query extensions for CosmosRepository
    /// </summary>
    public static class CosmosRepositoryQueryExtensions
    {
        /// <summary>
        /// Helper method to execute a query with consistent error handling
        /// </summary>
        public static async Task<IEnumerable<TResult>> ExecuteQueryAsync<T, TResult>(
            this CosmosRepository<T> repository,
            Container container,
            ILogger logger,
            Func<CancellationToken, Task<IEnumerable<TResult>>> queryFunc,
            string operationName,
            CancellationToken cancellationToken = default)
            where T : class, IMarketDataEntity
        {
            try
            {
                var results = await queryFunc(cancellationToken);
                logger.LogDebug("{OperationName} returned {Count} results of type {EntityType}",
                    operationName, results.Count(), typeof(TResult).Name);
                return results;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing {OperationName} for type {EntityType}: {ErrorMessage}",
                    operationName, typeof(TResult).Name, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Helper method to execute a single item query with consistent error handling
        /// </summary>
        public static async Task<TResult?> ExecuteSingleQueryAsync<T, TResult>(
            this CosmosRepository<T> repository,
            Container container,
            ILogger logger,
            Func<CancellationToken, Task<TResult?>> queryFunc,
            string operationName,
            string? entityId = null,
            CancellationToken cancellationToken = default)
            where T : class, IMarketDataEntity
            where TResult : class
        {
            try
            {
                var result = await queryFunc(cancellationToken);
                if (result == null)
                {
                    logger.LogInformation("{OperationName} for {EntityType}{IdInfo} returned no result",
                        operationName, typeof(TResult).Name,
                        entityId == null ? "" : $" with ID {entityId}");
                }
                return result;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                logger.LogInformation("{OperationName} for {EntityType}{IdInfo} not found",
                    operationName, typeof(TResult).Name,
                    entityId == null ? "" : $" with ID {entityId}");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing {OperationName} for {EntityType}{IdInfo}: {ErrorMessage}",
                    operationName, typeof(TResult).Name,
                    entityId == null ? "" : $" with ID {entityId}",
                    ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the latest market data based on specified criteria
        /// </summary>
        public static async Task<T?> GetLatestMarketDataAsync<T>(
            this CosmosRepository<T> repository,
            Container container,
            ILogger logger,
            string dataType,
            string assetClass,
            string assetId,
            string region,
            DateOnly asOfDate,
            string documentType,
            CancellationToken cancellationToken = default)
            where T : class, IMarketDataEntity
        {
            // Check if T implements IMarketDataEntity
            if (!typeof(IMarketDataEntity).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidOperationException(
                    $"Type {typeof(T).Name} must implement IMarketDataEntity to use GetLatestMarketDataAsync");
            }

            return await ExecuteSingleQueryAsync<T, T>(
                repository,
                container,
                logger,
                async (ct) =>
                {
                    // Use the assetId for the partition key if possible
                    var queryOptions = new QueryRequestOptions
                    {
                        PartitionKey = new PartitionKey(assetId)
                    };

                    // Use SQL query since we can't use dynamic in LINQ
                    var queryDefinition = new QueryDefinition(@"
                        SELECT TOP 1 * FROM c 
                        WHERE c.dataType = @dataType 
                        AND c.assetClass = @assetClass 
                        AND c.assetId = @assetId 
                        AND c.region = @region 
                        AND c.asOfDate = @asOfDate 
                        AND c.documentType = @documentType
                        ORDER BY c.version DESC")
                        .WithParameter("@dataType", dataType)
                        .WithParameter("@assetClass", assetClass)
                        .WithParameter("@assetId", assetId)
                        .WithParameter("@region", region)
                        .WithParameter("@asOfDate", asOfDate.ToString("yyyy-MM-dd"))
                        .WithParameter("@documentType", documentType);

                    var iterator = container.GetItemQueryIterator<T>(
                        queryDefinition,
                        requestOptions: queryOptions);

                    while (iterator.HasMoreResults)
                    {
                        var response = await iterator.ReadNextAsync(ct);
                        return response.FirstOrDefault();
                    }
                    return null;
                },
                "GetLatestMarketData",
                null,
                cancellationToken
            );
        }

        /// <summary>
        /// Queries market data within a specified date range
        /// </summary>
        public static async Task<IEnumerable<T>> QueryByRangeAsync<T>(
            this CosmosRepository<T> repository,
            Container container,
            ILogger logger,
            string dataType,
            string assetClass,
            string? assetId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
            where T : class, IMarketDataEntity
        {
            return await ExecuteQueryAsync<T, T>(
                repository,
                container,
                logger,
                async (ct) =>
                {
                    // Set default date bounds if not provided to avoid full container scans
                    var effectiveFromDate = fromDate ?? DateTime.UtcNow.AddDays(-30);
                    var effectiveToDate = toDate ?? DateTime.UtcNow;

                    // Build SQL query to avoid dynamic in expression trees
                    var queryBuilder = new System.Text.StringBuilder();
                    queryBuilder.Append(@"
                        SELECT * FROM c 
                        WHERE c.dataType = @dataType 
                        AND c.assetClass = @assetClass ");

                    if (!string.IsNullOrEmpty(assetId))
                    {
                        queryBuilder.Append("AND c.assetId = @assetId ");
                    }

                    queryBuilder.Append(@"
                        AND c.asOfDate >= @fromDate 
                        AND c.asOfDate <= @toDate ");

                    // Add soft delete filter if applicable
                    if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
                    {
                        queryBuilder.Append("AND (NOT IS_DEFINED(c.isDeleted) OR c.isDeleted = false) ");
                    }

                    // Add deterministic ordering
                    queryBuilder.Append("ORDER BY c.asOfDate DESC");

                    var queryDefinition = new QueryDefinition(queryBuilder.ToString())
                        .WithParameter("@dataType", dataType)
                        .WithParameter("@assetClass", assetClass)
                        .WithParameter("@fromDate", DateOnly.FromDateTime(effectiveFromDate).ToString("yyyy-MM-dd"))
                        .WithParameter("@toDate", DateOnly.FromDateTime(effectiveToDate).ToString("yyyy-MM-dd"));

                    if (!string.IsNullOrEmpty(assetId))
                    {
                        queryDefinition = queryDefinition.WithParameter("@assetId", assetId);
                    }

                    // Create query options with max item limit
                    var queryOptions = new QueryRequestOptions
                    {
                        MaxItemCount = 1000
                    };

                    // Add partition key if assetId is provided
                    if (!string.IsNullOrEmpty(assetId))
                    {
                        queryOptions.PartitionKey = new PartitionKey(assetId);
                    }

                    logger.LogDebug("Executing range query: DataType={DataType}, AssetClass={AssetClass}, " +
                                   "AssetId={AssetId}, FromDate={FromDate}, ToDate={ToDate}",
                                   dataType, assetClass, assetId ?? "any",
                                   effectiveFromDate.ToString("yyyy-MM-dd"),
                                   effectiveToDate.ToString("yyyy-MM-dd"));

                    var iterator = container.GetItemQueryIterator<T>(
                        queryDefinition,
                        requestOptions: queryOptions);

                    var results = new List<T>();

                    while (iterator.HasMoreResults && results.Count < 1000)
                    {
                        var response = await iterator.ReadNextAsync(ct);
                        results.AddRange(response);

                        if (results.Count >= 1000)
                        {
                            logger.LogWarning("Query result was truncated at 1000 items. Consider refining your query criteria.");
                            break;
                        }
                    }

                    return results;
                },
                "QueryByRange",
                cancellationToken
            );
        }

        // SQL translation methods

        /// <summary>
        /// Attempts to translate a LINQ expression to a SQL query for Cosmos DB
        /// </summary>
        /// <param name="predicate">The predicate expression to translate</param>
        /// <returns>Tuple containing: success flag, SQL query string, and parameters dictionary</returns>
        private static (bool Success, string? SqlQuery, Dictionary<string, object>? Parameters) TryTranslatePredicateToSql<T>(
            Expression<Func<T, bool>> predicate,
            ILogger logger)
            where T : class, IMarketDataEntity
        {
            try
            {
                var parameters = new Dictionary<string, object>();
                var sqlQuery = "SELECT * FROM c WHERE ";
                var whereClause = TranslateExpressionToSql(predicate.Body, parameters, logger);

                if (string.IsNullOrEmpty(whereClause))
                {
                    return (false, null, null);
                }

                sqlQuery += whereClause;
                return (true, sqlQuery, parameters);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to translate predicate to SQL");
                return (false, null, null);
            }
        }

        /// <summary>
        /// Translates an expression to a SQL condition
        /// </summary>
        private static string TranslateExpressionToSql(
            Expression expression,
            Dictionary<string, object> parameters,
            ILogger logger)
        {
            if (expression == null) return string.Empty;

            // Handle binary operations (==, !=, >, <, etc.)
            if (expression is BinaryExpression binary)
            {
                string left = TranslateExpressionToSql(binary.Left, parameters, logger);
                string right = TranslateExpressionToSql(binary.Right, parameters, logger);

                switch (binary.NodeType)
                {
                    case ExpressionType.Equal:
                        return $"({left} = {right})";
                    case ExpressionType.NotEqual:
                        return $"({left} != {right})";
                    case ExpressionType.GreaterThan:
                        return $"({left} > {right})";
                    case ExpressionType.GreaterThanOrEqual:
                        return $"({left} >= {right})";
                    case ExpressionType.LessThan:
                        return $"({left} < {right})";
                    case ExpressionType.LessThanOrEqual:
                        return $"({left} <= {right})";
                    case ExpressionType.AndAlso:
                        return $"({left} AND {right})";
                    case ExpressionType.OrElse:
                        return $"({left} OR {right})";
                    default:
                        logger.LogWarning("Unsupported binary expression node type: {NodeType}", binary.NodeType);
                        throw new NotSupportedException($"Binary operation {binary.NodeType} is not supported in SQL translation");
                }
            }

            // Handle member access (e.g., e.PropertyName)
            if (expression is MemberExpression member)
            {
                // Handle property access within an entity
                if (member.Expression != null && member.Expression.NodeType == ExpressionType.Parameter)
                {
                    string propertyName = member.Member.Name;
                    // Use camelCase for Cosmos DB JSON properties
                    string cosmosPropertyName = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
                    return $"c.{cosmosPropertyName}";
                }

                // Handle constant value through member access (e.g., accessing a variable)
                object? value = GetMemberValue(member);
                if (value != null)
                {
                    string paramName = $"@p{parameters.Count}";
                    parameters.Add(paramName, value);
                    return paramName;
                }
            }

            // Handle constant expressions
            if (expression is ConstantExpression constant)
            {
                if (constant.Value == null)
                {
                    return "null";
                }

                string paramName = $"@p{parameters.Count}";
                parameters.Add(paramName, constant.Value);
                return paramName;
            }

            // Handle method calls (e.g., string.Contains, string.StartsWith)
            if (expression is MethodCallExpression methodCall)
            {
                // Handle string methods
                if (methodCall.Object is MemberExpression memberExp &&
                    memberExp.Type == typeof(string))
                {
                    string propertyName = TranslateExpressionToSql(memberExp, parameters, logger);

                    if (methodCall.Method.Name == "Contains" && methodCall.Arguments.Count == 1)
                    {
                        string argValue = TranslateExpressionToSql(methodCall.Arguments[0], parameters, logger);
                        return $"CONTAINS({propertyName}, {argValue})";
                    }

                    if (methodCall.Method.Name == "StartsWith" && methodCall.Arguments.Count == 1)
                    {
                        string argValue = TranslateExpressionToSql(methodCall.Arguments[0], parameters, logger);
                        return $"STARTSWITH({propertyName}, {argValue})";
                    }

                    if (methodCall.Method.Name == "EndsWith" && methodCall.Arguments.Count == 1)
                    {
                        string argValue = TranslateExpressionToSql(methodCall.Arguments[0], parameters, logger);
                        return $"ENDSWITH({propertyName}, {argValue})";
                    }
                }

                // Handle static methods and other cases
                logger.LogWarning("Unsupported method: {MethodName}", methodCall.Method.Name);
                throw new NotSupportedException($"Method {methodCall.Method.Name} is not supported in SQL translation");
            }

            // Handle unary operations (not, negate, etc.)
            if (expression is UnaryExpression unary && unary.NodeType == ExpressionType.Not)
            {
                string operand = TranslateExpressionToSql(unary.Operand, parameters, logger);
                return $"NOT({operand})";
            }

            logger.LogWarning("Unsupported expression type: {ExpressionType}", expression.GetType().Name);
            throw new NotSupportedException($"Expression type {expression.GetType().Name} is not supported in SQL translation");
        }

        /// <summary>
        /// Gets the value of a member expression
        /// </summary>
        private static object? GetMemberValue(MemberExpression memberExpression)
        {
            // For closures in expressions like: x => x.Id == localVariable
            if (memberExpression.Expression is ConstantExpression constantExpression)
            {
                var container = constantExpression.Value;
                var member = memberExpression.Member;

                if (member is System.Reflection.FieldInfo fieldInfo)
                {
                    return fieldInfo.GetValue(container);
                }

                if (member is System.Reflection.PropertyInfo propertyInfo)
                {
                    return propertyInfo.GetValue(container);
                }
            }

            // Try to compile and execute the expression to get its value
            try
            {
                var lambda = Expression.Lambda<Func<object>>(Expression.Convert(memberExpression, typeof(object)));
                var compiledLambda = lambda.Compile();
                return compiledLambda();
            }
            catch
            {
                return null;
            }
        }
    }
}
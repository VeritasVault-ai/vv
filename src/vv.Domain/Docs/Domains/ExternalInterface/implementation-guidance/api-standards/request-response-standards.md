---
document_type: api-standards
classification: internal
status: draft
version: 0.1.0
last_updated: '2025-05-31'
applies_to:
- Core
reviewers:
- '@tech-lead'
priority: p2
next_review: '2026-05-31'
---

# Request and Response Standards

> Guidelines for Consistent API Request and Response Handling

---

## Overview

This document details the standards for handling API requests and responses within the VeritasVault platform. These standards ensure consistency, predictability, and usability across all APIs.

## Request Standards

### URL Structure

* Use kebab-case for URL paths (e.g., `/api/portfolio-analysis`)
* Include API version in the URL path (e.g., `/api/v1/portfolio-analysis`)
* Use nouns rather than verbs for resource endpoints
* Use plural nouns for collection resources (e.g., `/api/v1/portfolios`)
* Nest resources to represent relationships (e.g., `/api/v1/portfolios/{id}/assets`)
* Limit URL path depth to a maximum of 3-4 levels for readability

### HTTP Methods

* **GET**: For retrieving resources without side effects
* **POST**: For creating new resources or triggering operations
* **PUT**: For replacing resources completely
* **PATCH**: For partial updates to existing resources
* **DELETE**: For removing resources
* **HEAD**: For retrieving metadata without the response body
* **OPTIONS**: For retrieving supported methods and CORS information

### Query Parameters

* Use for filtering, sorting, pagination, and field selection
* Use consistent parameter naming across all APIs
* Parameter names should be camelCase
* Use `filter_` prefix for filtering parameters
* Use `sort` parameter with field names and direction (e.g., `sort=name:asc,date:desc`)
* Use `page` and `limit` for pagination
* Use `fields` parameter for field selection

### Headers

* Use standard HTTP headers where applicable
* Use `Accept` header for content negotiation
* Use `Content-Type` header to specify request body format
* Use `Authorization` header for authentication credentials
* Use `Idempotency-Key` header for idempotent operations
* Custom headers should use `X-` prefix and be documented clearly

### Request Body

* Use JSON as the primary format for request bodies
* Validate request bodies against JSON schemas
* Structure complex requests consistently
* Avoid deeply nested structures (limit to 3-4 levels)
* Use consistent field naming (camelCase)
* Document all required and optional fields

## Response Standards

### Status Codes

* **2xx** for successful operations
  * 200: OK (standard success response)
  * 201: Created (resource creation success)
  * 202: Accepted (for asynchronous operations)
  * 204: No Content (successful operation with no response body)
* **3xx** for redirections
  * 301: Moved Permanently
  * 302: Found
  * 304: Not Modified (for conditional requests)
* **4xx** for client errors
  * 400: Bad Request (malformed request)
  * 401: Unauthorized (authentication required)
  * 403: Forbidden (authenticated but not authorized)
  * 404: Not Found (resource doesn't exist)
  * 409: Conflict (request conflicts with current state)
  * 422: Unprocessable Entity (validation errors)
  * 429: Too Many Requests (rate limit exceeded)
* **5xx** for server errors
  * 500: Internal Server Error (unexpected server error)
  * 502: Bad Gateway (upstream service error)
  * 503: Service Unavailable (temporarily unavailable)
  * 504: Gateway Timeout (upstream service timeout)

### Response Body Structure

* Standardize success response structure:
  ```json
  {
    "data": { ... },
    "meta": {
      "timestamp": "2025-05-29T08:15:30Z",
      "requestId": "a1b2c3d4-e5f6-7890"
    }
  }
  ```

* Standardize error response structure:
  ```json
  {
    "error": {
      "code": "VALIDATION_ERROR",
      "message": "The request contains invalid parameters",
      "details": [
        {
          "field": "amount",
          "message": "Must be a positive number"
        }
      ]
    },
    "meta": {
      "timestamp": "2025-05-29T08:15:30Z",
      "requestId": "a1b2c3d4-e5f6-7890"
    }
  }
  ```

* Use standard field naming in all responses
* Include pagination metadata for collection responses:
  ```json
  {
    "data": [ ... ],
    "meta": {
      "pagination": {
        "total": 127,
        "page": 2,
        "limit": 20,
        "pages": 7
      },
      "timestamp": "2025-05-29T08:15:30Z"
    }
  }
  ```

### Content Negotiation

* Support multiple response formats through the `Accept` header
* Default to JSON if no format is specified
* Support at minimum JSON and XML formats
* Use appropriate content type headers in responses
* Document all supported response formats

### Pagination

* Use cursor-based pagination for large datasets
* Use page-based pagination for smaller, stable datasets
* Include links to next/previous pages in response metadata
* Set reasonable default page sizes (typically 20-50 items)
* Allow customization of page size up to a defined maximum

### Field Selection

* Support selecting specific fields via the `fields` query parameter
* Document which fields can be selected
* Include mandatory fields regardless of selection
* Ensure field selection doesn't negatively impact performance

## Implementation Examples

### Request Example

```
GET /api/v1/portfolios/123/assets?filter_type=equity&sort=value:desc&page=2&limit=20
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Accept: application/json
```

### Success Response Example

```json
{
  "data": [
    {
      "id": "asset456",
      "type": "equity",
      "ticker": "AAPL",
      "name": "Apple Inc.",
      "value": 150000.00,
      "currency": "USD",
      "lastUpdated": "2025-05-28T15:30:45Z"
    },
    ...
  ],
  "meta": {
    "pagination": {
      "total": 78,
      "page": 2,
      "limit": 20,
      "pages": 4
    },
    "timestamp": "2025-05-29T08:15:30Z",
    "requestId": "a1b2c3d4-e5f6-7890"
  }
}
```

### Error Response Example

```json
{
  "error": {
    "code": "RESOURCE_NOT_FOUND",
    "message": "The requested portfolio could not be found",
    "details": [
      {
        "resource": "Portfolio",
        "id": "123",
        "message": "Portfolio with ID 123 does not exist"
      }
    ]
  },
  "meta": {
    "timestamp": "2025-05-29T08:15:30Z",
    "requestId": "a1b2c3d4-e5f6-7890"
  }
}
```

## Compliance Checklist

- [ ] URLs follow the defined structure and naming conventions
- [ ] HTTP methods are used appropriately for each operation
- [ ] Query parameters follow naming and usage standards
- [ ] Standard headers are used correctly
- [ ] Request body validation is implemented
- [ ] Appropriate status codes are used for different scenarios
- [ ] Response bodies follow the standard structure
- [ ] Error responses provide actionable information
- [ ] Pagination is implemented for collection endpoints
- [ ] Content negotiation is supported where needed
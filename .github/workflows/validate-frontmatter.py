#!/usr/bin/env python3
"""
YAML Frontmatter Validator for VeritasVault Documentation

This script validates YAML frontmatter in markdown files against a JSON schema.
It reports validation errors and generates a markdown report of the results.

Usage:
    python validate-frontmatter.py [--docs-path PATH] [--schema-path PATH] [--report-path PATH]

Arguments:
    --docs-path     Path to documentation directory (default: src/vv.Domain/Docs)
    --schema-path   Path to JSON schema file (default: .github/workflows/frontmatter-schema.json)
    --report-path   Path to save the validation report (default: frontmatter-validation-report.md)
"""

import os
import re
import sys
import json
import yaml
import argparse
import logging
from pathlib import Path
from datetime import datetime
from typing import Dict, List, Tuple, Any, Optional

# Configure logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s',
    handlers=[
        logging.StreamHandler(sys.stdout),
        logging.FileHandler('frontmatter-validation.log')
    ]
)
logger = logging.getLogger(__name__)

# Regular expression to extract YAML frontmatter
FRONTMATTER_PATTERN = re.compile(r'^---\s*\n(.*?)\n---\s*\n', re.DOTALL)

class FrontmatterValidator:
    """
    Validates YAML frontmatter in markdown files against a JSON schema.
    """
    
    def __init__(self, schema_path: str):
        """
        Initialize the validator with a JSON schema.
        
        Args:
            schema_path: Path to the JSON schema file
        """
        self.schema_path = schema_path
        self.schema = self._load_schema()
        
    def _load_schema(self) -> Dict[str, Any]:
        """
        Load the JSON schema from file.
        
        Returns:
            The loaded JSON schema
        """
        try:
            with open(self.schema_path, 'r', encoding='utf-8') as f:
                return json.load(f)
        except (IOError, json.JSONDecodeError) as e:
            logger.error(f"Error loading schema from {self.schema_path}: {str(e)}")
            raise
    
    def extract_frontmatter(self, content: str) -> Optional[str]:
        """
        Extract YAML frontmatter from markdown content.
        
        Args:
            content: Markdown content
            
        Returns:
            Extracted YAML frontmatter or None if not found
        """
        match = FRONTMATTER_PATTERN.match(content)
        if match:
            return match.group(1)
        return None
    
    def validate_frontmatter(self, yaml_content: str) -> Tuple[bool, List[str]]:
        """
        Validate YAML frontmatter against the schema.
        
        Args:
            yaml_content: YAML frontmatter content
            
        Returns:
            Tuple of (is_valid, list of error messages)
        """
        errors = []
        
        try:
            # Parse YAML
            frontmatter = yaml.safe_load(yaml_content)
            
            # Basic type check
            if not isinstance(frontmatter, dict):
                return False, ["Frontmatter must be a YAML object/dictionary"]
            
            # Check required fields
            for field in self.schema.get('required', []):
                if field not in frontmatter:
                    errors.append(f"Missing required field: '{field}'")
            
            # Check field types and values
            for field, value in frontmatter.items():
                # Check if field is allowed
                if field not in self.schema.get('properties', {}):
                    errors.append(f"Unknown field: '{field}'")
                    continue
                
                field_schema = self.schema['properties'][field]
                
                # Check type
                if field_schema.get('type') == 'string' and not isinstance(value, str):
                    errors.append(f"Field '{field}' must be a string")
                elif field_schema.get('type') == 'array' and not isinstance(value, list):
                    errors.append(f"Field '{field}' must be an array")
                
                # Check enum values
                if 'enum' in field_schema and value not in field_schema['enum']:
                    allowed_values = ', '.join(f"'{v}'" for v in field_schema['enum'])
                    errors.append(f"Field '{field}' must be one of: {allowed_values}")
                
                # Check pattern
                if 'pattern' in field_schema and isinstance(value, str):
                    pattern = re.compile(field_schema['pattern'])
                    if not pattern.match(value):
                        errors.append(f"Field '{field}' does not match required pattern: {field_schema['pattern']}")
                
                # Check array properties
                if field_schema.get('type') == 'array' and isinstance(value, list):
                    if 'minItems' in field_schema and len(value) < field_schema['minItems']:
                        errors.append(f"Field '{field}' must have at least {field_schema['minItems']} items")
                    
                    # Check item types
                    if 'items' in field_schema and field_schema['items'].get('type') == 'string':
                        for i, item in enumerate(value):
                            if not isinstance(item, str):
                                errors.append(f"Item {i} in field '{field}' must be a string")
            
            return len(errors) == 0, errors
            
        except yaml.YAMLError as e:
            return False, [f"YAML parsing error: {str(e)}"]
    
    def validate_file(self, file_path: str) -> Tuple[bool, List[str]]:
        """
        Validate frontmatter in a markdown file.
        
        Args:
            file_path: Path to the markdown file
            
        Returns:
            Tuple of (is_valid, list of error messages)
        """
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            frontmatter = self.extract_frontmatter(content)
            if frontmatter is None:
                return False, ["No YAML frontmatter found"]
            
            return self.validate_frontmatter(frontmatter)
            
        except IOError as e:
            return False, [f"Error reading file: {str(e)}"]

def validate_docs(docs_path: str, schema_path: str, report_path: str) -> int:
    """
    Validate all markdown files in the documentation directory.
    
    Args:
        docs_path: Path to the documentation directory
        schema_path: Path to the JSON schema file
        report_path: Path to save the validation report
        
    Returns:
        Exit code (0 for success, 1 for validation errors)
    """
    # Initialize validator
    try:
        validator = FrontmatterValidator(schema_path)
    except Exception as e:
        logger.error(f"Failed to initialize validator: {str(e)}")
        return 1
    
    # Find all markdown files
    md_files = []
    for root, _, files in os.walk(docs_path):
        for filename in files:
            if filename.lower().endswith('.md'):
                md_files.append(os.path.join(root, filename))
    
    logger.info(f"Found {len(md_files)} markdown files to validate")
    
    # Validate each file
    results = []
    for file_path in md_files:
        rel_path = os.path.relpath(file_path, os.path.dirname(docs_path))
        is_valid, errors = validator.validate_file(file_path)
        
        status = "✅ Valid" if is_valid else "❌ Invalid"
        results.append({
            'file': rel_path,
            'status': status,
            'is_valid': is_valid,
            'errors': errors
        })
        
        if not is_valid:
            logger.warning(f"Validation failed for {rel_path}: {', '.join(errors)}")
    
    # Generate report
    generate_report(results, report_path)
    
    # Return exit code
    invalid_count = sum(1 for r in results if not r['is_valid'])
    if invalid_count > 0:
        logger.warning(f"Found {invalid_count} files with invalid frontmatter")
        return 1
    
    logger.info("All frontmatter is valid")
    return 0

def generate_report(results: List[Dict[str, Any]], report_path: str) -> None:
    """
    Generate a markdown report of validation results.
    
    Args:
        results: List of validation results
        report_path: Path to save the report
    """
    # Count statistics
    total = len(results)
    valid = sum(1 for r in results if r['is_valid'])
    invalid = total - valid
    
    # Build report content
    report = f"""# YAML Frontmatter Validation Report
Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}

## Summary
- **Total files checked**: {total}
- **Valid frontmatter**: {valid}
- **Invalid frontmatter**: {invalid}

## Validation Results

| File | Status | Issues |
|------|--------|--------|
"""
    
    # Sort results: invalid files first, then alphabetically
    sorted_results = sorted(results, key=lambda r: (r['is_valid'], r['file']))
    
    for result in sorted_results:
        file_path = result['file']
        status = result['status']
        
        if result['is_valid']:
            report += f"| {file_path} | {status} | - |\n"
        else:
            error_list = "<br>".join([f"ERROR: {e}" for e in result['errors']])
            report += f"| {file_path} | {status} | {error_list} |\n"
    
    # Add recommendations if there are invalid files
    if invalid > 0:
        report += """
## Recommendations

1. Add missing required fields to frontmatter
2. Ensure field values match the expected types and formats
3. Check for YAML syntax errors
4. Refer to the [master template](../../templates/master-template.md) for guidance
"""
    
    # Write report to file
    try:
        with open(report_path, 'w', encoding='utf-8') as f:
            f.write(report)
        logger.info(f"Validation report saved to {report_path}")
    except IOError as e:
        logger.error(f"Error writing report to {report_path}: {str(e)}")

def main():
    """
    Main entry point for the script.
    """
    # Parse command line arguments
    parser = argparse.ArgumentParser(description='Validate YAML frontmatter in markdown files.')
    parser.add_argument('--docs-path', default='src/vv.Domain/Docs', help='Path to documentation directory')
    parser.add_argument('--schema-path', default='.github/workflows/frontmatter-schema.json', help='Path to JSON schema file')
    parser.add_argument('--report-path', default='frontmatter-validation-report.md', help='Path to save the validation report')
    args = parser.parse_args()
    
    # Validate paths
    docs_path = Path(args.docs_path)
    schema_path = Path(args.schema_path)
    
    if not docs_path.exists() or not docs_path.is_dir():
        logger.error(f"Documentation directory not found: {docs_path}")
        return 1
    
    if not schema_path.exists() or not schema_path.is_file():
        logger.error(f"Schema file not found: {schema_path}")
        return 1
    
    # Run validation
    return validate_docs(str(docs_path), str(schema_path), args.report_path)

if __name__ == "__main__":
    sys.exit(main())

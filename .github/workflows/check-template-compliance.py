#!/usr/bin/env python3

"""
Template Compliance Checker for VeritasVault Documentation

This script checks if Markdown documentation files follow the structure defined
in the master template. It identifies files missing required sections or structure.

Usage:
    python check-template-compliance.py --docs-path PATH --template-path PATH --report-path PATH
"""

import os
import re
import sys
import argparse
import logging
from pathlib import Path

# Configure logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

# Files to exclude from compliance checks
EXCLUDED_FILES = [
    "README.md",
    "NAVIGATION.md",
    "index.md",
]

# Document types that should be strictly checked
STRICT_CHECK_TYPES = [
    "architecture",
    "domain-overview",
    "specification",
    "guide",
]

def extract_frontmatter(content):
    """Extract YAML frontmatter from content."""
    if content.startswith("---"):
        end_marker = content.find("---", 3)
        if end_marker != -1:
            return content[3:end_marker].strip()
    return None

def extract_document_type(frontmatter):
    """Extract document_type from frontmatter."""
    if not frontmatter:
        return None
    
    match = re.search(r'document_type:\s*([a-z-]+)', frontmatter)
    if match:
        return match.group(1)
    return None

def extract_template_sections(template_content):
    """Extract required section headers from the template."""
    # Find all headers (# Header)
    headers = re.findall(r'^#{1,3}\s+(.+?)$', template_content, re.MULTILINE)
    return headers

def check_file_compliance(file_path, template_sections):
    """Check if a file complies with the template structure."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Extract frontmatter and document type
        frontmatter = extract_frontmatter(content)
        doc_type = extract_document_type(frontmatter)
        
        # Skip strict checks for certain document types
        if doc_type not in STRICT_CHECK_TYPES:
            return True, f"Document type '{doc_type}' exempt from strict template compliance"
        
        # Check for presence of required sections
        issues = []
        for section in template_sections:
            # Some flexibility in matching - normalized case and stripped punctuation
            section_pattern = re.escape(section.lower().rstrip(':.?!'))
            if not re.search(rf'^#{1,3}\s+{section_pattern}', content.lower(), re.MULTILINE):
                issues.append(f"Missing required section: {section}")
        
        # Return results
        if issues:
            return False, "\n".join(issues)
        return True, "Compliant"
    
    except Exception as e:
        return False, f"Error processing file: {str(e)}"

def generate_report(results, report_path):
    """Generate a compliance report."""
    total = len(results)
    compliant = sum(1 for _, compliant, _ in results if compliant)
    
    report = f"""## Template Compliance Check

**Summary:**
- Total files checked: {total}
- Compliant files: {compliant}
- Non-compliant files: {total - compliant}

### Issues Found

"""
    
    for file_path, compliant, message in results:
        if not compliant:
            relative_path = file_path.replace("src/vv.Domain/Docs/", "")
            report += f"**ERROR:** `{relative_path}`\n"
            report += f"```\n{message}\n```\n\n"
    
    with open(report_path, 'w', encoding='utf-8') as f:
        f.write(report)
    
    logger.info(f"Report generated at {report_path}")

def main():
    """Main function."""
    parser = argparse.ArgumentParser(description='Check documentation template compliance.')
    parser.add_argument('--docs-path', required=True, help='Path to documentation directory')
    parser.add_argument('--template-path', required=True, help='Path to template file')
    parser.add_argument('--report-path', required=True, help='Path to save the report')
    args = parser.parse_args()
    
    docs_path = Path(args.docs_path)
    template_path = Path(args.template_path)
    report_path = args.report_path
    
    # Validate paths
    if not docs_path.exists():
        logger.error(f"Documentation directory not found: {docs_path}")
        return 1
    
    if not template_path.exists():
        logger.error(f"Template file not found: {template_path}")
        return 1
    
    # Read template content
    with open(template_path, 'r', encoding='utf-8') as f:
        template_content = f.read()
    
    # Extract template sections
    template_sections = extract_template_sections(template_content)
    logger.info(f"Found {len(template_sections)} required sections in template")
    
    # Check compliance for all Markdown files
    results = []
    for root, _, files in os.walk(docs_path):
        for file in files:
            if file.endswith('.md') and file not in EXCLUDED_FILES:
                file_path = os.path.join(root, file)
                compliant, message = check_file_compliance(file_path, template_sections)
                results.append((file_path, compliant, message))
    
    # Generate report
    generate_report(results, report_path)
    
    # Count non-compliant files
    non_compliant = sum(1 for _, compliant, _ in results if not compliant)
    logger.info(f"Found {non_compliant} non-compliant files out of {len(results)}")
    
    # Don't fail the workflow, just report issues
    return 0

if __name__ == '__main__':
    sys.exit(main())
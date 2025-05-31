#!/usr/bin/env python3

import os
import re
import yaml
from datetime import datetime, timedelta

def add_frontmatter_to_file(filepath):
    # Read file content
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Check if frontmatter exists
    if not content.startswith('---'):
        # Generate filename-based title
        base_filename = os.path.basename(filepath)
        title = os.path.splitext(base_filename)[0].replace('-', ' ').replace('_', ' ').title()
        
        # Determine document type based on path
        path = filepath.lower()
        if 'architecture' in path:
            doc_type = 'architecture'
        elif 'overview' in path:
            doc_type = 'domain-overview'
        elif 'specification' in path or 'spec' in path:
            doc_type = 'specification'
        elif 'runbook' in path:
            doc_type = 'runbook'
        elif 'guide' in path:
            doc_type = 'guide'
        elif 'policy' in path:
            doc_type = 'policy'
        elif 'api' in path and 'standard' in path:
            doc_type = 'api-standards'
        elif 'settlement' in path and 'protocol' in path:
            doc_type = 'settlement-protocol'
        elif 'portfolio' in path and 'optimization' in path:
            doc_type = 'portfolio-optimization-guide'
        elif 'audit' in path and 'system' in path:
            doc_type = 'audit-system-design'
        else:
            doc_type = 'guide'  # Default
        
        # Determine domain from path
        path_parts = filepath.split('/')
        domains = []
        for part in path_parts:
            if part in ['Asset', 'Risk', 'Security', 'Governance', 'AI', 'Crosscutting', 'ExternalInterface']:
                domains.append(part)
        
        if not domains:
            domains = ['Core']  # Default
        
        # Create frontmatter
        today = datetime.now().strftime('%Y-%m-%d')
        next_year = (datetime.now() + timedelta(days=365)).strftime('%Y-%m-%d')
        
        frontmatter = {
            'document_type': doc_type,
            'classification': 'internal',
            'status': 'draft',
            'version': '0.1.0',
            'last_updated': today,
            'applies_to': domains,
            'reviewers': ['@tech-lead'],
            'priority': 'p2',
            'next_review': next_year
        }
        
        # Convert to YAML and add to file
        yaml_content = yaml.dump(frontmatter, default_flow_style=False, sort_keys=False)
        new_content = f"---\n{yaml_content}---\n\n{content}"
        
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(new_content)
        
        print(f"Added frontmatter to {filepath}")
        return True
    
    # Check if the file has frontmatter but with errors (mentioned in the validation output)
    elif "---" in content[:100] and re.search(r"last_updated: \d+", content[:500]):
        # Fix date format issues
        with open(filepath, 'r', encoding='utf-8') as f:
            lines = f.readlines()
        
        for i, line in enumerate(lines):
            # Convert date fields from numbers to strings in quotes
            if re.match(r'(last_updated|next_review):\s+\d+', line):
                field = line.split(':')[0].strip()
                if field == 'last_updated':
                    lines[i] = f'{field}: "{datetime.now().strftime("%Y-%m-%d")}"\n'
                elif field == 'next_review':
                    next_year = (datetime.now() + timedelta(days=365)).strftime('%Y-%m-%d')
                    lines[i] = f'{field}: "{next_year}"\n'
        
        with open(filepath, 'w', encoding='utf-8') as f:
            f.writelines(lines)
        
        print(f"Fixed date format in frontmatter for {filepath}")
        return True
    
    return False

def process_directory(directory):
    files_updated = 0
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.md'):
                filepath = os.path.join(root, file)
                if add_frontmatter_to_file(filepath):
                    files_updated += 1
    
    return files_updated

if __name__ == "__main__":
    docs_dir = "src/vv.Domain/Docs"
    count = process_directory(docs_dir)
    print(f"Updated {count} files with missing or incorrect frontmatter")
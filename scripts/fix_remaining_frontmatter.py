#!/usr/bin/env python3

import os
import yaml

# List of files with specific issues
problem_files = [
    "src/vv.Domain/Docs/Domains/Asset/Guidelines.md",
    "src/vv.Domain/Docs/Domains/Asset/Design.md",
    "src/vv.Domain/Docs/Domains/Core/Design.md",
    "src/vv.Domain/Docs/Domains/Core/solidity-interfaces.md",
    "src/vv.Domain/Docs/Domains/AI/Guidelines.md",
    "src/vv.Domain/Docs/Domains/AI/Design.md"
]

# Default frontmatter template
def get_default_frontmatter(filepath):
    # Determine document type based on filename
    filename = os.path.basename(filepath)
    if filename.lower() == 'design.md':
        doc_type = 'architecture'
    elif filename.lower() == 'guidelines.md':
        doc_type = 'guide'
    elif 'interface' in filename.lower():
        doc_type = 'specification'
    else:
        doc_type = 'guide'
    
    # Determine domain from path
    path_parts = filepath.split('/')
    domain = 'Core'  # Default
    for part in path_parts:
        if part in ['Asset', 'Risk', 'Security', 'Governance', 'AI', 'Core']:
            domain = part
            break
    
    # Create frontmatter
    return {
        'document_type': doc_type,
        'classification': 'internal',
        'status': 'draft',
        'version': '0.1.0',
        'last_updated': '2025-05-31',
        'applies_to': [domain],
        'reviewers': ['@tech-lead'],
        'priority': 'p2',
        'next_review': '2026-05-31'
    }

def fix_file(filepath):
    try:
        # Read file content
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Check if content starts with frontmatter
        if content.startswith('---'):
            # Remove existing frontmatter
            end_frontmatter = content.find('---', 3)
            if end_frontmatter > 0:
                content = content[end_frontmatter + 3:].strip()
        
        # Generate new frontmatter
        frontmatter = get_default_frontmatter(filepath)
        yaml_content = yaml.dump(frontmatter, default_flow_style=False, sort_keys=False)
        
        # Create new content with proper frontmatter
        new_content = f"---\n{yaml_content}---\n\n{content}"
        
        # Write back to file
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(new_content)
        
        print(f"Fixed frontmatter for {filepath}")
        return True
    except Exception as e:
        print(f"Error fixing {filepath}: {str(e)}")
        return False

# Fix each problem file
for file_path in problem_files:
    fix_file(file_path)

print("Completed fixing problem files")
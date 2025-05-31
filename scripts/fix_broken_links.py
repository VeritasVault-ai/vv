#!/usr/bin/env python3

"""
Broken Link Fixer for VeritasVault Documentation

This script scans Markdown files for internal links and verifies if they are valid.
It can detect and report broken links, and optionally suggest fixes.

Usage:
    python fix_broken_links.py --docs-path PATH [--fix]
"""

import os
import re
import sys
import argparse
import logging
from pathlib import Path
from collections import defaultdict

# Configure logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

# Regular expression to find Markdown links
MARKDOWN_LINK_PATTERN = re.compile(r'\[([^\]]+)\]\(([^)]+)\)')

def is_internal_link(link):
    """Check if a link is internal (not external URL)."""
    return not link.startswith(('http://', 'https://', 'mailto:', 'tel:'))

def get_all_markdown_files(docs_path):
    """Get a list of all Markdown files in the documentation directory."""
    all_files = []
    for root, _, files in os.walk(docs_path):
        for file in files:
            if file.endswith('.md'):
                rel_path = os.path.relpath(os.path.join(root, file), docs_path)
                all_files.append(rel_path)
    return all_files

def extract_links(file_path, content):
    """Extract all internal links from a Markdown file."""
    links = []
    for match in MARKDOWN_LINK_PATTERN.finditer(content):
        link_text = match.group(1)
        link_url = match.group(2)
        
        # Skip external links and anchors
        if not is_internal_link(link_url) or link_url.startswith('#'):
            continue
        
        # Handle anchors within internal links
        base_url = link_url.split('#')[0]
        anchor = None
        if '#' in link_url:
            anchor = link_url.split('#', 1)[1]
        
        links.append({
            'text': link_text,
            'url': link_url,
            'base_url': base_url,
            'anchor': anchor,
            'match_obj': match
        })
    
    return links

def resolve_relative_path(source_file, target_path):
    """Resolve a relative path from the source file directory."""
    source_dir = os.path.dirname(source_file)
    if source_dir:
        target_path = os.path.normpath(os.path.join(source_dir, target_path))
    return target_path

def check_links(docs_path, fix_links=False):
    """Check all internal links in Markdown files."""
    # Get all Markdown files
    all_files = get_all_markdown_files(docs_path)
    all_files_set = set(all_files)
    
    # Map to store all section headers
    section_headers = {}
    
    # First pass: collect all section headers
    logger.info("Collecting section headers...")
    for file_path in all_files:
        try:
            with open(os.path.join(docs_path, file_path), 'r', encoding='utf-8') as f:
                content = f.read()
            
            # Find all headers
            headers = re.findall(r'^(#{1,6})\s+(.+?)$', content, re.MULTILINE)
            
            for level, header in headers:
                # Generate the anchor ID (similar to how GitHub does it)
                anchor = header.lower()
                anchor = re.sub(r'[^\w\s-]', '', anchor)  # Remove non-word chars
                anchor = re.sub(r'\s+', '-', anchor)      # Replace spaces with hyphens
                
                section_headers[file_path + '#' + anchor] = header
        except Exception as e:
            logger.error(f"Error processing {file_path}: {str(e)}")
    
    # Second pass: check all links
    logger.info("Checking internal links...")
    broken_links = []
    fixed_links = 0
    
    for file_path in all_files:
        try:
            full_path = os.path.join(docs_path, file_path)
            with open(full_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            links = extract_links(file_path, content)
            file_has_broken_links = False
            
            for link in links:
                target_path = resolve_relative_path(file_path, link['base_url'])
                
                # Check if the target file exists
                file_exists = target_path in all_files_set
                
                # Check if the anchor exists (if specified)
                anchor_exists = True
                if link['anchor'] and file_exists:
                    anchor_id = target_path + '#' + link['anchor']
                    anchor_exists = anchor_id in section_headers
                
                if not file_exists or (link['anchor'] and not anchor_exists):
                    broken_links.append({
                        'source_file': file_path,
                        'link_text': link['text'],
                        'link_url': link['url'],
                        'issue': 'File not found' if not file_exists else 'Anchor not found',
                        'target': target_path + (f"#{link['anchor']}" if link['anchor'] else "")
                    })
                    file_has_broken_links = True
            
            if file_has_broken_links and fix_links:
                # Attempt to fix broken links
                new_content = content
                for link in broken_links:
                    if link['source_file'] == file_path:
                        # Simple fix: look for similar files
                        suggested_targets = []
                        for existing_file in all_files_set:
                            if os.path.basename(existing_file) == os.path.basename(link['target']):
                                suggested_targets.append(existing_file)
                        
                        if suggested_targets:
                            # Use the first suggestion
                            rel_path = os.path.relpath(
                                os.path.join(docs_path, suggested_targets[0]), 
                                os.path.dirname(full_path)
                            )
                            # Handle Windows paths
                            rel_path = rel_path.replace('\\', '/')
                            
                            old_link = f"[{link['link_text']}]({link['link_url']})"
                            new_link = f"[{link['link_text']}]({rel_path})"
                            new_content = new_content.replace(old_link, new_link)
                            fixed_links += 1
                
                # Write back if changes were made
                if new_content != content:
                    with open(full_path, 'w', encoding='utf-8') as f:
                        f.write(new_content)
        
        except Exception as e:
            logger.error(f"Error checking links in {file_path}: {str(e)}")
    
    # Generate report
    logger.info(f"Found {len(broken_links)} broken links")
    if fix_links:
        logger.info(f"Fixed {fixed_links} links")
    
    # Print details of broken links
    if broken_links:
        logger.info("\nBroken links:")
        for link in broken_links:
            logger.info(f"File: {link['source_file']}")
            logger.info(f"  Link text: {link['link_text']}")
            logger.info(f"  Target: {link['link_url']}")
            logger.info(f"  Issue: {link['issue']}")
    
    return broken_links

def main():
    """Main function."""
    parser = argparse.ArgumentParser(description='Check for broken internal links in documentation.')
    parser.add_argument('--docs-path', default='src/vv.Domain/Docs', help='Path to documentation directory')
    parser.add_argument('--fix', action='store_true', help='Attempt to fix broken links')
    args = parser.parse_args()
    
    docs_path = Path(args.docs_path)
    
    # Validate docs path
    if not docs_path.exists() or not docs_path.is_dir():
        logger.error(f"Documentation directory not found: {docs_path}")
        return 1
    
    # Check links
    broken_links = check_links(docs_path, args.fix)
    
    # Return non-zero exit code if broken links were found
    return 1 if broken_links else 0

if __name__ == '__main__':
    sys.exit(main())
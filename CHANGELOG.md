# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial project setup with MIT license
- Comprehensive documentation

## [1.0.0] - 2025-10-22

### Added
- JSON formatting functionality for large files
- Memory-efficient streaming JSON processing
- Command-line interface with multiple options:
  - `--buffer-size`: Configurable buffer size for performance tuning
  - `--allow-trailing-commas`: Support for trailing commas in JSON
  - `--skip-comments`: Option to skip JSON comments
  - `--no-parallel`: Disable parallel processing
  - `--overwrite`: Overwrite input files instead of creating new ones
  - `--max-depth`: Set maximum parsing depth
- Parallel processing support for multiple files
- Cross-platform compatibility (.NET 9.0)
- Low memory footprint for processing large JSON files

### Security
- Input validation for file operations
- Safe JSON parsing with depth limits
# Security Policy

## Supported Versions

We release patches for security vulnerabilities in the following versions:

| Version | Supported          |
| ------- | ------------------ |
| 1.x.x   | :white_check_mark: |

## Reporting a Vulnerability

If you discover a security vulnerability within JsonFormat, please send an email to security@jsonformat.dev (or create a private security advisory on GitHub). All security vulnerabilities will be promptly addressed.

### What to include in your report:

1. **Description** of the vulnerability
2. **Steps to reproduce** the issue
3. **Potential impact** assessment
4. **Suggested fix** (if you have one)

### What to expect:

- We will acknowledge your report within 48 hours
- We will provide an estimated timeline for fixes
- We will notify you when the vulnerability has been fixed
- We will publicly credit you for the discovery (unless you prefer to remain anonymous)

### Security Best Practices

When using JsonFormat:

1. **Validate input files** before processing
2. **Use appropriate file permissions** for input/output files
3. **Be cautious with the `--overwrite` flag** as it modifies original files
4. **Monitor system resources** when processing very large files
5. **Keep JsonFormat updated** to the latest version

## Security Features

- Input validation for all command-line arguments
- Safe JSON parsing with configurable depth limits
- No network communication (offline tool)
- Minimal external dependencies
- Memory-safe operations
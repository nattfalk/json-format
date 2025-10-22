# Contributing to JsonFormat

Thank you for your interest in contributing to JsonFormat! This document provides guidelines for contributing to the project.

## Code of Conduct

By participating in this project, you agree to maintain a respectful and inclusive environment for all contributors.

## How to Contribute

### Reporting Issues

1. **Search existing issues** first to avoid duplicates
2. **Use the issue template** if available
3. **Provide clear reproduction steps** for bugs
4. **Include system information** (OS, .NET version, etc.)

### Suggesting Features

1. **Check existing feature requests** to avoid duplicates
2. **Provide clear use cases** for the proposed feature
3. **Consider backwards compatibility** implications

### Submitting Code Changes

1. **Fork the repository** and create a new branch from `main`
2. **Follow the coding style** used in the existing codebase
3. **Write clear commit messages** following conventional commits
4. **Add tests** for new functionality
5. **Update documentation** if needed
6. **Ensure all tests pass** before submitting

## Development Setup

### Prerequisites

- .NET 9.0 SDK or later
- Git
- A code editor (Visual Studio, VS Code, or similar)

### Building the Project

```bash
git clone https://github.com/nattfalk/JsonFormat.git
cd JsonFormat/src
dotnet restore
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Running the Application

```bash
dotnet run -- [options] <json-files>
```

## Coding Standards

### C# Style Guidelines

- Follow standard C# naming conventions
- Use `var` when the type is obvious from the right side
- Prefer explicit type when it improves readability
- Use meaningful variable and method names
- Add XML documentation for public APIs

### Code Structure

- Keep methods focused and concise
- Use appropriate access modifiers
- Follow SOLID principles
- Minimize dependencies between components

### Performance Considerations

- Consider memory usage for large file processing
- Use appropriate data structures for the task
- Profile performance-critical code paths
- Document any performance assumptions

## Pull Request Process

1. **Create a descriptive title** for your pull request
2. **Fill out the pull request template** completely
3. **Link related issues** using keywords (fixes #123, closes #456)
4. **Ensure CI passes** all checks
5. **Request review** from maintainers
6. **Address feedback** promptly and professionally

### Pull Request Checklist

- [ ] Code follows project coding standards
- [ ] Tests added for new functionality
- [ ] Documentation updated if needed
- [ ] Changelog updated for notable changes
- [ ] All tests pass
- [ ] No merge conflicts with main branch

## Testing Guidelines

### Unit Tests

- Write tests for new functionality
- Maintain or improve code coverage
- Use descriptive test names
- Test edge cases and error conditions

### Integration Tests

- Test complete workflows
- Verify file I/O operations
- Test command-line argument parsing

## Documentation

### Code Documentation

- Use XML documentation for public APIs
- Include parameter descriptions and return values
- Document exceptions that may be thrown

### User Documentation

- Update README.md for new features
- Add examples for new command-line options
- Keep documentation in sync with code changes

## Release Process

1. Update version numbers in project files
2. Update CHANGELOG.md with new release notes
3. Create a release branch
4. Tag the release with semantic version
5. Publish release notes

## Getting Help

- **Discussion**: Use GitHub Discussions for questions
- **Issues**: Use GitHub Issues for bugs and feature requests
- **Documentation**: Check the README and project wiki

## License

By contributing to JsonFormat, you agree that your contributions will be licensed under the MIT License.
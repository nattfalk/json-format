# Project Structure

This document describes the structure and organization of the JsonFormat project.

## Directory Structure

```
JsonFormat/
├── .github/                    # GitHub-specific files
│   ├── ISSUE_TEMPLATE/        # Issue templates
│   │   ├── bug_report.md
│   │   └── feature_request.md
│   ├── workflows/             # GitHub Actions workflows
│   │   └── ci.yml
│   └── pull_request_template.md
├── docs/                      # Documentation files
│   └── project-structure.md
├── src/                       # Source code
│   ├── Program.cs            # Main application entry point
│   ├── JsonFormat.csproj     # Project file
│   └── JsonFormat.sln        # Solution file
├── .editorconfig             # Editor configuration
├── .gitignore               # Git ignore rules
├── CHANGELOG.md             # Version history
├── CONTRIBUTING.md          # Contribution guidelines
├── LICENSE                  # MIT License
├── README.md               # Project documentation
└── SECURITY.md             # Security policy
```

## Key Components

### Source Code (`src/`)

- **Program.cs**: Contains the main application logic including:
  - Command-line argument parsing
  - JSON processing logic
  - File I/O operations
  - Error handling

- **JsonFormat.csproj**: MSBuild project file with:
  - Target framework configuration (.NET 9.0)
  - Package metadata
  - Build and publish settings

### Documentation

- **README.md**: Primary project documentation with usage examples
- **CONTRIBUTING.md**: Guidelines for contributors
- **CHANGELOG.md**: Version history and release notes
- **SECURITY.md**: Security policy and vulnerability reporting
- **docs/**: Additional documentation files

### Configuration Files

- **.editorconfig**: Code style and formatting rules
- **.gitignore**: Files and directories to exclude from Git
- **LICENSE**: MIT license text

### GitHub Integration

- **.github/workflows/ci.yml**: CI/CD pipeline configuration
- **.github/ISSUE_TEMPLATE/**: Issue reporting templates
- **.github/pull_request_template.md**: PR template

## Build Artifacts

When building the project, the following directories are created:

- `bin/`: Compiled binaries
- `obj/`: Intermediate build files
- `artifacts/`: Published releases (CI/CD)

## Development Workflow

1. **Local Development**: Edit files in `src/`
2. **Testing**: Use `dotnet test` (when tests are added)
3. **Building**: Use `dotnet build` for debug builds
4. **Publishing**: Use `dotnet publish` for release builds
5. **CI/CD**: Automated via GitHub Actions on push/PR

## Adding New Features

When adding new features:

1. Update `Program.cs` with new functionality
2. Update `README.md` with usage examples
3. Update `CHANGELOG.md` with changes
4. Add tests (when test framework is added)
5. Update documentation as needed
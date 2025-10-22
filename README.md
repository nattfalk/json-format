# JsonFormat

A high-performance command-line tool for formatting (pretty-printing) large JSON files while maintaining low memory usage.

## Features

- **Memory Efficient**: Processes very large JSON files without loading them entirely into memory
- **Fast Processing**: Optimized for speed with parallel processing support
- **Flexible Options**: Configurable buffer sizes, comment handling, and formatting options
- **Cross-Platform**: Built on .NET 9.0, runs on Windows, macOS, and Linux

## Installation

### Prerequisites

- .NET 9.0 SDK or later

### Build from Source

```bash
git clone https://github.com/nattfalk/JsonFormat.git
cd JsonFormat/src
dotnet build -c Release
```

### Run

```bash
dotnet run -- [options] <json-files>
```

Or after building:

```bash
./bin/Release/net9.0/JsonFormat [options] <json-files>
```

## Usage

```bash
JsonFormat [options] <json-files>
```

### Options

- `--help`: Show help information
- `--buffer-size <size>`: Set buffer size in bytes (default: 65536, minimum: 1024)
- `--allow-trailing-commas`: Allow trailing commas in JSON
- `--skip-comments`: Skip JSON comments during processing
- `--no-parallel`: Disable parallel processing for multiple files
- `--overwrite`: Overwrite input files instead of creating `.formatted.json` files
- `--max-depth <depth>`: Set maximum parsing depth (0 for unlimited, default: 0)

### Examples

Format a single JSON file:
```bash
JsonFormat data.json
```

Format multiple files with custom buffer size:
```bash
JsonFormat --buffer-size 131072 file1.json file2.json file3.json
```

Format with trailing commas allowed and overwrite original files:
```bash
JsonFormat --allow-trailing-commas --overwrite data.json
```

Process files sequentially (no parallel processing):
```bash
JsonFormat --no-parallel *.json
```

## Performance

JsonFormat is designed to handle very large JSON files efficiently:

- Uses streaming JSON parsing to minimize memory usage
- Configurable buffer sizes for optimal performance
- Parallel processing support for multiple files
- Low memory footprint even with gigabyte-sized JSON files

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Requirements

- .NET 9.0 or later

## Changelog

### v1.0.0
- Initial release
- Basic JSON formatting functionality
- Memory-efficient processing
- Parallel processing support
- Configurable options
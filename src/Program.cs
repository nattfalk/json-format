using System.Buffers;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web; // Added for encoder

/// <summary>
/// JsonFormat: Formats (pretty-prints / indents) very large JSON files while keeping a low memory profile.
/// </summary>
if (args.Length == 0 || HasFlag(args, "--help"))
{
    PrintHelp(); 
    return 0;
}

var bufferSize = GetIntOption(args, "--buffer-size", 65536, min: 1024);
var allowTrailingCommas = HasFlag(args, "--allow-trailing-commas");
var skipComments = HasFlag(args, "--skip-comments");
var noParallel = HasFlag(args, "--no-parallel");
var overwrite = HasFlag(args, "--overwrite");
var maxDepth = GetIntOption(args, "--max-depth", 0, min: 0); // 0 == unlimited

var files = args.Where(a => !a.StartsWith("--", StringComparison.Ordinal)).ToArray();
if (files.Length == 0) 
{ 
    Console.Error.WriteLine("No input files specified. Use --help for usage."); 
    return 1;
}

var jsonOptions = new JsonReaderOptions
{
    CommentHandling = skipComments ? JsonCommentHandling.Skip : JsonCommentHandling.Disallow,
    AllowTrailingCommas = allowTrailingCommas,
    MaxDepth = maxDepth == 0 ? 0 : maxDepth
};

var failures = 0;
var swTotal = Stopwatch.StartNew();

if (noParallel || files.Length == 1)
{
    foreach (var f in files) 
        if (!ProcessOne(f, bufferSize, jsonOptions, overwrite)) 
            failures++;
}
else
{
    Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, f =>
    {
        if (!ProcessOne(f, bufferSize, jsonOptions, overwrite)) 
            Interlocked.Increment(ref failures);
    });
}

swTotal.Stop();
Console.WriteLine($"Processed {files.Length - failures}/{files.Length} file(s) in {swTotal.Elapsed}. Failures: {failures}");
return failures == 0 ? 0 : 2;

static bool ProcessOne(string filePath, int bufferSize, JsonReaderOptions readerOptions, bool overwrite)
{
    if (!File.Exists(filePath))
    {
        Console.Error.WriteLine($"File not found: {filePath}"); 
        return false;
    }

    var outputPath = GetFormattedFilePath(filePath);
    if (File.Exists(outputPath) && !overwrite)
    {
        Console.Error.WriteLine($"Output already exists (use --overwrite to replace): {outputPath}"); 
        return false;
    }
    var dir = Path.GetDirectoryName(outputPath); 
    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) 
        Directory.CreateDirectory(dir);

    var sw = Stopwatch.StartNew(); 
    var inputLength = new FileInfo(filePath).Length;

    var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
    var done = false;
    try
    {
        using var input = new FileStream(
            filePath, 
            FileMode.Open, 
            FileAccess.Read, 
            FileShare.Read, 
            bufferSize,
            FileOptions.SequentialScan);
        using var output = new FileStream(
            outputPath, 
            FileMode.Create, 
            FileAccess.Write, 
            FileShare.None, 
            bufferSize,
            FileOptions.SequentialScan);
        // Configure writer to avoid escaping non-ASCII characters (e.g., "Göteborg" stays literal)
        using var writer = new Utf8JsonWriter(output, new JsonWriterOptions { Indented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

        var state = new JsonReaderState(readerOptions);
        var remainderCount = 0; // bytes preserved at start of buffer for next iteration
        while (!done)
        {
            var readInto = remainderCount; // offset
            var toRead = bufferSize - remainderCount;
            var bytesRead = input.Read(buffer, readInto, toRead);
            var isFinalBlock = bytesRead == 0; // no new bytes
            var totalLength = remainderCount + bytesRead;
            if (totalLength == 0 && isFinalBlock)
                break; // empty file

            var span = new ReadOnlySpan<byte>(buffer, 0, totalLength);
            var reader = new Utf8JsonReader(span, isFinalBlock, state);
            while (reader.Read())
            {
                WriteToken(ref reader, writer);
            }

            state = reader.CurrentState;
            var consumed = (int)reader.BytesConsumed;
            remainderCount = totalLength - consumed;
            if (remainderCount > 0)
            {
                // Move leftover bytes to start for next chunk
                Buffer.BlockCopy(buffer, consumed, buffer, 0, remainderCount);
            }

            if (isFinalBlock)
            {
                if (remainderCount != 0)
                {
                    Console.Error.WriteLine(
                        $"JSON appears truncated in {filePath}. {remainderCount} leftover bytes after final block.");
                    return false;
                }

                done = true;
            }
        }

        writer.Flush();
        sw.Stop();
        Console.WriteLine($"Formatted: {filePath} -> {outputPath} ({inputLength / 1024.0:F2} KB) in {sw.Elapsed}");
        return true;
    }
    catch (JsonException jex)
    {
        Console.Error.WriteLine($"JSON error in {filePath}: {jex.Message}");
        return false;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failure processing {filePath}: {ex.Message}");
        return false;
    }
    finally
    {
        ArrayPool<byte>.Shared.Return(buffer);
    }
}

static void WriteToken(ref Utf8JsonReader reader, Utf8JsonWriter writer)
{
    switch (reader.TokenType)
    {
        case JsonTokenType.StartObject: writer.WriteStartObject(); break;
        case JsonTokenType.EndObject: writer.WriteEndObject(); break;
        case JsonTokenType.StartArray: writer.WriteStartArray(); break;
        case JsonTokenType.EndArray: writer.WriteEndArray(); break;
        case JsonTokenType.PropertyName: writer.WritePropertyName(reader.GetString()); break;
        case JsonTokenType.String: writer.WriteStringValue(reader.GetString()); break;
        case JsonTokenType.Number:
            if (reader.HasValueSequence)
            {
                var seq = reader.ValueSequence; 
                var num = Encoding.UTF8.GetString(seq.ToArray()); 
                writer.WriteRawValue(num, skipInputValidation: true);
            }
            else
            {
                var span = reader.ValueSpan; 
                var num = Encoding.UTF8.GetString(span); 
                writer.WriteRawValue(num, skipInputValidation: true);
            }
            break;
        case JsonTokenType.True: writer.WriteBooleanValue(true); break;
        case JsonTokenType.False: writer.WriteBooleanValue(false); break;
        case JsonTokenType.Null: writer.WriteNullValue(); break;
        case JsonTokenType.None:
        case JsonTokenType.Comment:
        default: break; // Comments skipped by reader options
    }
}

static string GetFormattedFilePath(string original)
{
    var dir = Path.GetDirectoryName(original) ?? string.Empty; 
    var name = Path.GetFileNameWithoutExtension(original); 
    var ext = Path.GetExtension(original);
    return string.IsNullOrEmpty(ext) 
        ? Path.Combine(dir, name + "-formatted") 
        : Path.Combine(dir, name + "-formatted" + ext);
}

static bool HasFlag(string[] args, string flag) 
    => args.Any(a => string.Equals(a, flag, StringComparison.OrdinalIgnoreCase));

static int GetIntOption(string[] args, string name, int @default, int min)
{
    for (var i = 0; i < args.Length; i++)
    {
        if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase) 
            && i + 1 < args.Length 
            && !args[i + 1].StartsWith("--"))
        {
            if (int.TryParse(args[i + 1], out var value) && value >= min) 
                return value; 
            Console.Error.WriteLine($"Invalid value for {name}, using default {@default}."); 
            return @default;
        }
    }
    return @default;
}

static void PrintHelp()
{
    Console.WriteLine("JsonFormat - Stream format large JSON files with low memory usage");
    Console.WriteLine("Usage: JsonFormat [options] <file1.json> <file2.json> ...");
    Console.WriteLine("Options:");
    Console.WriteLine("  --buffer-size <bytes>        Buffer size (default 65536)");
    Console.WriteLine("  --allow-trailing-commas      Allow JSON with trailing commas");
    Console.WriteLine("  --skip-comments              Skip comments if present");
    Console.WriteLine("  --max-depth <n>              Max depth (default unlimited)");
    Console.WriteLine("  --no-parallel                Disable parallel processing");
    Console.WriteLine("  --overwrite                  Overwrite existing formatted output");
    Console.WriteLine("  --help                       Show help");
}
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using System.Text;

namespace Akade.Workshops.Performance.Benchmarks.Advanced;

/// <summary>
/// All the following operations can be performed without any memory allocations. Can you reduce the allocations?
/// Hints:
/// - VGhlcmUgYXJlIG90aGVyIFBhcnNlIG92ZXJsb2Fkcywgb25lIG9mIHRoZW0gaXMgd2hhdCB5b3Ugd2FudA==
/// - WW91IGNhbiBzbGljZSBkYXRhIHVzaW5nIFJlYWRPbmx5U3BhbjxUPiB3aXRoIFQgPSBjaGFyIGZvciBzdHJpbmdzIGluc3RlYWQgb2YgY29weWluZyBhbnkgdmFsdWUuIFNwYW5zIGFyZSByZWYgc3RydWN0cyB0aGF0IGxpdmUgb24gdGhlIHN0YWNrLg==
/// - VGFrZSBhIGxvb2sgYXQgU3lzdGVtLlRleHQuSnNvbj8gSXMgdGhlcmUgYW55ICpzdHJ1Y3QqIHRoYXQgcmVhZHMgVXRmOEpzb24/
/// - SXRlcmF0ZSB0aHJvdWdoIHRoZSBqc29uIGFuZCBkaXJlY3RseSB1c2UgdGhlIHZhbHVlcyBvZiB0aGUgcHJvcGVydGllcw==
/// </summary>
[FastJob]
[MemoryDiagnoser]
public class AdvancedStringParsing
{
    private const string inputString = "20230825";
    private readonly string inputStringWithNewLines;

    private readonly byte[] jsonData;

    public AdvancedStringParsing()
    {
        inputStringWithNewLines = string.Join(Environment.NewLine, Enumerable.Range(1, 2048));

        // Generate some utf8 json
        IEnumerable<string> values = Enumerable.Range(0, 1000).Select(i => $"{{\"include\": {(i % 13 == 0 ? "false" : "true")}, \"value\": {i}}}");
        string json = $"[{string.Join(',', values)}]";
        jsonData = Encoding.UTF8.GetBytes(json);
    }

    [Benchmark]
    public DateOnly DateParsing()
    {
        int year = int.Parse(inputString[..4]);
        int month = int.Parse(inputString[4..6]);
        int day = int.Parse(inputString[6..8]);

        return new(year, month, day);
    }

    [Benchmark]
    public int ReadingLines()
    {
        int result = 0;

        foreach (string number in inputStringWithNewLines.Split(Environment.NewLine))
        {
            result += int.Parse(number);
        }

        return result;
    }

    [Benchmark]
    public int ReadingJson()
    {
        string json = Encoding.UTF8.GetString(jsonData);

        var deserialzedArray = JsonConvert.DeserializeAnonymousType(json, new[]
        {
            new
            {
                include = false,
                value = 0
            }
        });

        int result = 0;

        if (deserialzedArray is not null)
        {
            foreach (var item in deserialzedArray.Where(item => item.include))
            {
                result += item.value;
            }
        }

        return result;
    }
}

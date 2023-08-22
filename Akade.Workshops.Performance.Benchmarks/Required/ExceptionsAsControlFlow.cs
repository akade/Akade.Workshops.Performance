using BenchmarkDotNet.Attributes;

namespace Akade.Workshops.Performance.Benchmarks.Required;

/// <summary>
/// This benchmark shows code that uses exceptions as control flow. 
/// Can you make it faster? Can you explain what is going on?
/// Hints 
/// - QXJlIHRoZXJlIG90aGVyIHBhdHRlcm5zIGluIEMjIGZvciBwYXJzaW5nPw== 
/// - LlRyeVBhcnNl
/// </summary>
[FastJob]
public class ExceptionsAsControlFlow
{
    /// <summary>
    /// Params instruct BenchmarkDotNet to run the Benchmark with different values.
    /// </summary>
    [Params("5", "5.2", "Not a number")]
    public string Value { get; set; } = "";

    [Benchmark]
    public object? ParsingIntOrDouble()
    {
        object? result;
        try
        {
            result = int.Parse(Value);
        }
        catch (FormatException)
        {
            try
            {
                result = double.Parse(Value);
            }
            catch (FormatException)
            {
                result = null;
            }
        }

        return result;
    }
}

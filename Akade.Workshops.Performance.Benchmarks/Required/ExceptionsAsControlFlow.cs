using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akade.Workshops.Performance.Benchmarks.Required;

/// <summary>
/// This benchmark shows code that uses exceptions as control flow. 
/// Can you make it faster? Can you explain what is going on?
/// - Hint: Are there other patterns in C# for parsing? 
/// </summary>
[FastJob]
public class ExceptionsAsControlFlow
{

    [Params("5", "5.2", "Not a number")]
    public string Value { get; set; } = "";

    [Benchmark(Baseline = true)]
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

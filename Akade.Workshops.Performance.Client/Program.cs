// See https://aka.ms/new-console-template for more information
using Akade.Workshops.Performance.Client;
using Newtonsoft.Json;
using Spectre.Console;

AnsiConsole.Write(new FigletText("Climate Data Switzerland 2022"));

using HttpClient client = new()
{
    BaseAddress = new("https://localhost:7011/")
};

while (true)
{
    try
    {
        AnsiConsole.Write("Loading available stations:");
        Station[] stations = JsonConvert.DeserializeObject<Station[]>(await client.GetStringAsync("/HistoricalWeather/stations"))
            ?? throw new InvalidOperationException("Failed to load stations");


        Station station = AnsiConsole.Prompt(new SelectionPrompt<Station>().Title("Which station do you want to load average date from?").AddChoices(stations));

        string fromString = AnsiConsole.Prompt(new TextPrompt<string>("From date:").Validate(val => DateOnly.TryParse(val, out _))
                                                                                         .ValidationErrorMessage("[red]That's not a valid date[/]"));

        string toString = AnsiConsole.Prompt(new TextPrompt<string>("From date:").Validate(val => DateOnly.TryParse(val, out _))
                                                                                 .ValidationErrorMessage("[red]That's not a valid date[/]"));

        DateOnly from = DateOnly.Parse(fromString);
        DateOnly to = DateOnly.Parse(toString);

        List<HistoricalData> data = new();

        for (DateOnly current = from; current <= to; current = current.AddDays(1))
        {
            data.Add(JsonConvert.DeserializeObject<HistoricalData>(await client.GetStringAsync($"/HistoricalWeather/stations/{station.Code}/{current:yyyy-MM-dd}"))
                           ?? throw new InvalidOperationException("Failed to load data"));
        }

        AnsiConsole.Write(new Table().Title($"Data for {station.Name} from {from} to {to}")
                                     .AddColumns("Measurement", "Average")
                                     .AddRow("Precipitation", data.Average(x => x.Precipitation)?.ToString() ?? "n/a")
                                     .AddRow("Snow", data.Average(x => x.Snow)?.ToString() ?? "n/a")
                                     .AddRow("TAvg", data.Average(x => x.TAvg)?.ToString() ?? "n/a")
                                     .AddRow("TMax", data.Average(x => x.TMax)?.ToString() ?? "n/a")
                                     .AddRow("TMin", data.Average(x => x.TMin)?.ToString() ?? "n/a")
                                     );
    }
    catch (Exception ex)
    {
        AnsiConsole.WriteException(ex);
    }
}

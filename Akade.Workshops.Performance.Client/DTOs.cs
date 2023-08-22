﻿namespace Akade.Workshops.Performance.Client;

public record Station(string Code, string Name);

public record HistoricalData(Station Station, DateOnly Date, double? Precipitation, double? Snow, double? TMax, double? TMin, double? TAvg);

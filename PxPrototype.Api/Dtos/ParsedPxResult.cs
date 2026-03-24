namespace PxPrototype.Api.Dtos;

public class ParsedPxResult
{
    public string Title { get; set; } = string.Empty;
    public List<ParsedVariable> Variables { get; set; } = new();
    public List<ParsedObservation> Observations { get; set; } = new();
}

public class ParsedVariable
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<ParsedVariableValue> Values { get; set; } = new();
}

public class ParsedVariableValue
{
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public class ParsedObservation
{
    public Dictionary<string, string> Coordinates { get; set; } = new();
    public decimal? Value { get; set; }
    public string? Status { get; set; }
}

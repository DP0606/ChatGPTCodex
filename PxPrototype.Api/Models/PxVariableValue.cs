namespace PxPrototype.Api.Models;

public class PxVariableValue
{
    public Guid Id { get; set; }
    public Guid PxVariableId { get; set; }
    public PxVariable PxVariable { get; set; } = null!;
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int Position { get; set; }
}

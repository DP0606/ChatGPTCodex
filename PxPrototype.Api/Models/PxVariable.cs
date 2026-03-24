namespace PxPrototype.Api.Models;

public class PxVariable
{
    public Guid Id { get; set; }
    public Guid PxDatasetId { get; set; }
    public PxDataset PxDataset { get; set; } = null!;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
    public ICollection<PxVariableValue> Values { get; set; } = new List<PxVariableValue>();
}

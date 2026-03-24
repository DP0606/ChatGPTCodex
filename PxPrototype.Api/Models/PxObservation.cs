namespace PxPrototype.Api.Models;

public class PxObservation
{
    public Guid Id { get; set; }
    public Guid PxDatasetId { get; set; }
    public PxDataset PxDataset { get; set; } = null!;
    public string CoordinateJson { get; set; } = "{}";
    public decimal? Value { get; set; }
    public string? Status { get; set; }
}

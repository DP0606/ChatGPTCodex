namespace PxPrototype.Api.Models;

public class PxDataset
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
    public ICollection<PxVariable> Variables { get; set; } = new List<PxVariable>();
    public ICollection<PxObservation> Observations { get; set; } = new List<PxObservation>();
}

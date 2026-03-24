using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PxPrototype.Api.Data;
using PxPrototype.Api.Models;
using PxPrototype.Api.Services;

namespace PxPrototype.Api.Controllers;

[ApiController]
[Route("api/px")]
public class PxUploadController(PxDbContext db, IPxParserService parser) : ControllerBase
{
    [HttpPost("upload")]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return BadRequest("Datoteka je prazna.");
        }

        if (!Path.GetExtension(file.FileName).Equals(".px", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Podržane su samo .px datoteke.");
        }

        await using var stream = file.OpenReadStream();
        var parsed = await parser.ParseAsync(stream, file.FileName, cancellationToken);

        var dataset = new PxDataset
        {
            FileName = file.FileName,
            Title = parsed.Title,
            UploadedAtUtc = DateTime.UtcNow
        };

        foreach (var (v, varIndex) in parsed.Variables.Select((value, index) => (value, index)))
        {
            var variable = new PxVariable
            {
                Code = v.Code,
                Name = v.Name,
                Position = varIndex
            };

            foreach (var (val, valIndex) in v.Values.Select((value, index) => (value, index)))
            {
                variable.Values.Add(new PxVariableValue
                {
                    Code = val.Code,
                    Label = val.Label,
                    Position = valIndex
                });
            }

            dataset.Variables.Add(variable);
        }

        foreach (var obs in parsed.Observations)
        {
            dataset.Observations.Add(new PxObservation
            {
                CoordinateJson = JsonSerializer.Serialize(obs.Coordinates),
                Value = obs.Value,
                Status = obs.Status
            });
        }

        db.Datasets.Add(dataset);
        await db.SaveChangesAsync(cancellationToken);

        return Ok(new
        {
            dataset.Id,
            dataset.FileName,
            dataset.Title,
            Variables = dataset.Variables.Count,
            Observations = dataset.Observations.Count
        });
    }

    [HttpGet("datasets")]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var datasets = await db.Datasets
            .OrderByDescending(d => d.UploadedAtUtc)
            .Select(d => new
            {
                d.Id,
                d.FileName,
                d.Title,
                d.UploadedAtUtc,
                Variables = d.Variables.Count,
                Observations = d.Observations.Count
            })
            .ToListAsync(cancellationToken);

        return Ok(datasets);
    }
}

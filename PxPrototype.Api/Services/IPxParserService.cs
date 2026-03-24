using PxPrototype.Api.Dtos;

namespace PxPrototype.Api.Services;

public interface IPxParserService
{
    Task<ParsedPxResult> ParseAsync(Stream pxStream, string fileName, CancellationToken cancellationToken = default);
}

using System.Reflection;
using PxPrototype.Api.Dtos;

namespace PxPrototype.Api.Services;

public class PxParserService : IPxParserService
{
    public async Task<ParsedPxResult> ParseAsync(Stream pxStream, string fileName, CancellationToken cancellationToken = default)
    {
        await using var ms = new MemoryStream();
        await pxStream.CopyToAsync(ms, cancellationToken);
        ms.Position = 0;

        var paxiomAssembly = Assembly.Load("PCAxis.Paxiom");
        var builderType = paxiomAssembly.GetType("PCAxis.Paxiom.PXFileBuilder")
            ?? throw new InvalidOperationException("PXFileBuilder type not found.");

        var builder = Activator.CreateInstance(builderType)
            ?? throw new InvalidOperationException("Unable to create PXFileBuilder instance.");

        object? model = InvokeBuild(builderType, builder, ms, fileName);
        if (model is null)
        {
            throw new InvalidOperationException("PAXIOM parser returned null model.");
        }

        return ExtractModel(model);
    }

    private static object? InvokeBuild(Type builderType, object builder, MemoryStream stream, string fileName)
    {
        var methods = builderType.GetMethods().Where(m => m.Name == "Build").ToList();

        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 2 && parameters[0].ParameterType == typeof(Stream) && parameters[1].ParameterType == typeof(string))
            {
                stream.Position = 0;
                return method.Invoke(builder, new object[] { stream, fileName });
            }

            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Stream))
            {
                stream.Position = 0;
                return method.Invoke(builder, new object[] { stream });
            }
        }

        throw new InvalidOperationException("No compatible PXFileBuilder.Build overload found.");
    }

    private static ParsedPxResult ExtractModel(object model)
    {
        var result = new ParsedPxResult();
        var modelType = model.GetType();

        result.Title = ReadProperty(model, modelType, "Title")?.ToString() ?? "(untitled)";

        if (ReadProperty(model, modelType, "Variables") is System.Collections.IEnumerable variables)
        {
            foreach (var variable in variables)
            {
                var variableType = variable.GetType();
                var parsed = new ParsedVariable
                {
                    Code = ReadProperty(variable, variableType, "Code")?.ToString() ?? string.Empty,
                    Name = ReadProperty(variable, variableType, "Name")?.ToString() ?? string.Empty
                };

                if (ReadProperty(variable, variableType, "Values") is System.Collections.IEnumerable values)
                {
                    foreach (var value in values)
                    {
                        var valueType = value.GetType();
                        parsed.Values.Add(new ParsedVariableValue
                        {
                            Code = ReadProperty(value, valueType, "Code")?.ToString() ?? string.Empty,
                            Label = ReadProperty(value, valueType, "Value")?.ToString()
                                ?? ReadProperty(value, valueType, "Text")?.ToString()
                                ?? string.Empty
                        });
                    }
                }

                result.Variables.Add(parsed);
            }
        }

        if (ReadProperty(model, modelType, "Data") is System.Collections.IEnumerable data)
        {
            foreach (var row in data)
            {
                var rowType = row.GetType();
                var observation = new ParsedObservation
                {
                    Value = decimal.TryParse(ReadProperty(row, rowType, "Value")?.ToString(), out var v) ? v : null,
                    Status = ReadProperty(row, rowType, "Status")?.ToString()
                };

                result.Observations.Add(observation);
            }
        }

        return result;
    }

    private static object? ReadProperty(object source, Type type, string propertyName)
        => type.GetProperty(propertyName)?.GetValue(source);
}

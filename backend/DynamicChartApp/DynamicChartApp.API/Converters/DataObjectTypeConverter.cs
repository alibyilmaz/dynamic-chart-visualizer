using System.Text.Json;
using System.Text.Json.Serialization;
using DynamicChartApp.Domain.Enums;

namespace DynamicChartApp.API.Converters;

public class DataObjectTypeConverter : JsonConverter<DataObjectType>
{
    public override DataObjectType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "View" => DataObjectType.View,
            "SP" => DataObjectType.Procedure,
            "Function" => DataObjectType.Function,
            _ => throw new JsonException($"Unable to convert \"{value}\" to {nameof(DataObjectType)}")
        };
    }

    public override void Write(Utf8JsonWriter writer, DataObjectType value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            DataObjectType.View => "View",
            DataObjectType.Procedure => "SP",
            DataObjectType.Function => "Function",
            _ => throw new JsonException($"Unable to convert {value} to string")
        };
        writer.WriteStringValue(stringValue);
    }
}

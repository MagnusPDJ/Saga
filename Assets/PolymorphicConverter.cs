using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saga.Assets
{
    public class PolymorphicConverter<TBase> : JsonConverter<TBase>
    {
        private readonly Dictionary<string, Type> _typeMap;
        private readonly Dictionary<Type, string> _discriminatorMap;

        public PolymorphicConverter() {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(TBase).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => new {
                    Type = t,
                    Discriminator = t.GetCustomAttribute<DiscriminatorAttribute>()?.Name ?? t.Name
                })
                .ToList();

            _typeMap = types.ToDictionary(x => x.Discriminator, x => x.Type, StringComparer.OrdinalIgnoreCase);
            _discriminatorMap = types.ToDictionary(x => x.Type, x => x.Discriminator);
        }

        public override TBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("Type", out var typeProperty))
                throw new JsonException("Missing 'Type' property for polymorphic deserialization.");

            var discriminator = typeProperty.GetString() ?? throw new JsonException("Discriminator property cannot be null.");

            if (!_typeMap.TryGetValue(discriminator, out var targetType))
                throw new JsonException($"Unknown discriminator '{discriminator}' for {typeof(TBase).Name}");

            var result = JsonSerializer.Deserialize(root.GetRawText(), targetType, options) ?? throw new JsonException($"Failed to deserialize {targetType.Name}.");
            return (TBase)result;
        }

        public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options) {
            if (value is not null) {
                var type = value.GetType();
                if (!_discriminatorMap.TryGetValue(type, out var discriminator))
                    throw new JsonException($"Type {type.Name} is not registered with a discriminator.");

                // Serialize the object normally
                var json = JsonSerializer.Serialize(value, type, options);

                using var doc = JsonDocument.Parse(json);

                writer.WriteStartObject();
                writer.WriteString("Type", discriminator); // Inject discriminator

                foreach (var property in doc.RootElement.EnumerateObject()) {
                    property.WriteTo(writer);
                }

                writer.WriteEndObject();
            }
        }
    }

}

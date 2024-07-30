using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZBRA.Maybe.Serialization
{
    internal class MaybeJsonConverter<T> : JsonConverter<Maybe<T>>
    {
        public override Maybe<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return Maybe<T>.Nothing;

            var result = JsonSerializer.Deserialize<T>(ref reader, options);
            return result.ToMaybe();
        }

        public override void Write(Utf8JsonWriter writer, Maybe<T> value, JsonSerializerOptions options)
        {
            switch (value.HasValue)
            {
                case true:
                    JsonSerializer.Serialize(writer, value.Value, typeof(T), options);
                    break;

                default:
                    JsonSerializer.Serialize(writer, null, typeof(object), options);
                    break;
            }
        }
    }
}
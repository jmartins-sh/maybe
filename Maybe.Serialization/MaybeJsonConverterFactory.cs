using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZBRA.Maybe.Serialization
{
    public class MaybeJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType) return false;

            return typeToConvert.GetGenericTypeDefinition() == typeof(Maybe<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var wrappedType = typeToConvert.GetGenericArguments()[0];

            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(MaybeJsonConverter<>).MakeGenericType(wrappedType)
            );

            return converter;
        }
    }
}
using AutoFixture;
using System.Text.Encodings.Web;
using System.Text.Json;
using FluentAssertions;
using ZBRA.Maybe;
using ZBRA.Maybe.Serialization;

namespace Maybe.Serialization.Test;

public class MaybeJsonConverter
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    private readonly Fixture _fixture = new();

    public MaybeJsonConverter()
    {
        _serializerOptions.Converters.Add(new MaybeJsonConverterFactory());
    }

    [Fact]
    public void Deserialize_JsonInteger_ShouldBeEqualsToMaybeInt()
    {
        var maybeValue = _fixture.Create<int>().ToMaybe();
        var jsonString = maybeValue.ToString();
        var result = JsonSerializer.Deserialize<Maybe<int>>(jsonString, _serializerOptions);

        result.Value.Should().Be(maybeValue.Value);
    }

    [Fact]
    public void Deserialize_JsonDouble_ShouldBeEqualsToMaybeDouble()
    {
        var maybeValue = _fixture.Create<double>().ToMaybe();
        var jsonString = maybeValue.ToString();
        var result = JsonSerializer.Deserialize<Maybe<double>>(jsonString, _serializerOptions);

        result.Value.Should().Be(maybeValue.Value);
    }

    [Fact]
    public void Deserialize_JsonBoolean_ShouldBeEqualsToMaybeBoolean()
    {
        var maybeValue = false.ToMaybe();
        // C# converts boolean values to title case (True/False) when calling ToString(), which conflicts with the default JSON serialization behavior,
        // leading to an exception.
        var jsonString = "false";
        var result = JsonSerializer.Deserialize<Maybe<bool>>(jsonString, _serializerOptions);

        result.Value.Should().Be(maybeValue.Value);
    }

    [Fact]
    public void Deserialize_JsonString_ShouldBeEqualsToMaybeString()
    {
        var stringValue = _fixture.Create<string>();
        var maybeValue = stringValue.ToMaybe();
        var jsonString = $"\"{stringValue}\"";
        var result = JsonSerializer.Deserialize<Maybe<string>>(jsonString, _serializerOptions);

        result.Value.Should().Be(maybeValue.Value);
    }

    [Fact]
    public void SerializeAndDeserialize_JsonNullInt_ShouldWorkAsExpected()
    {
        var maybeValue = Maybe<int>.Nothing;
        var temp = JsonSerializer.Serialize(maybeValue, _serializerOptions);
        var result = JsonSerializer.Deserialize<Maybe<int>>(temp, _serializerOptions);

        result.HasValue.Should().BeFalse();
    }

    [Fact]
    public void SerializeAndDeserialize_JsonNullDouble_ShouldWorkAsExpected()
    {
        var maybeValue = Maybe<double>.Nothing;
        var temp = JsonSerializer.Serialize(maybeValue, _serializerOptions);
        var result = JsonSerializer.Deserialize<Maybe<double>>(temp, _serializerOptions);

        result.HasValue.Should().BeFalse();
    }

    [Fact]
    public void SerializeAndDeserialize_JsonNullBoolean_ShouldWorkAsExpected()
    {
        var maybeValue = Maybe<bool>.Nothing;
        var temp = JsonSerializer.Serialize(maybeValue, _serializerOptions);
        var result = JsonSerializer.Deserialize<Maybe<bool>>(temp, _serializerOptions);

        result.HasValue.Should().BeFalse();
    }

    [Fact]
    public void SerializeAndDeserialize_JsonNullString_ShouldWorkAsExpected()
    {
        var maybeValue = Maybe<string>.Nothing;
        var temp = JsonSerializer.Serialize(maybeValue, _serializerOptions);
        var result = JsonSerializer.Deserialize<Maybe<string>>(temp, _serializerOptions);

        result.HasValue.Should().BeFalse();
    }

    [Fact]
    public void ObjString_Maybe()
    {
        var jsonString = "{\"name\":\"João\",\"age\":20,\"height\":2.3,\"sad\":false}";
        var normalObject = new NormalObject()
        {
            Name = "João",
            Age = 20,
            Height = 2.3,
            Sad = false,
        };

        var result = JsonSerializer.Serialize(normalObject, _serializerOptions);

        result.Should().Be(jsonString);
    }

    [Fact]
    public void ListObjString_Maybe()
    {
        var jsonString = "{\"name\":\"João\",\"age\":20,\"height\":2.3,\"sad\":false}";
        var normalObject = new NormalObject()
        {
            Name = "João",
            Age = 20,
            Height = 2.3,
            Sad = false
        };

        var result = JsonSerializer.Serialize(normalObject, _serializerOptions);

        result.Should().Be(jsonString);
    }

    [Fact]
    public void String_Maybe()
    {
        var jsonString = "{\"name\":\"João\",\"age\":20,\"height\":2.3,\"sad\":false}";
        var maybeValue = new NormalObject()
        {
            Name = "João",
            Age = 20,
            Height = 2.3,
            Sad = false
        }.ToMaybe();

        var result = JsonSerializer.Serialize(maybeValue, _serializerOptions);

        result.Should().Be(jsonString);
    }

    [Fact]
    public void Boolean_Maybe()
    {
        var jsonString = "{\"name\":\"João\",\"age\":20,\"height\":2.3,\"sad\":false}";
        var maybeValue = new NormalObject()
        {
            Name = "João",
            Age = 20,
            Height = 2.3,
            Sad = false
        }.ToMaybe();

        var result = JsonSerializer.Serialize(maybeValue, _serializerOptions);

        result.Should().Be(jsonString);
    }

    [Fact]
    public void Decimal_Maybe()
    {
        var jsonString = "{\"name\":\"João\",\"age\":20,\"height\":2.3,\"sad\":false}";
        var maybeValue = new NormalObject()
        {
            Name = "João",
            Age = 20,
            Height = 2.3,
            Sad = false
        }.ToMaybe();

        var result = JsonSerializer.Serialize(maybeValue, _serializerOptions);

        result.Should().Be(jsonString);
    }

    [Fact]
    public void Test1()
    {
        var jsonString =
            "{\"field\":{\"name\":\"Martins\",\"age\":10,\"height\":1.2,\"sad\":true}}";
        var maybeValue = new NormalObjectNestedMaybe()
        {
            Field = new NormalObject()
            {
                Name = "Martins",
                Age = 10,
                Height = 1.2,
                Sad = true,
            }
        }.ToMaybe();

        var result = JsonSerializer.Serialize(maybeValue, _serializerOptions);

        result.Should().Be(jsonString);
    }


    [Fact]
    public void TestNullInside()
    {
        var jsonString = "{\"field\":null}";

        var maybeValue = new NormalObjectNestedMaybe()
        {
            Field = Maybe<NormalObject>.Nothing,
        }.ToMaybe();

        var result = JsonSerializer.Serialize(maybeValue, _serializerOptions);

        result.Should().Be(jsonString);
    }

    [Fact]
    public void TestNullInsideFields()
    {
        var jsonString =
            "{\"name\":null,\"age\":null}";

        var maybeValue = new NullField()
        {
            Name = Maybe<string>.Nothing,
            Age = Maybe<int>.Nothing,
        }.ToMaybe();

        var result = JsonSerializer.Serialize(maybeValue, _serializerOptions);

        result.Should().Be(jsonString);
    }

    [Fact]
    public void Integer_Maybes()
    {
        var jsonString = "null";
        var result = JsonSerializer.Deserialize<Maybe<int>>(jsonString, _serializerOptions);

        result.Should().Be(Maybe<int>.Nothing);
        result.HasValue.Should().BeFalse();
    }

    [Fact]
    public void Integer_MaybesList()
    {
        var mock = new NormalObjectNestedMaybeListMaybe()
        {
            Field = Maybe<List<Maybe<NormalObject>>>.Nothing,
        };

        var jsonString =
            "{\"field\":null}";

        var result = JsonSerializer.Serialize(mock, _serializerOptions);

        result.Should().Be(jsonString);
    }

    [Fact]
    public void Integer_MaybesMaybeList()
    {
        var mockNormalObject = _fixture.Create<NormalObject>();
        var mockNormalObjectString = JsonSerializer.Serialize(mockNormalObject, _serializerOptions);
        var mock = new NormalObjectNestedListMaybe()
        {
            Field = [Maybe<NormalObject>.Nothing, mockNormalObject.ToMaybe()],
        };

        var jsonString = string.Concat("{\"field\":[null,", mockNormalObjectString, "]}");
        var result = JsonSerializer.Serialize(mock, _serializerOptions);

        result.Should().Be(jsonString);
    }
}

public class NullField
{
    public Maybe<string> Name { get; set; }
    public Maybe<int> Age { get; set; }
}

public class NormalObject
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Height { get; set; }
    public bool Sad { get; set; }
}

public class NormalObjectNestedMaybe
{
    public Maybe<NormalObject> Field { get; set; }
}

public class NormalObjectNestedListMaybe
{
    public List<Maybe<NormalObject>> Field { get; set; }
}

public class NormalObjectNestedMaybeListMaybe
{
    public Maybe<List<Maybe<NormalObject>>> Field { get; set; }
}

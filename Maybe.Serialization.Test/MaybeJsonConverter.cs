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

    public MaybeJsonConverter()
    {
        _serializerOptions.Converters.Add(new MaybeJsonConverterFactory());
    }

    [Fact]
    public void Deserialize_JsonInteger_ShouldBeEqualsToMaybeInt()
    {
        var maybeValue = 20.ToMaybe();
        var jsonString = maybeValue.ToString();
        var result = JsonSerializer.Deserialize<Maybe<int>>(jsonString, _serializerOptions);

        result.Value.Should().Be(maybeValue.Value);
    }

    [Fact]
    public void Deserialize_JsonDouble_ShouldBeEqualsToMaybeDouble()
    {
        var maybeValue = 20.2.ToMaybe();
        var jsonString = maybeValue.ToString();
        var result = JsonSerializer.Deserialize<Maybe<double>>(jsonString, _serializerOptions);

        result.Value.Should().Be(maybeValue.Value);
    }

    [Fact]
    public void Deserialize_JsonBoolean_ShouldBeEqualsToMaybeBoolean()
    {
        var maybeValue = false.ToMaybe();
        var jsonString = "false";
        var result = JsonSerializer.Deserialize<Maybe<bool>>(jsonString, _serializerOptions);

        result.Value.Should().Be(maybeValue.Value);
    }

    [Fact]
    public void Deserialize_JsonString_ShouldBeEqualsToMaybeString()
    {
        var maybeValue = "João".ToMaybe();
        var jsonString = "\"João\"";
        var result = JsonSerializer.Deserialize<Maybe<string>>(jsonString, _serializerOptions);

        result.Value.Should().Be(maybeValue.Value);
    }

    [Fact]
    public void ObjString_Maybe()
    {
        var jsonString = "{\"name\":\"João\",\"age\":20,\"height\":2.3,\"sad\":false}";
        var normalObject = new Teste()
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
    public void ListObjString_Maybe()
    {
        var jsonString = "{\"name\":\"João\",\"age\":20,\"height\":2.3,\"sad\":false}";
        var normalObject = new Teste()
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
        var maybeValue = new Teste()
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
        var maybeValue = new Teste()
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
        var maybeValue = new Teste()
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
            "{\"name\":\"João\",\"age\":20,\"height\":2.3,\"sad\":false,\"field\":{\"name\":\"Martins\",\"age\":10,\"height\":1.2,\"sad\":true}}";
        var maybeValue = new MaybeInsideTeste()
        {
            Name = "João",
            Age = 20,
            Height = 2.3,
            Sad = false,
            Field = new Teste()
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
        var jsonString =
            "{\"name\":\"João\",\"age\":20,\"height\":2.3,\"sad\":false,\"field\":null}";

        var maybeValue = new MaybeInsideTeste()
        {
            Name = "João",
            Age = 20,
            Height = 2.3,
            Sad = false,
            Field = Maybe<Teste>.Nothing,
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
        var mock = new MaybeListInsideTeste()
        {
            Name = "teste",
            Field = Maybe<List<Maybe<Teste>>>.Nothing,
        };

        var jsonString =
            "{\"name\":\"teste\",\"field\":null}";

        var result = JsonSerializer.Serialize(mock, _serializerOptions);

        result.Should().Be(jsonString);
    }
}

public class NullField
{
    public Maybe<string> Name { get; set; }
    public Maybe<int> Age { get; set; }
}

public class Teste
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Height { get; set; }
    public bool Sad { get; set; }
}

public class MaybeInsideTeste
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Height { get; set; }
    public bool Sad { get; set; }
    public Maybe<Teste> Field { get; set; }
}

public class ListMaybeInsideTeste
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Height { get; set; }
    public bool Sad { get; set; }
    public List<Maybe<Teste>> Field { get; set; }
}

public class MaybeListInsideTeste
{
    public string Name { get; set; }
    public Maybe<List<Maybe<Teste>>> Field { get; set; }
}
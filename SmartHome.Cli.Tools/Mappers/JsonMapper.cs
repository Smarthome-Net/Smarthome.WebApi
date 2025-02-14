using System.Text.Json;
using MongoDB.Bson;

namespace SmartHome.Cli.Tools.Mappers;

public static class JsonMapper
{
    public static IEnumerable<BsonDocument> AsBsonDocuments(this IEnumerable<IDictionary<string, JsonElement>> dictionaries)
    {
        return dictionaries.Select(dict => dict.AsBsonDocument()).ToList();
    }

    private static BsonDocument AsBsonDocument(this IDictionary<string, JsonElement> dict)
    {
        var document = new BsonDocument();
        foreach (var item in dict)
        {
            document.Add(item.AsBsonElement());
        }
        return document;
    }

    private static BsonElement AsBsonElement(this KeyValuePair<string, JsonElement> keyPair)
    {
        var bsonValue = keyPair.Value.AsBsonValue(keyPair.Key);
        return new BsonElement(keyPair.Key, bsonValue);
    }

    private static BsonValue AsBsonValue(this JsonElement jsonValue, string name)
    {   
        return jsonValue.ValueKind switch
        {
            JsonValueKind.Object when string.Equals(name, "_id") => BsonObjectId.Create(jsonValue.EnumerateObject().Select(item => item.Value.GetString()).FirstOrDefault()),
            JsonValueKind.Object => new BsonDocument(jsonValue.EnumerateObject().Select(item => new BsonElement(item.Name, item.Value.AsBsonValue(item.Name)))),
            JsonValueKind.Array => new BsonArray(jsonValue.EnumerateArray().Select(item => item.AsBsonValue(string.Empty))),
            JsonValueKind.String => BsonValue.Create(jsonValue.GetString()),
            JsonValueKind.Number => BsonValue.Create(jsonValue.GetInt32()),
            JsonValueKind.True => BsonValue.Create(true),
            JsonValueKind.False => BsonValue.Create(false),
            _ => BsonValue.Create(null),
        };
    }
}

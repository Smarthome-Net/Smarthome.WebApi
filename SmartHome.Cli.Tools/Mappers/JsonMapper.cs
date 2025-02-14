using System.Text.Json;
using MongoDB.Bson;

namespace SmartHome.Cli.Tools.Mappers;

public static class JsonMapper
{
    public static IEnumerable<BsonDocument> AsBsonDocuments(this IEnumerable<IDictionary<string, JsonElement>> dicts)
    {
        var bsonDocuments = new List<BsonDocument>();
        foreach (var dict in dicts)
        {
            bsonDocuments.Add(dict.AsBsonDocument());
        }
        return bsonDocuments;
    }

    public static BsonDocument AsBsonDocument(this IDictionary<string, JsonElement> dict)
    {
        var document = new BsonDocument();
        foreach (var item in dict)
        {
            document.Add(item.AsBsonValue());
        }
        return document;
    }

    public static BsonElement AsBsonValue(this KeyValuePair<string, JsonElement> keyPair)
    {
        var bsonValue = keyPair.Value.ValueKind switch
        {
            JsonValueKind.Object when string.Equals(keyPair.Key, "_id") => BsonObjectId.Create(keyPair.Value.EnumerateObject().Select(item => item.Value.GetString()).FirstOrDefault()),
            JsonValueKind.Object => new BsonDocument(keyPair.Value.EnumerateObject().Select(item => new BsonElement(item.Name, item.Value.AsBsonValue()))),
            JsonValueKind.Array => new BsonArray(keyPair.Value.EnumerateArray().Select(item => item.AsBsonValue())),
            JsonValueKind.String => BsonValue.Create(keyPair.Value.GetString()),
            JsonValueKind.Number => BsonValue.Create(keyPair.Value.GetInt32()),
            JsonValueKind.True => BsonValue.Create(true),
            JsonValueKind.False => BsonValue.Create(false),
            _ => BsonValue.Create(null),
        };
        return new BsonElement(keyPair.Key, bsonValue);
    }
    
    public static BsonValue AsBsonValue(this JsonElement jsonValue)
    {   
        return jsonValue.ValueKind switch
        {
            JsonValueKind.Object => new BsonDocument(jsonValue.EnumerateObject().Select(item => new BsonElement(item.Name, item.Value.AsBsonValue()))),
            JsonValueKind.Array => new BsonArray(jsonValue.EnumerateArray().Select(item => item.AsBsonValue())),
            JsonValueKind.String => BsonValue.Create(jsonValue.GetString()),
            JsonValueKind.Number => BsonValue.Create(jsonValue.GetInt32()),
            JsonValueKind.True => BsonValue.Create(true),
            JsonValueKind.False => BsonValue.Create(false),
            _ => BsonValue.Create(null),
        };
    }
}

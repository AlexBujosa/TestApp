using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace EndPointProject;
public class UserModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
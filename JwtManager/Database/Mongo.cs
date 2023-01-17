namespace EndPointProject.DB;
using MongoDB.Bson;
using MongoDB.Driver;

public class MongoDb
{
    static string connectionString =
        "mongodb+srv://Bujosa2023:hCG4gdj82IZb1BXp@cluster0.0b42w.mongodb.net/?retryWrites=true&w=majority";
    static string dbName = "EndPointDB";
    static string collectionName = "user";

    public static MongoClient? client;

    public static IMongoDatabase? db;

    public static IMongoCollection<UserModel>? userCollection;
    public MongoDb()
    {
        client = new MongoClient(connectionString);
        db = client.GetDatabase(dbName);
        userCollection = db.GetCollection<UserModel>(collectionName);
    }

    public async void InsertOne(string Username, string Password)
    {
        var user = new UserModel { Username = Username, Password = Password };
        await userCollection!.InsertOneAsync(user);
    }

    public async Task<bool> GetUser(string username, string password)
    {
        var filterByUsername = Builders<UserModel>.Filter.Eq(M => M.Username, username);
        var user = await userCollection.Find(filterByUsername).FirstOrDefaultAsync();
        if (user == null) return false;
        bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
        return isValid;
    }




}



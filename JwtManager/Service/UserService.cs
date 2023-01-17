namespace EndPointProject.Service;
using EndPointProject.Interface;
using EndPointProject.DB;
using EndPointProject.Dto;

public class UserService : IUserService
{
    private readonly MongoDb _db;
    public UserService(MongoDb db)
    {
        _db = db;
    }
    public async Task<bool> IsValidUser(UserDto usrDto)
    {
        (string username, string password) = (usrDto.Username!, usrDto.Password!);
        bool isValid = await _db.GetUser(username, password);
        return isValid;
    }
    public void InsertOne(string username, string hashPassword)
    {
        _db.InsertOne(username, hashPassword);
    }

}
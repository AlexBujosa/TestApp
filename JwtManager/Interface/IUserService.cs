namespace EndPointProject.Interface;
using EndPointProject.Dto;
public interface IUserService
{
    Task<bool> IsValidUser(UserDto userDto);
    void InsertOne(string username, string hashPassword);
}
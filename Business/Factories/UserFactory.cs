using Business.Dtos;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Factories;

public class UserFactory(IPasswordHasher<UserEntity> passwordHasher)
{
    private readonly IPasswordHasher<UserEntity> _passwordHasher = passwordHasher;

    //Github Copilot helped with the PasswordHasher
    public UserEntity CreateUserEntity(UserDto dto)
    {
        var user = new UserEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email,
            Role = dto.Role
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        return user;
    }

    public static User CreateUserModel(UserEntity entity) => new User
    {
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Email = entity.Email!,
        Role = entity.Role
    };
    
    public static UserEntity UpdateUser(UserEntity userEntity, UserUpdateDto userUpdate)
    {
        userEntity.FirstName = userUpdate.FirstName ?? userEntity.FirstName;
        userEntity.LastName = userUpdate.LastName ?? userEntity.LastName;
        userEntity.Email = userUpdate.Email ?? userEntity.Email;
        userEntity.Role = userUpdate.Role ?? userEntity.Role;
        userEntity.PasswordHash = userUpdate.Password != null
            ? new PasswordHasher<UserEntity>().HashPassword(userEntity, userUpdate.Password)
            : userEntity.PasswordHash;

        return userEntity;
    }
}

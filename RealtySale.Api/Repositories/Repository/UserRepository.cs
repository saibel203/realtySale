using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.Data;
using RealtySale.Shared.DTOs;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.Repository;

public class UserRepository : IUserRepository
{
    private readonly RealtySaleContext _context;

    public UserRepository(RealtySaleContext context)
    {
        _context = context;
    }

    public async Task<UserRepositoryResponse> GetUserDataAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        
        if (user is null)
            return new()
            {
                Message = "User not Found",
                IsSuccess = false
            };

        return new()
        {
            Message = "User found successfully",
            IsSuccess = true,
            User = user
        };
    }

    public async Task<UserRepositoryResponse> GetUserDataAsync(long id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        if (user is null)
            return new()
            {
                Message = "User not Found",
                IsSuccess = false
            };
        
        return new()
        {
            Message = "User found successfully",
            IsSuccess = true,
            User = user
        };
    }

    public async Task<UserRepositoryResponse> AuthenticateAsync(string username, string passwordText)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        if (user is null || user.PasswordKey is null)
            return new()
            {
                Message = "Invalid some properties",
                IsSuccess = false
            };

        if (!MatchPasswordHash(passwordText, user.Password, user.PasswordKey))
            return new()
            {
                Message = "Password hashes do not match",
                IsSuccess = false
            };

        return new()
        {
            Message = "Authenticate was successfully",
            IsSuccess = true,
            User = user
        };
    }

    public async Task<UserRepositoryResponse> RegisterAsync(string username, string password, string email)
    {
        using var hmac = new HMACSHA512();
        byte[] passwordKey = hmac.Key;
        byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        User user = new()
        {
            Username = username,
            Password = passwordHash,
            PasswordKey = passwordKey,
            Email = email
        };
        
        await _context.Users.AddAsync(user);

        return new()
        {
            Message = "Register was successfully",
            IsSuccess = true
        };
    }

    public async Task<UserRepositoryResponse> IsUserExistsAsync(string username)
    {
        var result = await _context.Users.AnyAsync(x => x.Username == username);

        if (result)
            return new()
            {
                Message = "User already exists, please try something else",
                IsSuccess = true
            };
        
        return new()
        {
            Message = "User does not exist",
            IsSuccess = false
        };
    }

    public async Task<UserRepositoryResponse> ChangePasswordAsync(string username, string oldPassword, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        
        if (user is null || user.PasswordKey is null)
            return new()
            {
                Message = "Invalid some properties",
                IsSuccess = false
            };
        
        if (!MatchPasswordHash(oldPassword, user.Password, user.PasswordKey))
            return new()
            {
                Message = "TeST ASD",
                IsSuccess = false
            };
        
        using var hmac = new HMACSHA512();
        byte[] passwordKey = hmac.Key;
        byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));

        user.Password = passwordHash;
        user.PasswordKey = passwordKey;

        _context.Users.Update(user);

        return new()
        {
            Message = "OK",
            IsSuccess = true
        };
    }

    public async Task<UserRepositoryResponse> AddFavouritePropertyAsync(UsernameDto user, int id)
    {
        var property = await _context.Properties.FirstOrDefaultAsync(x => x.Id == id);
        var currentUserResult = await GetUserDataAsync(user.Username);
        var currentUser = currentUserResult.User!;

        if (property is null)
            return new()
            {
                Message = "Property not found",
                IsSuccess = false
            };
        
        if (!currentUserResult.IsSuccess)
            return new()
            {
                Message = "User not found!",
                IsSuccess = false
            };

        var isElementContains =  await _context.FavouriteProperties
            .AnyAsync(el => el.PropertyId == id && el.UserId == currentUser.Id);

        var favourite = new UserProperty
        {
            PropertyId = id,
            UserId = currentUser.Id
        };

        if (isElementContains)
        {
            _context.FavouriteProperties.Remove(favourite);

            return new()
            {
                Message = "Property remove from Favourite list successfully",
                IsSuccess = true
            };
        }

        await _context.FavouriteProperties.AddAsync(favourite);

        return new()
        {
            Message = "Property add to Favourite list successfully",
            IsSuccess = true,
            Favourite = favourite
        };
    }

    public async Task<UserRepositoryResponse> IsPropertyFavouriteAsync(UsernameDto user, int id)
    {
        var property = await _context.Properties.FirstOrDefaultAsync(x => x.Id == id);
        var currentUser = await GetUserDataAsync(user.Username);

        if (property is null)
            return new()
            {
                Message = "Property not found",
                IsSuccess = false
            };

        if (!currentUser.IsSuccess)
            return new()
            {
                Message = "User not found",
                IsSuccess = false
            };
        
        var propertiesList = _context.FavouriteProperties.Include(x => x.User)
            .Where(x => x.UserId == currentUser.User!.Id).Select(x => x.Property).ToListAsync();
        bool isProperty = propertiesList.Result.Contains(property);
        
        return new()
        {
            Message = "Property in favourite list: " + isProperty,
            IsSuccess = true,
            IsPropertyInFavourite = isProperty
        };
    }

    private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
    {
        using var hmac = new HMACSHA512(passwordKey);
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordText));
        
        for (int i = 0; i < passwordHash.Length; i++)
            if (passwordHash[i] != password[i])
                return false;

        return true;
    }
}

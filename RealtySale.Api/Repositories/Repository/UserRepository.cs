﻿using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.Data;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.Repository;

public class UserRepository : IUserRepository
{
    private readonly RealtySaleContext _context;

    public UserRepository(RealtySaleContext context)
    {
        _context = context;
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

    public async Task<UserRepositoryResponse> RegisterAsync(string username, string password)
    {
        using var hmac = new HMACSHA512();
        byte[] passwordKey = hmac.Key;
        byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        User user = new();
        user.Username = username;
        user.Password = passwordHash;
        user.PasswordKey = passwordKey;

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

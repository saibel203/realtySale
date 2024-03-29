﻿namespace RealtySale.Shared.DTOs;

public class UserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? NewPassword { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? TelegramLink { get; set; } = string.Empty;
    public string? InstagramLink { get; set; } = string.Empty;
    public string? FacebookLink { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? UserImage { get; set; } = string.Empty;
}

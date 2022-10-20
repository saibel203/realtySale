﻿using System.ComponentModel.DataAnnotations;

namespace RealtySale.Api.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

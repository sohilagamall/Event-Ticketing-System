using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EventTicketing.Infrastructure.Identity.Jwt;

//it maps the Jwt configuration section into a strongly typed C# object
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    [Required]
    public string Issuer { get; init; } = string.Empty;
    [Required]
    public string Audience { get; init; } = string.Empty;
    [Required]
    [MinLength(32)]
    public string Key { get; init; } = string.Empty;

    [Range(1, 1440)]
    public int AccessTokenLifetimeMinutes { get; init; } = 60;
}

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

namespace RichbetsResurrected.Entities.Identity.Models;

public class AppUser : IdentityUser<int>
{
    
    private string _passwordHash;
    private string _email;
    private int _id;
    private string _concurrencyStamp;
    private bool _emailConfirmed;
    private bool _lockoutEnabled;
    private DateTimeOffset? _lockoutEnd;
    private string _normalizedEmail;
    private string _phoneNumber;
    private string _securityStamp;
    private string _userName;
    private int _accessFailedCount;
    private string _normalizedUserName;
    private bool _phoneNumberConfirmed;
    private bool _twoFactorEnabled;

    [JsonIgnore]
    public override string PasswordHash
    {
        get => _passwordHash;
        set => _passwordHash = value;
    }
    
    [JsonIgnore]
    public override string Email
    {
        get => _email;
        set => _email = value;
    }

    public override int Id
    {
        get => _id;
        set => _id = value;
    }
    [JsonIgnore]
    public override string ConcurrencyStamp
    {
        get => _concurrencyStamp;
        set => _concurrencyStamp = value;
    }
    [JsonIgnore]
    public override bool EmailConfirmed
    {
        get => _emailConfirmed;
        set => _emailConfirmed = value;
    }
    [JsonIgnore]
    public override bool LockoutEnabled
    {
        get => _lockoutEnabled;
        set => _lockoutEnabled = value;
    }
    [JsonIgnore]
    public override DateTimeOffset? LockoutEnd
    {
        get => _lockoutEnd;
        set => _lockoutEnd = value;
    }
    [JsonIgnore]
    public override string NormalizedEmail
    {
        get => _normalizedEmail;
        set => _normalizedEmail = value;
    }
    [JsonIgnore]
    public override string PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = value;
    }
    [JsonIgnore]
    public override string SecurityStamp
    {
        get => _securityStamp;
        set => _securityStamp = value;
    }

    public override string UserName
    {
        get => _userName;
        set => _userName = value;
    }
    [JsonIgnore]
    public override int AccessFailedCount
    {
        get => _accessFailedCount;
        set => _accessFailedCount = value;
    }
    [JsonIgnore]
    public override string NormalizedUserName
    {
        get => _normalizedUserName;
        set => _normalizedUserName = value;
    }
    [JsonIgnore]
    public override bool PhoneNumberConfirmed
    {
        get => _phoneNumberConfirmed;
        set => _phoneNumberConfirmed = value;
    }
    [JsonIgnore]
    public override bool TwoFactorEnabled
    {
        get => _twoFactorEnabled;
        set => _twoFactorEnabled = value;
    }

    public RichbetUser RichbetUser { get; set; }
}
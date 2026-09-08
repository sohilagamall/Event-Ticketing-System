namespace EventTicketing.Application.Features.Authorization;

public static class ApplicationRoles
{
    public const string Customer = "Customer";
    public const string Admin = "Admin";
    public const string EventOrganizer = "EventOrganizer";

    public static IReadOnlyList<string> AllRoles { get; } =
    [
            Customer,
            Admin,
            EventOrganizer
     ];
}


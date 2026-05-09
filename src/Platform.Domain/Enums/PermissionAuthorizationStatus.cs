namespace Platform.Domain.Enums;

public enum PermissionAuthorizationStatus
{
    Authorized = 1,
    MissingToken = 2,
    InvalidToken = 3,
    Forbidden = 4
}

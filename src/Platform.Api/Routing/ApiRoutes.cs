namespace Platform.Api.Routing;

public static class ApiRoutes
{
    public const string AuthLogin = "/api/auth/login";
    public const string Me = "/api/me";
    public const string Modules = "/api/modules";
    public const string SystemPermissions = "/api/system/permissions";
    public const string Customers = "/api/customers";
    public const string CustomerById = "/api/customers/{customerId:guid}";
    public const string CustomerContacts = "/api/customers/{customerId:guid}/contacts";
    public const string ContactById = "/api/contacts/{contactId:guid}";
}

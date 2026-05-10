using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Auth;
using Platform.Application.Contacts;
using Platform.Application.Customers;
using Platform.Application.Finance;
using Platform.Application.Opportunities;
using Platform.Domain.Catalog;
using Platform.Domain.Enums;
using Platform.Infrastructure;
using Platform.Persistence;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' was not provided.");

builder.Services.AddPersistence(connectionString);
builder.Services.AddInfrastructure();
builder.Services.AddScoped<PermissionEndpointFilter>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost(ApiRoutes.AuthLogin, async (
    LoginRequest request,
    IAuthenticationService authenticationService,
    CancellationToken cancellationToken) =>
{
    var result = await authenticationService.LoginAsync(request, cancellationToken);

    return result.Succeeded
        ? Results.Ok(result.Login)
        : Results.Unauthorized();
})
.WithName(ApiEndpointNames.Login)
.WithOpenApi();

app.MapGet(ApiRoutes.Me, async (
    HttpRequest request,
    IAuthenticationService authenticationService,
    CancellationToken cancellationToken) =>
{
    var accessToken = BearerTokenReader.Read(request);
    if (accessToken is null)
    {
        return Results.Unauthorized();
    }

    var currentUser = await authenticationService.GetCurrentUserAsync(accessToken, cancellationToken);

    return currentUser is not null
        ? Results.Ok(currentUser)
        : Results.Unauthorized();
})
.WithName(ApiEndpointNames.Me)
.WithOpenApi();

app.MapGet(ApiRoutes.Modules, async (
    HttpRequest request,
    IAuthenticationService authenticationService,
    IModuleCatalogService moduleCatalogService,
    CancellationToken cancellationToken) =>
{
    var accessToken = BearerTokenReader.Read(request);
    if (accessToken is null)
    {
        return Results.Unauthorized();
    }

    var currentUser = await authenticationService.GetCurrentUserAsync(accessToken, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var modules = await moduleCatalogService.GetModulesAsync(currentUser.TenantId, cancellationToken);

    return Results.Ok(modules);
})
.RequirePermission(KnownPermissions.CoreModulesView)
.WithName(ApiEndpointNames.Modules)
.WithOpenApi();

app.MapGet(ApiRoutes.Customers, async (
    HttpRequest request,
    IAuthenticationService authenticationService,
    ICustomerService customerService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var customers = await customerService.ListAsync(currentUser.TenantId, cancellationToken);

    return Results.Ok(customers);
})
.RequirePermission(KnownPermissions.CrmCustomersView)
.WithName(ApiEndpointNames.CustomersList)
.WithOpenApi();

app.MapGet(ApiRoutes.CustomerById, async (
    Guid customerId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    ICustomerService customerService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var customer = await customerService.GetByIdAsync(currentUser.TenantId, customerId, cancellationToken);

    return customer is null
        ? Results.NotFound()
        : Results.Ok(customer);
})
.RequirePermission(KnownPermissions.CrmCustomersView)
.WithName(ApiEndpointNames.CustomersGetById)
.WithOpenApi();

app.MapPost(ApiRoutes.Customers, async (
    CreateCustomerRequest customerRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    ICustomerService customerService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await customerService.CreateAsync(currentUser.TenantId, customerRequest, cancellationToken);

    return ToCustomerWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmCustomersCreate)
.WithName(ApiEndpointNames.CustomersCreate)
.WithOpenApi();

app.MapPut(ApiRoutes.CustomerById, async (
    Guid customerId,
    UpdateCustomerRequest customerRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    ICustomerService customerService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await customerService.UpdateAsync(currentUser.TenantId, customerId, customerRequest, cancellationToken);

    return ToCustomerWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmCustomersUpdate)
.WithName(ApiEndpointNames.CustomersUpdate)
.WithOpenApi();

app.MapDelete(ApiRoutes.CustomerById, async (
    Guid customerId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    ICustomerService customerService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await customerService.DeactivateAsync(currentUser.TenantId, customerId, cancellationToken);

    return result.Status == CustomerOperationStatus.Success
        ? Results.NoContent()
        : ToCustomerWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmCustomersDelete)
.WithName(ApiEndpointNames.CustomersDeactivate)
.WithOpenApi();

app.MapGet(ApiRoutes.CustomerContacts, async (
    Guid customerId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IContactService contactService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var contacts = await contactService.ListByCustomerAsync(currentUser.TenantId, customerId, cancellationToken);

    return Results.Ok(contacts);
})
.RequirePermission(KnownPermissions.CrmCustomersView)
.WithName(ApiEndpointNames.ContactsListByCustomer)
.WithOpenApi();

app.MapGet(ApiRoutes.ContactById, async (
    Guid contactId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IContactService contactService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var contact = await contactService.GetByIdAsync(currentUser.TenantId, contactId, cancellationToken);

    return contact is null
        ? Results.NotFound()
        : Results.Ok(contact);
})
.RequirePermission(KnownPermissions.CrmCustomersView)
.WithName(ApiEndpointNames.ContactsGetById)
.WithOpenApi();

app.MapPost(ApiRoutes.CustomerContacts, async (
    Guid customerId,
    CreateContactRequest contactRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IContactService contactService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await contactService.CreateAsync(currentUser.TenantId, customerId, contactRequest, cancellationToken);

    return ToContactWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmCustomersCreate)
.WithName(ApiEndpointNames.ContactsCreate)
.WithOpenApi();

app.MapPut(ApiRoutes.ContactById, async (
    Guid contactId,
    UpdateContactRequest contactRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IContactService contactService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await contactService.UpdateAsync(currentUser.TenantId, contactId, contactRequest, cancellationToken);

    return ToContactWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmCustomersUpdate)
.WithName(ApiEndpointNames.ContactsUpdate)
.WithOpenApi();

app.MapDelete(ApiRoutes.ContactById, async (
    Guid contactId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IContactService contactService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await contactService.DeleteAsync(currentUser.TenantId, contactId, cancellationToken);

    return result.Status == ContactOperationStatus.Success
        ? Results.NoContent()
        : ToContactWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmCustomersDelete)
.WithName(ApiEndpointNames.ContactsDelete)
.WithOpenApi();

app.MapGet(ApiRoutes.CustomerOpportunities, async (
    Guid customerId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IOpportunityService opportunityService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var opportunities = await opportunityService.ListByCustomerAsync(currentUser.TenantId, customerId, cancellationToken);

    return Results.Ok(opportunities);
})
.RequirePermission(KnownPermissions.CrmOpportunitiesView)
.WithName(ApiEndpointNames.OpportunitiesListByCustomer)
.WithOpenApi();

app.MapGet(ApiRoutes.OpportunityById, async (
    Guid opportunityId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IOpportunityService opportunityService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var opportunity = await opportunityService.GetByIdAsync(currentUser.TenantId, opportunityId, cancellationToken);

    return opportunity is null
        ? Results.NotFound()
        : Results.Ok(opportunity);
})
.RequirePermission(KnownPermissions.CrmOpportunitiesView)
.WithName(ApiEndpointNames.OpportunitiesGetById)
.WithOpenApi();

app.MapPost(ApiRoutes.CustomerOpportunities, async (
    Guid customerId,
    CreateOpportunityRequest opportunityRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IOpportunityService opportunityService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await opportunityService.CreateAsync(currentUser.TenantId, customerId, opportunityRequest, cancellationToken);

    return ToOpportunityWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmOpportunitiesManage)
.WithName(ApiEndpointNames.OpportunitiesCreate)
.WithOpenApi();

app.MapPut(ApiRoutes.OpportunityById, async (
    Guid opportunityId,
    UpdateOpportunityRequest opportunityRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IOpportunityService opportunityService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await opportunityService.UpdateAsync(currentUser.TenantId, opportunityId, opportunityRequest, cancellationToken);

    return ToOpportunityWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmOpportunitiesManage)
.WithName(ApiEndpointNames.OpportunitiesUpdate)
.WithOpenApi();

app.MapPost(ApiRoutes.OpportunityWon, async (
    Guid opportunityId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IOpportunityService opportunityService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await opportunityService.MarkAsWonAsync(currentUser.TenantId, opportunityId, cancellationToken);

    return ToOpportunityWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmOpportunitiesManage)
.WithName(ApiEndpointNames.OpportunitiesMarkWon)
.WithOpenApi();

app.MapPost(ApiRoutes.OpportunityLost, async (
    Guid opportunityId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IOpportunityService opportunityService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await opportunityService.MarkAsLostAsync(currentUser.TenantId, opportunityId, cancellationToken);

    return ToOpportunityWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmOpportunitiesManage)
.WithName(ApiEndpointNames.OpportunitiesMarkLost)
.WithOpenApi();

app.MapPost(ApiRoutes.OpportunityCanceled, async (
    Guid opportunityId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IOpportunityService opportunityService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await opportunityService.CancelAsync(currentUser.TenantId, opportunityId, cancellationToken);

    return ToOpportunityWriteResult(result);
})
.RequirePermission(KnownPermissions.CrmOpportunitiesManage)
.WithName(ApiEndpointNames.OpportunitiesCancel)
.WithOpenApi();

app.MapGet(ApiRoutes.FinancialAccounts, async (
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialAccountService accountService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var accounts = await accountService.ListAsync(currentUser.TenantId, cancellationToken);

    return Results.Ok(accounts);
})
.RequirePermission(KnownPermissions.FinanceAccountsView)
.WithName(ApiEndpointNames.FinancialAccountsList)
.WithOpenApi();

app.MapGet(ApiRoutes.FinancialAccountById, async (
    Guid accountId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialAccountService accountService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var account = await accountService.GetByIdAsync(currentUser.TenantId, accountId, cancellationToken);

    return account is null ? Results.NotFound() : Results.Ok(account);
})
.RequirePermission(KnownPermissions.FinanceAccountsView)
.WithName(ApiEndpointNames.FinancialAccountsGetById)
.WithOpenApi();

app.MapPost(ApiRoutes.FinancialAccounts, async (
    CreateFinancialAccountRequest accountRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialAccountService accountService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await accountService.CreateAsync(currentUser.TenantId, accountRequest, cancellationToken);

    return ToFinancialAccountWriteResult(result);
})
.RequirePermission(KnownPermissions.FinanceAccountsManage)
.WithName(ApiEndpointNames.FinancialAccountsCreate)
.WithOpenApi();

app.MapPut(ApiRoutes.FinancialAccountById, async (
    Guid accountId,
    UpdateFinancialAccountRequest accountRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialAccountService accountService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await accountService.UpdateAsync(currentUser.TenantId, accountId, accountRequest, cancellationToken);

    return ToFinancialAccountWriteResult(result);
})
.RequirePermission(KnownPermissions.FinanceAccountsManage)
.WithName(ApiEndpointNames.FinancialAccountsUpdate)
.WithOpenApi();

app.MapDelete(ApiRoutes.FinancialAccountById, async (
    Guid accountId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialAccountService accountService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await accountService.DeactivateAsync(currentUser.TenantId, accountId, cancellationToken);

    return result.Status == FinancialAccountOperationStatus.Success
        ? Results.NoContent()
        : ToFinancialAccountWriteResult(result);
})
.RequirePermission(KnownPermissions.FinanceAccountsManage)
.WithName(ApiEndpointNames.FinancialAccountsDeactivate)
.WithOpenApi();

app.MapGet(ApiRoutes.FinancialAccountTransactions, async (
    Guid accountId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialTransactionService transactionService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var transactions = await transactionService.ListByAccountAsync(currentUser.TenantId, accountId, cancellationToken);

    return Results.Ok(transactions);
})
.RequirePermission(KnownPermissions.FinanceTransactionsView)
.WithName(ApiEndpointNames.FinancialTransactionsListByAccount)
.WithOpenApi();

app.MapGet(ApiRoutes.FinancialTransactionById, async (
    Guid transactionId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialTransactionService transactionService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var transaction = await transactionService.GetByIdAsync(currentUser.TenantId, transactionId, cancellationToken);

    return transaction is null ? Results.NotFound() : Results.Ok(transaction);
})
.RequirePermission(KnownPermissions.FinanceTransactionsView)
.WithName(ApiEndpointNames.FinancialTransactionsGetById)
.WithOpenApi();

app.MapPost(ApiRoutes.FinancialAccountTransactions, async (
    Guid accountId,
    CreateFinancialTransactionRequest transactionRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialTransactionService transactionService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await transactionService.CreateAsync(currentUser.TenantId, accountId, transactionRequest, cancellationToken);

    return ToFinancialTransactionWriteResult(result);
})
.RequirePermission(KnownPermissions.FinanceTransactionsManage)
.WithName(ApiEndpointNames.FinancialTransactionsCreate)
.WithOpenApi();

app.MapPut(ApiRoutes.FinancialTransactionById, async (
    Guid transactionId,
    UpdateFinancialTransactionRequest transactionRequest,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialTransactionService transactionService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await transactionService.UpdateAsync(currentUser.TenantId, transactionId, transactionRequest, cancellationToken);

    return ToFinancialTransactionWriteResult(result);
})
.RequirePermission(KnownPermissions.FinanceTransactionsManage)
.WithName(ApiEndpointNames.FinancialTransactionsUpdate)
.WithOpenApi();

app.MapPost(ApiRoutes.FinancialTransactionVoid, async (
    Guid transactionId,
    HttpRequest request,
    IAuthenticationService authenticationService,
    IFinancialTransactionService transactionService,
    CancellationToken cancellationToken) =>
{
    var currentUser = await ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
    if (currentUser is null)
    {
        return Results.Unauthorized();
    }

    var result = await transactionService.VoidAsync(currentUser.TenantId, transactionId, cancellationToken);

    return ToFinancialTransactionWriteResult(result);
})
.RequirePermission(KnownPermissions.FinanceTransactionsManage)
.WithName(ApiEndpointNames.FinancialTransactionsVoid)
.WithOpenApi();

app.MapGet(ApiRoutes.SystemPermissions, () => KnownPermissions.All)
.RequirePermission(KnownPermissions.CoreSystemView)
.WithName(ApiEndpointNames.SystemPermissions)
.WithOpenApi();

app.Run();

static async Task<CurrentUserResponse?> ResolveCurrentUserAsync(
    HttpRequest request,
    IAuthenticationService authenticationService,
    CancellationToken cancellationToken)
{
    var accessToken = BearerTokenReader.Read(request);

    return accessToken is null
        ? null
        : await authenticationService.GetCurrentUserAsync(accessToken, cancellationToken);
}

static IResult ToCustomerWriteResult(CustomerOperationResult result)
{
    return result.Status switch
    {
        CustomerOperationStatus.Success => Results.Ok(result.Customer),
        CustomerOperationStatus.NotFound => Results.NotFound(),
        CustomerOperationStatus.DuplicateDocument => Results.Conflict(),
        CustomerOperationStatus.InvalidInput => Results.BadRequest(),
        _ => Results.BadRequest()
    };
}

static IResult ToContactWriteResult(ContactOperationResult result)
{
    return result.Status switch
    {
        ContactOperationStatus.Success => Results.Ok(result.Contact),
        ContactOperationStatus.NotFound => Results.NotFound(),
        ContactOperationStatus.CustomerNotFound => Results.NotFound(),
        ContactOperationStatus.DuplicateEmail => Results.Conflict(),
        ContactOperationStatus.InvalidInput => Results.BadRequest(),
        _ => Results.BadRequest()
    };
}

static IResult ToOpportunityWriteResult(OpportunityOperationResult result)
{
    return result.Status switch
    {
        OpportunityOperationStatus.Success => Results.Ok(result.Opportunity),
        OpportunityOperationStatus.NotFound => Results.NotFound(),
        OpportunityOperationStatus.CustomerNotFound => Results.NotFound(),
        OpportunityOperationStatus.InvalidInput => Results.BadRequest(),
        _ => Results.BadRequest()
    };
}

static IResult ToFinancialAccountWriteResult(FinancialAccountOperationResult result)
{
    return result.Status switch
    {
        FinancialAccountOperationStatus.Success => Results.Ok(result.Account),
        FinancialAccountOperationStatus.NotFound => Results.NotFound(),
        FinancialAccountOperationStatus.DuplicateName => Results.Conflict(),
        FinancialAccountOperationStatus.InvalidInput => Results.BadRequest(),
        _ => Results.BadRequest()
    };
}

static IResult ToFinancialTransactionWriteResult(FinancialTransactionOperationResult result)
{
    return result.Status switch
    {
        FinancialTransactionOperationStatus.Success => Results.Ok(result.Transaction),
        FinancialTransactionOperationStatus.NotFound => Results.NotFound(),
        FinancialTransactionOperationStatus.AccountNotFound => Results.NotFound(),
        FinancialTransactionOperationStatus.InvalidInput => Results.BadRequest(),
        _ => Results.BadRequest()
    };
}

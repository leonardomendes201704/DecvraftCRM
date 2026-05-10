using Platform.Api.Routing;
using Platform.Api.Security;
using Platform.Application.Abstractions;
using Platform.Application.Finance;
using Platform.Domain.Catalog;
using Platform.Domain.Enums;

namespace Platform.Api.Endpoints;

public sealed class FinanceEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.FinancialAccounts, async (
            HttpRequest request,
            IAuthenticationService authenticationService,
            IFinancialAccountService accountService,
            CancellationToken cancellationToken) =>
        {
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
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
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
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
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await accountService.CreateAsync(currentUser.TenantId, accountRequest, cancellationToken);

            return EndpointResultMapper.ToFinancialAccountWriteResult(result);
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
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await accountService.UpdateAsync(currentUser.TenantId, accountId, accountRequest, cancellationToken);

            return EndpointResultMapper.ToFinancialAccountWriteResult(result);
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
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await accountService.DeactivateAsync(currentUser.TenantId, accountId, cancellationToken);

            return result.Status == FinancialAccountOperationStatus.Success
                ? Results.NoContent()
                : EndpointResultMapper.ToFinancialAccountWriteResult(result);
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
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
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
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
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
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await transactionService.CreateAsync(currentUser.TenantId, accountId, transactionRequest, cancellationToken);

            return EndpointResultMapper.ToFinancialTransactionWriteResult(result);
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
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await transactionService.UpdateAsync(currentUser.TenantId, transactionId, transactionRequest, cancellationToken);

            return EndpointResultMapper.ToFinancialTransactionWriteResult(result);
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
            var currentUser = await EndpointUserResolver.ResolveCurrentUserAsync(request, authenticationService, cancellationToken);
            if (currentUser is null)
            {
                return Results.Unauthorized();
            }

            var result = await transactionService.VoidAsync(currentUser.TenantId, transactionId, cancellationToken);

            return EndpointResultMapper.ToFinancialTransactionWriteResult(result);
        })
        .RequirePermission(KnownPermissions.FinanceTransactionsManage)
        .WithName(ApiEndpointNames.FinancialTransactionsVoid)
        .WithOpenApi();
    }
}

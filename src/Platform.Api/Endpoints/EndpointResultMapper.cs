using Platform.Application.Contacts;
using Platform.Application.Customers;
using Platform.Application.Finance;
using Platform.Application.Opportunities;
using Platform.Domain.Enums;

namespace Platform.Api.Endpoints;

internal static class EndpointResultMapper
{
    internal static IResult ToCustomerWriteResult(CustomerOperationResult result)
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

    internal static IResult ToContactWriteResult(ContactOperationResult result)
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

    internal static IResult ToOpportunityWriteResult(OpportunityOperationResult result)
    {
        return result.Status switch
        {
            OpportunityOperationStatus.Success => Results.Ok(result.Opportunity),
            OpportunityOperationStatus.NotFound => Results.NotFound(),
            OpportunityOperationStatus.CustomerNotFound => Results.NotFound(),
            OpportunityOperationStatus.StageNotFound => Results.NotFound(),
            OpportunityOperationStatus.InvalidInput => Results.BadRequest(),
            _ => Results.BadRequest()
        };
    }

    internal static IResult ToOpportunityStageWriteResult(OpportunityStageOperationResult result)
    {
        return result.Status switch
        {
            OpportunityStageOperationStatus.Success => Results.Ok(result.Stage),
            OpportunityStageOperationStatus.NotFound => Results.NotFound(),
            OpportunityStageOperationStatus.DuplicateName => Results.Conflict(),
            OpportunityStageOperationStatus.InvalidInput => Results.BadRequest(),
            _ => Results.BadRequest()
        };
    }

    internal static IResult ToOpportunityActivityWriteResult(OpportunityActivityOperationResult result)
    {
        return result.Status switch
        {
            OpportunityActivityOperationStatus.Success => Results.Ok(result.Activity),
            OpportunityActivityOperationStatus.NotFound => Results.NotFound(),
            OpportunityActivityOperationStatus.OpportunityNotFound => Results.NotFound(),
            OpportunityActivityOperationStatus.InvalidInput => Results.BadRequest(),
            _ => Results.BadRequest()
        };
    }

    internal static IResult ToFinancialAccountWriteResult(FinancialAccountOperationResult result)
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

    internal static IResult ToFinancialTransactionWriteResult(FinancialTransactionOperationResult result)
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
}

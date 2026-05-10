using Platform.Application.Common;
using Platform.Application.Contacts;
using Platform.Application.Customers;
using Platform.Application.Finance;
using Platform.Application.Opportunities;
using Platform.Domain.Enums;

namespace Platform.UnitTests.Application;

public sealed class ApplicationResultTests
{
    [Fact]
    public void OperationResultsShouldExposeStandardSuccessFlag()
    {
        AssertOperationResult(CustomerOperationResult.InvalidInput(), CustomerOperationStatus.InvalidInput, false);
        AssertOperationResult(ContactOperationResult.DuplicateEmail(), ContactOperationStatus.DuplicateEmail, false);
        AssertOperationResult(OpportunityOperationResult.NotFound(), OpportunityOperationStatus.NotFound, false);
        AssertOperationResult(FinancialAccountOperationResult.DuplicateName(), FinancialAccountOperationStatus.DuplicateName, false);
        AssertOperationResult(FinancialTransactionOperationResult.AccountNotFound(), FinancialTransactionOperationStatus.AccountNotFound, false);
    }

    private static void AssertOperationResult<TStatus>(
        IApplicationOperationResult<TStatus> result,
        TStatus expectedStatus,
        bool expectedSucceeded)
        where TStatus : struct, Enum
    {
        Assert.Equal(expectedStatus, result.Status);
        Assert.Equal(expectedSucceeded, result.Succeeded);
    }
}

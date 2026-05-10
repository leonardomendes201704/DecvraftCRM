namespace Platform.Api.Routing;

public static class ApiRoutes
{
    public const string AuthLogin = "/api/auth/login";
    public const string Me = "/api/me";
    public const string Modules = "/api/modules";
    public const string SystemPermissions = "/api/system/permissions";
    public const string Departments = "/api/departments";
    public const string DepartmentById = "/api/departments/{departmentId:guid}";
    public const string JobTitles = "/api/job-titles";
    public const string JobTitleById = "/api/job-titles/{jobTitleId:guid}";
    public const string Employees = "/api/employees";
    public const string EmployeeById = "/api/employees/{employeeId:guid}";
    public const string EmployeeLinkUser = "/api/employees/{employeeId:guid}/link-user";
    public const string EmployeeUnlinkUser = "/api/employees/{employeeId:guid}/unlink-user";
    public const string EmployeeSubordinates = "/api/employees/{employeeId:guid}/subordinates";
    public const string Customers = "/api/customers";
    public const string CustomerById = "/api/customers/{customerId:guid}";
    public const string CustomerContacts = "/api/customers/{customerId:guid}/contacts";
    public const string ContactById = "/api/contacts/{contactId:guid}";
    public const string CustomerOpportunities = "/api/customers/{customerId:guid}/opportunities";
    public const string OpportunityStages = "/api/opportunity-stages";
    public const string OpportunityStageById = "/api/opportunity-stages/{stageId:guid}";
    public const string OpportunityStageOpportunities = "/api/opportunity-stages/{stageId:guid}/opportunities";
    public const string OpportunityById = "/api/opportunities/{opportunityId:guid}";
    public const string OpportunityStageMove = "/api/opportunities/{opportunityId:guid}/stage";
    public const string OpportunityOwner = "/api/opportunities/{opportunityId:guid}/owner";
    public const string OpportunityActivities = "/api/opportunities/{opportunityId:guid}/activities";
    public const string OpportunityActivitiesOverdue = "/api/opportunity-activities/overdue";
    public const string OpportunityActivitiesUpcoming = "/api/opportunity-activities/upcoming";
    public const string OpportunityActivityById = "/api/opportunity-activities/{activityId:guid}";
    public const string OpportunityActivityOwner = "/api/opportunity-activities/{activityId:guid}/owner";
    public const string OpportunityActivityComplete = "/api/opportunity-activities/{activityId:guid}/complete";
    public const string OpportunityActivityCancel = "/api/opportunity-activities/{activityId:guid}/cancel";
    public const string OpportunityHistory = "/api/opportunities/{opportunityId:guid}/history";
    public const string OpportunityWon = "/api/opportunities/{opportunityId:guid}/won";
    public const string OpportunityLost = "/api/opportunities/{opportunityId:guid}/lost";
    public const string OpportunityCanceled = "/api/opportunities/{opportunityId:guid}/canceled";
    public const string FinancialAccounts = "/api/financial/accounts";
    public const string FinancialAccountById = "/api/financial/accounts/{accountId:guid}";
    public const string FinancialAccountTransactions = "/api/financial/accounts/{accountId:guid}/transactions";
    public const string FinancialTransactionById = "/api/financial/transactions/{transactionId:guid}";
    public const string FinancialTransactionVoid = "/api/financial/transactions/{transactionId:guid}/void";
}

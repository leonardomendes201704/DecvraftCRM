namespace Platform.Domain.Enums;

public enum OrganizationOperationStatus
{
    Success = 1,
    NotFound = 2,
    DuplicateCode = 3,
    DuplicateEmail = 4,
    InvalidInput = 5,
    RelatedEntityNotFound = 6
}

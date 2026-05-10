namespace Platform.Application.Common;

public interface IApplicationResult
{
    bool Succeeded { get; }
}

public interface IApplicationOperationResult<TStatus> : IApplicationResult
    where TStatus : struct, Enum
{
    TStatus Status { get; }
}

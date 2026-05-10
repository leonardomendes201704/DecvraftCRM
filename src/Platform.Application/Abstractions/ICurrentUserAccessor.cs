using Platform.Application.Auth;

namespace Platform.Application.Abstractions;

public interface ICurrentUserAccessor
{
    CurrentUserResponse? CurrentUser { get; }

    void Set(CurrentUserResponse currentUser);
}

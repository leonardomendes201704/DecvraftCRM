using Platform.Application.Abstractions;
using Platform.Application.Auth;

namespace Platform.Api.Security;

public sealed class CurrentUserAccessor : ICurrentUserAccessor
{
    public CurrentUserResponse? CurrentUser { get; private set; }

    public void Set(CurrentUserResponse currentUser)
    {
        ArgumentNullException.ThrowIfNull(currentUser);

        CurrentUser = currentUser;
    }
}

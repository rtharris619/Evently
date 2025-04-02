using Evently.Common.Domain;

namespace Evently.Modules.Users.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId)
    {
        return Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found");
    }

    public static Error NotFound(string identityId)
    {
        return Error.NotFound("Users.NotFound", $"The user with the IDP identifier {identityId} not found");
    }
}

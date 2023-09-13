using System.Security.Claims;

namespace TaskManagementAPI.Services;

public class AuthUserIdExtractor
{
    public int GetUserId(ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst("id");

        if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
        {
            return userId;
        }
        throw new InvalidOperationException("User ID not found in claims.");
    }
}
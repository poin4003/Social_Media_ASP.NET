using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api.Extenstions;

public static class ClaimsExtensions
{
    public static string GetEmail(this ClaimsPrincipal user)
    {
        return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))?.Value;
    }

    public static string GetUserName(this ClaimsPrincipal user)
    {
        return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"))?.Value;
    }

    public static string GetId(this ClaimsPrincipal user)
    {
        return user.Claims.SingleOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
    }
}
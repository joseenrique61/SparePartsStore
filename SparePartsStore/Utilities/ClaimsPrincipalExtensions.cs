using System.Security.Claims;

namespace SparePartsStoreWeb.Utilities
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasRole(this ClaimsPrincipal user, string roleName)
        {
            if (user == null || !user.Identity!.IsAuthenticated) return false;

            // Buscamos en TODOS los claims si alguno tiene el valor del rol, 
            // sin importar si el Type es el largo de Microsoft o el corto de Keycloak.
            return user.Claims.Any(c => 
                (c.Type == ClaimTypes.Role || c.Type == "role" || c.Type.EndsWith("/role")) 
                && c.Value.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }


        public static string Email(this ClaimsPrincipal user)
        {
            if (user == null || !user.Identity!.IsAuthenticated) return "";

            // Buscamos en TODOS los claims si alguno tiene el valor del rol, 
            // sin importar si el Type es el largo de Microsoft o el corto de Keycloak.
            return user.Claims.First(c => 
                c.Type == ClaimTypes.Email || c.Type == "email" || c.Type.EndsWith("/email")).Value;
        }
    }
}
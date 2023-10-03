using Microsoft.AspNetCore.Identity;

namespace BooStoreApp.Utils
{
    public static class IdentityHelper
    {
        public static string GetIdentityErrors(IdentityResult identityResult)
        {
            var errorMessages = new List<string>();

            foreach (var error in identityResult.Errors)
            {
                errorMessages.Add(error.Description);
            }

            return string.Join(", ", errorMessages);
        }
    }
}

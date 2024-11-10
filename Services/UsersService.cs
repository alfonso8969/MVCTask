using MVCTask.Interfaces;
using System.Security.Claims;

namespace MVCTask.Services {
    public class UsersService: IUsersService {

        private readonly HttpContext httpContext;


        public UsersService(IHttpContextAccessor httpContextAccessor) {
            httpContext = httpContextAccessor.HttpContext;
        }


        public string GetUserId() {
            if (httpContext.User.Identity.IsAuthenticated) {
                var idClaim = httpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (idClaim != null) {
                    return idClaim.Value;
                } else {
                    throw new InvalidOperationException("The user does not have an identifier");
                }
            } else {
                throw new InvalidOperationException("The user is not authenticated");
            }
        }
    }
}

using System.Security.Claims;

namespace BudgetManager.Services
{
    public class UserService : IUserService
    {
        private readonly HttpContext? httpContext;
        public UserService(IHttpContextAccessor contextAccessor)
        {
            httpContext = contextAccessor.HttpContext;
        }
        public int GetUserId()
        {
            
            if(httpContext is not null && 
                httpContext.User is not null &&
                httpContext.User.Identity is not null &&
                httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims
                    .Where(x => x.Type.Equals(ClaimTypes.NameIdentifier))
                    .FirstOrDefault();

                var id = int.Parse(idClaim is not null ? idClaim.Value: "0");

                return id;
            }
            else
            {
                throw new ApplicationException("El usuario no esta autenticado");
            }
        }
    }
}

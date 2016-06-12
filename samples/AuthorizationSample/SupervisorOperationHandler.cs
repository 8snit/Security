using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AuthorizationSample
{
    public class SupervisorOperationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement)
        {
            if (!context.User.IsInRole("supervisor"))
            {
                return;
            }

            context.Succeed(requirement);
        }
    }
}
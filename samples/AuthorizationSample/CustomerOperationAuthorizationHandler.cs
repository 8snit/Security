using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AuthorizationSample
{
    public class CustomerOperationAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Customer>
    {
        private readonly IPermissionService _permissions;

        public CustomerOperationAuthorizationHandler(IPermissionService permissions)
        {
            _permissions = permissions;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Customer resource)
        {
            if (!context.User.HasClaim("department", "sales"))
            {
                return;
            }

            if (!context.User.HasClaim("region", resource.Region))
            {
                return;
            }

            if (resource.Fortune500)
            {
                if (!context.User.HasClaim("status", "senior"))
                {
                    return;
                }
            }

            var discountOperationAuthorizationRequirement = requirement as DiscountOperationAuthorizationRequirement;
            if (discountOperationAuthorizationRequirement != null)
            {
                var salesRep = context.User.FindFirst("id").Value;
                if (!await _permissions.IsDiscountAllowedAsync(
                    salesRep,
                    resource.Id,
                    discountOperationAuthorizationRequirement.Amount))
                {
                    return;
                }
            }

            context.Succeed(requirement);
        }
    }
}
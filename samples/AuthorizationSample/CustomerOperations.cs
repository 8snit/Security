using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AuthorizationSample
{
    public static class CustomerOperations
    {
        public static OperationAuthorizationRequirement Manage = new OperationAuthorizationRequirement {Name = "Manage"};

        public static OperationAuthorizationRequirement GiveDiscount(int amount)
        {
            return new DiscountOperationAuthorizationRequirement
            {
                Name = "GiveDiscount" + amount,
                Amount = amount
            };
        }
    }

    public class DiscountOperationAuthorizationRequirement : OperationAuthorizationRequirement
    {
        public int Amount { get; set; }
    }
}
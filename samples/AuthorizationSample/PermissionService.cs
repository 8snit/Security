using System.Threading.Tasks;

namespace AuthorizationSample
{
    public interface IPermissionService
    {
        Task<bool> IsDiscountAllowedAsync(string id, int customerId, int amount);
    }

    public class PermissionService : IPermissionService
    {
        public async Task<bool> IsDiscountAllowedAsync(string id, int customerId, int amount)
        {
            await Task.Delay(100);
            return amount < 10;
        }
    }
}
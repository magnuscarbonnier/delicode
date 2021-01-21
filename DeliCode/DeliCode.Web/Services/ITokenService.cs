using DeliCode.Web.Models;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public interface ITokenService
    {
        ApiToken GetOrderApiToken();
        ApiToken GetProductApiToken();
    }
}
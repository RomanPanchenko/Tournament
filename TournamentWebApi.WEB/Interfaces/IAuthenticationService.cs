using System.Threading.Tasks;
using System.Web;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.WEB.Models;

namespace TournamentWebApi.WEB.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AccountModel> Login(HttpResponse httpResponse, LoginModel loginModel);
        void Logout(HttpResponse httpResponse);
    }
}
using GymWEB.Models;

namespace GymWEB.Services
{
    public class UsuarioService
    {
        private readonly ApiClient _apiClient = new ApiClient();

        public LoginResponse Login(LoginRequest login)
        {
            return _apiClient
                .PostAsync<LoginRequest, LoginResponse>("Auth/login", login)
                .GetAwaiter()
                .GetResult();
        }
    }
}
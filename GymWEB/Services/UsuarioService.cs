using GymWEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymWEB.Services
{
    public class UsuarioService
    {
        private readonly ApiClient _apiClient =
            new ApiClient();

        // =========================================================
        // LOGIN
        // =========================================================

        public LoginResponse Login(LoginRequest login)
        {
            return _apiClient
                .PostAsync<LoginRequest, LoginResponse>(
                    "Auth/login",
                    login)
                .GetAwaiter()
                .GetResult();
        }

        // =========================================================
        // LISTAR
        // =========================================================

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _apiClient
                .GetAsync<List<Usuario>>(
                    "Usuario");
        }

        // =========================================================
        // BUSCAR POR ID
        // =========================================================

        public async Task<Usuario> ObtenerPorIdAsync(int id)
        {
            return await _apiClient
                .GetAsync<Usuario>(
                    $"Usuario/{id}");
        }

        // =========================================================
        // CREAR
        // =========================================================

        public async Task<bool> AgregarAsync(
            Usuario usuario)
        {
            return await _apiClient
                .PostSimpleAsync(
                    "Usuario",
                    usuario);
        }

        // =========================================================
        // ACTUALIZAR
        // =========================================================

        public async Task<bool> ActualizarAsync(
            Usuario usuario)
        {
            return await _apiClient
                .PutAsync(
                    $"Usuario/{usuario.Id}",
                    usuario);
        }

        // =========================================================
        // ELIMINAR
        // =========================================================

        public async Task<bool> EliminarAsync(int id)
        {
            return await _apiClient
                .DeleteAsync(
                    $"Usuario/{id}");
        }
    }
}
using GymWEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymWEB.Services
{
    public class ClienteService
    {
        private readonly ApiClient _apiClient =
            new ApiClient();

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _apiClient.GetAsync<List<Cliente>>(
                "Cliente");
        }

        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            return await _apiClient.GetAsync<Cliente>(
                $"Cliente/{id}");
        }

        public async Task<bool> AgregarAsync(Cliente cliente)
        {
            return await _apiClient.PostSimpleAsync(
                "Cliente",
                cliente);
        }

        public async Task<bool> ActualizarAsync(Cliente cliente)
        {
            return await _apiClient.PutAsync(
                "Cliente",
                cliente);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _apiClient.DeleteAsync(
                $"Cliente/{id}");
        }
    }
}
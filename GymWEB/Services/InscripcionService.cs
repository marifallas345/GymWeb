using GymWEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymWEB.Services
{
    public class InscripcionService
    {
        private readonly ApiClient _apiClient =
            new ApiClient();

        // =========================================================
        // OBTENER TODAS
        // =========================================================

        public async Task<List<Inscripcion>>
            ObtenerTodosAsync()
        {
            return await _apiClient
                .GetAsync<List<Inscripcion>>(
                    "Inscripcion");
        }

        // =========================================================
        // OBTENER POR ID
        // =========================================================

        public async Task<Inscripcion>
            ObtenerPorIdAsync(int id)
        {
            return await _apiClient
                .GetAsync<Inscripcion>(
                    $"Inscripcion/{id}");
        }

        // =========================================================
        // CREAR
        // =========================================================

        public async Task<bool> AgregarAsync(
            Inscripcion inscripcion)
        {
            return await _apiClient
                .PostSimpleAsync(
                    "Inscripcion",
                    inscripcion);
        }

        // =========================================================
        // ACTUALIZAR
        // =========================================================

        public async Task<bool> ActualizarAsync(
            Inscripcion inscripcion)
        {
            return await _apiClient
                .PutAsync(
                    "Inscripcion",
                    inscripcion);
        }

        // =========================================================
        // ELIMINAR
        // =========================================================

        public async Task<bool> EliminarAsync(
            int id)
        {
            return await _apiClient
                .DeleteAsync(
                    $"Inscripcion/{id}");
        }
    }
}
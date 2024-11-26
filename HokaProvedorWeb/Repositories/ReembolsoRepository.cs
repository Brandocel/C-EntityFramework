using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HokaProvedorWeb.Repositories
{
    public class ReembolsoRepository : IReembolsoRepository
    {
        private readonly string _connectionString;

        public ReembolsoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<SelectListItem> ObtenerOpciones(string query, string columnName)
        {
            var opciones = new List<SelectListItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        opciones.Add(new SelectListItem
                        {
                            Value = reader[columnName]?.ToString() ?? string.Empty,
                            Text = reader[columnName]?.ToString() ?? "Sin valor"
                        });
                    }
                }
            }
            return opciones;
        }

        public bool GuardarUUID(string uuid)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "INSERT INTO entradaM (uuid) VALUES (@UUID)";
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UUID", uuid);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GuardarReembolso(ReembolsoViewModel model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "INSERT INTO Reembolsos (NombreRazonSocial, Folio, Importe, Iva, Total) VALUES (@NombreRazonSocial, @Folio, @Importe, @Iva, @Total)";
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreRazonSocial", model.NombreRazonSocial);
                    command.Parameters.AddWithValue("@Folio", model.Folio);
                    command.Parameters.AddWithValue("@Importe", model.Importe);
                    command.Parameters.AddWithValue("@Iva", model.Iva);
                    command.Parameters.AddWithValue("@Total", model.Total);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

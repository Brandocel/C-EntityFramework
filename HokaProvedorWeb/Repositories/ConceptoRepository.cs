using HokaProvedorWeb.Data;
using HokaProvedorWeb.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Repositories
{
    public class ConceptoRepository : IConceptoRepository
    {
        private readonly string _connectionString;

        public ConceptoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> AgregarConceptoAsync(string conceptoPago)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "INSERT INTO provedores (Concepto) VALUES (@Concepto)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Concepto", conceptoPago);
                        await command.ExecuteNonQueryAsync();
                    }
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

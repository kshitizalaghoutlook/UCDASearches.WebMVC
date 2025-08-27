using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UCDASearches.WebMVC.Models;

namespace UCDASearches.WebMVC.Controllers
{
    [Authorize]
    public class PreviousSearchesController : Controller
    {
        private readonly IConfiguration _configuration;
        public PreviousSearchesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PreviousSearchesViewModel model)
        {
            // Only query when a search is performed
            if (Request.Query.Count == 0)
                return View(model);

            var results = new List<PreviousSearch>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var sql = @"SELECT RequestID, UID, VIN, Time_Stamp, Account, Operator, AutoCheck, Lien, History, OOPS
                     FROM Requests WHERE 1 = 1";
            var command = new SqlCommand();
            command.Connection = connection;

            if (int.TryParse(model.RequestId, out var requestId))
            {
                sql += " AND RequestID = @RequestID";
                command.Parameters.Add("@RequestID", System.Data.SqlDbType.Int).Value = requestId;
            }
            if (!string.IsNullOrWhiteSpace(model.Vin))
            {
                sql += " AND VIN = @Vin";
                command.Parameters.AddWithValue("@Vin", model.Vin);
            }
            if (model.FromDate.HasValue)
            {
                sql += " AND Time_Stamp >= @FromDate";
                command.Parameters.AddWithValue("@FromDate", model.FromDate.Value);
            }
            if (model.ToDate.HasValue)
            {
                sql += " AND Time_Stamp <= @ToDate";
                command.Parameters.AddWithValue("@ToDate", model.ToDate.Value);
            }

            command.CommandText = sql;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new PreviousSearch
                {
                    RequestID = reader.GetInt32(0),
                    UID = reader[1] == DBNull.Value ? string.Empty : Convert.ToString(reader[1])!,
                    Vin = reader[2] == DBNull.Value ? string.Empty : Convert.ToString(reader[2])!,
                    TimeStamp = reader.GetDateTime(3),
                    Account = reader[4] == DBNull.Value ? string.Empty : Convert.ToString(reader[4])!,
                    Operator = reader[5] == DBNull.Value ? string.Empty : Convert.ToString(reader[5])!,
                    AutoCheck = reader[6] == DBNull.Value ? string.Empty : Convert.ToString(reader[6])!,
                    Lien = reader[7] == DBNull.Value ? string.Empty : Convert.ToString(reader[7])!,
                    History = reader[8] == DBNull.Value ? string.Empty : Convert.ToString(reader[8])!,
                    OOPS = reader[9] == DBNull.Value ? string.Empty : Convert.ToString(reader[9])!
                });
            }

            model.Results = results;
            return View(model);
        }
    }
}

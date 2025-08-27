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

            if (!string.IsNullOrWhiteSpace(model.RequestId))
            {
                sql += " AND RequestID = @RequestID";
                command.Parameters.AddWithValue("@RequestID", model.RequestId);
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
                    UID = reader.GetString(1),
                    Vin = reader.GetString(2),
                    TimeStamp = reader.GetDateTime(3),
                    Account = reader.GetString(4),
                    Operator = reader.GetString(5),
                    AutoCheck = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Lien = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    History = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    OOPS = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
                });
            }

            model.Results = results;
            return View(model);
        }
    }
}

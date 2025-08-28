using System.Data;
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
            bool isMobile = Request.Headers["User-Agent"].ToString().ToLowerInvariant().Contains("mobi");

            var results = new List<PreviousSearch>();
            if (Request.Query.Count > 0)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var sql = @"SELECT RequestID, UID, VIN, Time_Stamp, Account, Operator, AutoCheck, Lien, History, OOPS, ExcaDate, EXCA, IRE, Carfax, CPIC, CPICdate, CAMVAP, LNOpath, LNOcompleted FROM Requests WHERE 1 = 1";
                using var command = new SqlCommand();
                command.Connection = connection;

                if (!string.IsNullOrWhiteSpace(model.RequestId) && int.TryParse(model.RequestId, out var requestId))
                {
                    sql += " AND RequestID = @RequestID";
                    command.Parameters.Add("@RequestID", SqlDbType.Int).Value = requestId;
                }

                if (!string.IsNullOrWhiteSpace(model.Vin))
                {
                    sql += " AND VIN = @Vin";
                    command.Parameters.Add("@Vin", SqlDbType.VarChar, 17).Value = model.Vin;
                }

                if (!string.IsNullOrWhiteSpace(model.Uid))
                {
                    sql += " AND UID = @Uid";
                    command.Parameters.AddWithValue("@Uid", model.Uid);
                }

                if (!string.IsNullOrWhiteSpace(model.Account))
                {
                    sql += " AND Account = @Account";
                    command.Parameters.AddWithValue("@Account", model.Account);
                }

                if (!string.IsNullOrWhiteSpace(model.Operator))
                {
                    sql += " AND Operator = @Operator";
                    command.Parameters.AddWithValue("@Operator", model.Operator);
                }

                if (model.FromDate.HasValue)
                {
                    sql += " AND Time_Stamp >= @FromDate";
                    command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = model.FromDate.Value;
                }

                if (model.ToDate.HasValue)
                {
                    sql += " AND Time_Stamp <= @ToDate";
                    command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = model.ToDate.Value;
                }

                command.CommandText = sql;
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    results.Add(new PreviousSearch
                    {
                        RequestID = reader.GetInt32(0),
                        UID = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Vin = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        TimeStamp = reader.GetDateTime(3),
                        Account = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        Operator = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        AutoCheck = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Lien = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        History = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        OOPS = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        ExcaDate = reader.IsDBNull(10) ? (DateTime?)null : reader.GetDateTime(10),
                        EXCA = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        IRE = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Carfax = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        CPIC = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        CPICdate = reader.IsDBNull(15) ? (DateTime?)null : reader.GetDateTime(15),
                        CAMVAP = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                        LNOpath = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                        LNOcompleted = reader.IsDBNull(18) ? (DateTime?)null : reader.GetDateTime(18)
                    });
                }
            }

            model.Results = results;
            var viewName = isMobile ? "Index.Mobile" : "Index";
            return View(viewName, model);
        }
    }
}

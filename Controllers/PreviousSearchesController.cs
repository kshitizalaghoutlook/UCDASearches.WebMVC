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
        [HttpGet("/previoussearches")]
        public async Task<IActionResult> Index(PreviousSearchesViewModel model)
        {
            bool isMobile = Request.Headers["User-Agent"].ToString().ToLowerInvariant().Contains("mobi");

            var results = new List<PreviousSearch>();
            if (Request.Query.Count > 0)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();


            var sql = @"SELECT RequestID, UID, VIN, Time_Stamp, Account, Operator, AutoCheck, Lien, History, OOPS, ExcaDate, EXCA, IRE, Carfax, CPIC, CPICdate, CAMVAP, LNOpath, LNOcompleted

            var sql = @"SELECT RequestID, UID, VIN, Time_Stamp, Account, Operator, AutoCheck, Lien, History, OOPS

                     FROM Requests WHERE 1 = 1";
            await using var command = new SqlCommand();
            command.Connection = connection;


            if (int.TryParse(model.RequestId, out var requestId))
            {
                sql += " AND RequestID = @RequestID";
                command.Parameters.Add("@RequestID", System.Data.SqlDbType.Int).Value = requestId;

            if (!string.IsNullOrWhiteSpace(model.RequestId))
            {
                sql += " AND RequestID = @RequestID";
                command.Parameters.Add("@RequestID", SqlDbType.Int).Value = int.Parse(model.RequestId);

            }
            if (!string.IsNullOrWhiteSpace(model.Vin))
            {
                sql += " AND VIN = @Vin";

                command.Parameters.Add("@Vin", System.Data.SqlDbType.VarChar, 17).Value = model.Vin;

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

                command.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = model.FromDate.Value;

                command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = model.FromDate.Value;

            }
            if (model.ToDate.HasValue)
            {
                sql += " AND Time_Stamp <= @ToDate";

                command.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = model.ToDate.Value;

                command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = model.ToDate.Value;

            }

            command.CommandText = sql;


            await using var reader = await command.ExecuteReaderAsync();

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
                    OOPS = reader[9] == DBNull.Value ? string.Empty : Convert.ToString(reader[9])!,
                    ExcaDate = reader.IsDBNull(10) ? (DateTime?)null : reader.GetDateTime(10),
                    EXCA = reader[11] == DBNull.Value ? string.Empty : Convert.ToString(reader[11])!,
                    IRE = reader[12] == DBNull.Value ? string.Empty : Convert.ToString(reader[12])!,
                    Carfax = reader[13] == DBNull.Value ? string.Empty : Convert.ToString(reader[13])!,
                    CPIC = reader[14] == DBNull.Value ? string.Empty : Convert.ToString(reader[14])!,
                    CPICdate = reader.IsDBNull(15) ? (DateTime?)null : reader.GetDateTime(15),
                    CAMVAP = reader[16] == DBNull.Value ? string.Empty : Convert.ToString(reader[16])!,
                    LNOpath = reader[17] == DBNull.Value ? string.Empty : Convert.ToString(reader[17])!,
                    LNOcompleted = reader.IsDBNull(18) ? (DateTime?)null : reader.GetDateTime(18)

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
            var viewName = isMobile ? "Index.Mobile" : "Index";
            return View(viewName, model);
        }
    }
}
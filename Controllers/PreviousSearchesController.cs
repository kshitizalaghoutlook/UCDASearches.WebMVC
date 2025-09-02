using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UCDASearches.WebMVC.Models;

namespace UCDASearches.WebMVC.Controllers;

[Authorize]
public class PreviousSearchesController : Controller
{
    private readonly IConfiguration _configuration;
    private const string DefaultAccount = "008035-0001";

    public PreviousSearchesController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("/previoussearches")]
    public async Task<IActionResult> Index(PreviousSearchesViewModel model)
    {
        bool isMobile = Request.Headers["User-Agent"].ToString().ToLowerInvariant().Contains("mobi");

        var results = new List<PreviousSearch>();
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        bool hasFilters = !string.IsNullOrWhiteSpace(model.RequestId)
                          || !string.IsNullOrWhiteSpace(model.Vin)
                          || model.Date.HasValue;

        var sb = new StringBuilder();
        sb.Append("SELECT ");
        sb.Append(hasFilters ? string.Empty : "TOP 100 ");
        sb.Append("RequestID, UID, VIN, Time_Stamp, Account, Operator, AutoCheck, Lien, History, OOPS, ");
        sb.Append("ExcaDate, EXCA, IRE, Carfax, CPIC, CPICdate, CAMVAP, LNOpath, LNOcompleted ");
        sb.Append("FROM Requests WHERE Account = @Account");

        await using var command = new SqlCommand();
        command.Connection = connection;
        command.Parameters.Add("@Account", SqlDbType.VarChar, 20).Value = DefaultAccount;

        if (!string.IsNullOrWhiteSpace(model.RequestId))
        {
            sb.Append(" AND RequestID = @RequestID");
            command.Parameters.Add("@RequestID", SqlDbType.Int).Value = int.Parse(model.RequestId);
        }

        if (!string.IsNullOrWhiteSpace(model.Vin))
        {
            sb.Append(" AND VIN = @Vin");
            command.Parameters.Add("@Vin", SqlDbType.VarChar, 17).Value = model.Vin;
        }

        if (model.Date.HasValue)
        {
            sb.Append(" AND CONVERT(date, Time_Stamp) = @Date");
            command.Parameters.Add("@Date", SqlDbType.Date).Value = model.Date.Value.Date;
        }

        sb.Append(" ORDER BY Time_Stamp DESC");
        command.CommandText = sb.ToString();

        await using var reader = await command.ExecuteReaderAsync();
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
                CPICTime = reader.IsDBNull(15) ? (DateTime?)null : reader.GetDateTime(15),
                CAMVAP = reader[16] == DBNull.Value ? string.Empty : Convert.ToString(reader[16])!,
                LNONpath = reader[17] == DBNull.Value ? string.Empty : Convert.ToString(reader[17])!,
                LNONcompleted = reader.IsDBNull(18) ? (DateTime?)null : reader.GetDateTime(18)
            });
        }

        model.Results = results;
        var viewName = isMobile ? "Index.Mobile" : "Index";
        return View(viewName, model);
    }
}

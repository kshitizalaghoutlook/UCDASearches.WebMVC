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

        bool hasFilters =  !string.IsNullOrWhiteSpace(model.Vin)
                          || model.Date.HasValue;

        var sb = new StringBuilder();
        sb.Append("SELECT ");
        sb.Append(hasFilters ? string.Empty : "TOP 100 ");
        sb.Append("RequestID, UID, VIN, Time_Stamp, Account, Operator, AutoCheck, Lien, History, OOPS, ");
        sb.Append("ExcaDate, EXCA, IRE, Carfax, CPIC, CPICtime, CAMVAP, LNONpath, LNONcompleted ");
        sb.Append("FROM Requests WHERE Account = @Account");

        await using var command = new SqlCommand();
        command.Connection = connection;
        command.Parameters.Add("@Account", SqlDbType.VarChar, 20).Value = DefaultAccount;

        //if (!string.IsNullOrWhiteSpace(model.RequestId))
        //{
        //    sb.Append(" AND RequestID = @RequestID");
        //    command.Parameters.Add("@RequestID", SqlDbType.Int).Value = int.Parse(model.RequestId);
        //}

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
                RequestID = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0),
                UID = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),

                Vin = reader.IsDBNull(2) ? null : reader.GetString(2),
                TimeStamp = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                Account = reader.IsDBNull(4) ? null : reader.GetString(4).Trim(),
                Operator = reader.IsDBNull(5) ? null : reader.GetString(5),

                AutoCheck = reader.IsDBNull(6) ? (short?)null : reader.GetInt16(6),
                Lien = reader.IsDBNull(7) ? (short?)null : reader.GetInt16(7),
                History = reader.IsDBNull(8) ? (short?)null : reader.GetInt16(8),
                OOPS = reader.IsDBNull(9) ? (short?)null : reader.GetInt16(9),

                ExcaDate = reader.IsDBNull(10) ? (DateTime?)null : reader.GetDateTime(10),
                EXCA = reader.IsDBNull(11) ? (short?)null : reader.GetInt16(11),
                IRE = reader.IsDBNull(12) ? (short?)null : reader.GetInt16(12),
                Carfax = reader.IsDBNull(13) ? (short?)null : reader.GetInt16(13),
                CPIC = reader.IsDBNull(14) ? (short?)null : reader.GetInt16(14),

                CPICTime = reader.IsDBNull(15) ? (DateTime?)null : reader.GetDateTime(15),
                CAMVAP = reader.IsDBNull(16) ? (short?)null : reader.GetInt16(16),
                LNONpath = reader.IsDBNull(17) ? (byte?)null : reader.GetByte(17),
                LNONcompleted = reader.IsDBNull(18) ? (DateTime?)null : reader.GetDateTime(18)
            });
        }

        model.Results = results;
        var viewName = isMobile ? "Index.Mobile" : "Index";
        return View(viewName, model);
    }
}
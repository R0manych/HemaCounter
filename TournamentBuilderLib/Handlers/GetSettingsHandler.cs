using ExcelLib;
using Google.Apis.Sheets.v4.Data;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IGetSettingsHandler
{
    public IEnumerable<Dictionary<string, string>> GetSettings();
}

public class GetSettingsHandler : IGetSettingsHandler
{
    private const string SHEET_NAME = "Settings";
    private string SHEET_ID = GoogleSheet.Default.CONFIG_SHEET_ID;

    public GetSettingsHandler()
    {

    }

    public IEnumerable<Dictionary<string, string>> GetSettings()
    {
        var range = $"{SHEET_NAME}";
        var rows = ExcelReader.Read(SHEET_ID, range);
        var settings = new List<Dictionary<string, string>>();
        foreach (var row in rows.Skip(1))
        {
            var item = new Dictionary<string, string>();
            for(int i = 0; i < row.Count; i++)
            {
                item.Add(rows[0][i].ToString(), row[i].ToString());
            }
            settings.Add(item);
        }
        return settings;
    }
}

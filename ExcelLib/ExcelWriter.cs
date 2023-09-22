using Google.Apis.Sheets.v4.Data;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace ExcelLib;

public static class ExcelWriter
{
    public static void Write(string sheetId, string sheetRange, IList<IList<object>> data)
    {
        var googleSheetsHelper = GoogleSheetsHelper.Instance;
        var googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
        var valueRange = new ValueRange
        {
            Values = data
        };
        var updateRequest = googleSheetValues.Update(valueRange, sheetId, sheetRange);
        updateRequest.ValueInputOption = UpdateRequest.ValueInputOptionEnum.USERENTERED;
        updateRequest.Execute();
    }
}

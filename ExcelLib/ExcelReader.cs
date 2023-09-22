namespace ExcelLib
{
    public static class ExcelReader
    {
        public static IList<IList<object>> Read(string sheetId, string sheetRange)
        {
            var googleSheetsHelper = GoogleSheetsHelper.Instance;
            var googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
            var request = googleSheetValues.Get(sheetId, sheetRange);
            var response = request.Execute();
            return response.Values;
        }
    }
}

using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public class WriteToExistingBattleResultHandler : BaseHandler, IWriteBattleResultHandler
    {
        private readonly string _sheetName = GoogleSheet.Default.RESULT_SHEET_NAME;

        public WriteToExistingBattleResultHandler(string sheetId) : base(sheetId)
        {
        }

        public void Execute(BattleResult battleResult)
        {
            var range = $"{_sheetName}!{battleResult.Range}";
            var existedData = ExcelReader.Read(_sheetId, range);
            var values = ExcelReader.Read(_sheetId, range);
            (int, int) existingValue = new (0, 0);
            if (values != null)
            {
                var value = values.First();
                existingValue = (Convert.ToInt32(value[0]), Convert.ToInt32(value[1]));
            }
            var objectList = new List<object>()
            {
                battleResult.Result + existingValue.Item1,
                battleResult.Score + existingValue.Item2,
            };
            var rangeData = new List<IList<object>> { objectList };
            ExcelWriter.Write(_sheetId, range, rangeData);
        }
    }
}

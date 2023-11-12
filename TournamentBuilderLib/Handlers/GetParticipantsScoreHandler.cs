using ExcelLib;
using System.Globalization;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public interface IGetParticipantsScoreHandler
    {
        IEnumerable<ParticipantScore> Execute();
    }

    public class GetParticipantsScoreHandler : BaseHandler, IGetParticipantsScoreHandler
    {
        private readonly string _sheetName = GoogleSheet.Default.SCORE_SHEET_NAME;

        public GetParticipantsScoreHandler(string sheetId) : base(sheetId)
        {
        }

        public IEnumerable<ParticipantScore> Execute()
        {
            var range = $"{_sheetName}";
            var values = ExcelReader.Read(_sheetId, range);
            var participantScores = new List<ParticipantScore>();
            foreach (var value in values)
            {
                ParticipantScore item = new()
                {
                    Name = value[0]?.ToString(),
                    WinScore = string.IsNullOrEmpty(value[1].ToString()) ? 0 : Convert.ToDecimal(value[1].ToString().Replace(",", "."), CultureInfo.InvariantCulture),
                    PointsScore = string.IsNullOrEmpty(value[2].ToString()) ? 0 : Convert.ToInt32(value[2]),
                };
                participantScores.Add(item);
            }
            return participantScores;
        }
    }
}
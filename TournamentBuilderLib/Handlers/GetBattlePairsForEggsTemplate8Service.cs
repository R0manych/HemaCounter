using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public class GetBattlePairsForEggsTemplate8Service : BaseHandler
    {
        public GetBattlePairsForEggsTemplate8Service(string sheetId) : base(sheetId)
        {
        }

        public IEnumerable<BattlePair> GetBattlePairsForFightRound1(string sheetName, int participantsCount)
        {
            var lastStr = participantsCount * 2 - 2 + 9;
            var startIndex = 9;
            var range = $"{sheetName}!F{startIndex}:G{lastStr}";
            var values = ExcelReader.Read(_sheetId, range);
            if (values == null)
                return new List<BattlePair>();

            var battlePairs = new List<BattlePair>();

            for (var i = 0; i < lastStr - startIndex; i += 4)
            {
                var firstFighterStr = values[i];
                var secondFighterStr = values[i + 1];
                {
                    BattlePair item = new()
                    {
                        FighterRedName = firstFighterStr[0]?.ToString(),
                        FighterRedScore = string.IsNullOrEmpty(firstFighterStr[1].ToString()) ? 0 : Convert.ToInt32(firstFighterStr[1]),
                        FighterBlueScore = string.IsNullOrEmpty(secondFighterStr[1].ToString()) ? 0 : Convert.ToInt32(secondFighterStr[1]),
                        FighterBlueName = secondFighterStr[0]?.ToString(),
                        FighterRedRange = $"{sheetName}!G{startIndex + i}:G{startIndex + i}",
                        FighterBlueRange = $"{sheetName}!G{startIndex + i + 1}:G{startIndex + i + 1}",
                    };
                    battlePairs.Add(item);
                }
            }
            return battlePairs;
        }

        public IEnumerable<BattlePair> GetBattlePairsForFightRound2(string sheetName, int participantsCount)
        {
            var lastStr = participantsCount * 2 - 2 + 9;
            var startIndex = 9;
            var range = $"{sheetName}!I{startIndex}:J{lastStr}";
            var values = ExcelReader.Read(_sheetId, range);
            if (values == null)
                return new List<BattlePair>();

            var battlePairs = new List<BattlePair>();

            for (var i = 0; i < lastStr - startIndex; i+=8)
            {
                var firstFighterStr = values[i];
                var secondFighterStr = values[i + 4];
                {
                    BattlePair item = new()
                    {
                        FighterRedName = firstFighterStr[0]?.ToString(),
                        FighterRedScore = string.IsNullOrEmpty(firstFighterStr[1].ToString()) ? 0 : Convert.ToInt32(firstFighterStr[1]),
                        FighterBlueScore = string.IsNullOrEmpty(secondFighterStr[1].ToString()) ? 0 : Convert.ToInt32(secondFighterStr[1]),
                        FighterBlueName = secondFighterStr[0]?.ToString(),
                        FighterRedRange = $"{sheetName}!J{startIndex + i}:J{startIndex + i}",
                        FighterBlueRange = $"{sheetName}!J{startIndex + i + 4}:J{startIndex + i + 4}",
                    };
                    battlePairs.Add(item);
                }
            }
            return battlePairs;
        }

        public IEnumerable<BattlePair> GetBattlePairsForFightRound3(string sheetName, int participantsCount)
        {
            var startIndex = 11;
            var lastStr = participantsCount * 2 + startIndex;
            var range = $"{sheetName}!L{startIndex}:M{lastStr}";
            var values = ExcelReader.Read(_sheetId, range);
            if (values == null)
                return new List<BattlePair>();

            var battlePairs = new List<BattlePair>();

            for (var i = 0; i < lastStr - startIndex; i += 16)
            {
                var firstFighterStr = values[i];
                var secondFighterStr = values[i + 8];
                {
                    BattlePair item = new()
                    {
                        FighterRedName = firstFighterStr[0]?.ToString(),
                        FighterRedScore = string.IsNullOrEmpty(firstFighterStr[1].ToString()) ? 0 : Convert.ToInt32(firstFighterStr[1]),
                        FighterBlueScore = string.IsNullOrEmpty(secondFighterStr[1].ToString()) ? 0 : Convert.ToInt32(secondFighterStr[1]),
                        FighterBlueName = secondFighterStr[0]?.ToString(),
                        FighterRedRange = $"{sheetName}!M{startIndex + i}:M{startIndex + i}",
                        FighterBlueRange = $"{sheetName}!M{startIndex + i + 8}:M{startIndex + i + 8}",
                    };
                    battlePairs.Add(item);
                }
            }
            return battlePairs;
        }

        public IEnumerable<BattlePair> GetBattlePairsForFightRound4(string sheetName, int participantsCount)
        {
            var startIndex = 15;
            var lastStr = participantsCount * 2 - 2 + startIndex;
            var range = $"{sheetName}!O{startIndex}:P{lastStr}";
            var values = ExcelReader.Read(_sheetId, range);
            if (values == null)
                return new List<BattlePair>();

            var battlePairs = new List<BattlePair>();

            for (var i = 0; i < lastStr - startIndex; i += 32)
            {
                var firstFighterStr = values[i];
                var secondFighterStr = values[i + 16];
                {
                    BattlePair item = new()
                    {
                        FighterRedName = firstFighterStr[0]?.ToString(),
                        FighterRedScore = string.IsNullOrEmpty(firstFighterStr[1].ToString()) ? 0 : Convert.ToInt32(firstFighterStr[1]),
                        FighterBlueScore = string.IsNullOrEmpty(secondFighterStr[1].ToString()) ? 0 : Convert.ToInt32(secondFighterStr[1]),
                        FighterBlueName = secondFighterStr[0]?.ToString(),
                        FighterRedRange = $"{sheetName}!P{startIndex + i}:P{startIndex + 16}",
                        FighterBlueRange = $"{sheetName}!P{startIndex + i + 16}:P{startIndex + i + 16}",
                    };
                    battlePairs.Add(item);
                }
            }
            return battlePairs;
        }
    }
}

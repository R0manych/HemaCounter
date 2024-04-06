using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers
{
    public class GetBattlePairsForEggsTemplateService : BaseHandler
    {
        public GetBattlePairsForEggsTemplateService(string sheetId) : base(sheetId)
        {
        }

        public IEnumerable<BattlePair> GetBattlePairsForFightRound1(string sheetName, int participantsCount)
        {
            var lastStr = participantsCount * 2 - 2 + 9;
            var startIndex = 9;
            var range = $"{sheetName}!G{startIndex}:H{lastStr}";
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
                        FighterRedRange = $"{sheetName}!H{startIndex + i}:H{startIndex + i}",
                        FighterBlueRange = $"{sheetName}!H{startIndex + i + 1}:H{startIndex + i + 1}",
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
            var range = $"{sheetName}!J{startIndex}:K{lastStr}";
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
                        FighterRedRange = $"{sheetName}!K{startIndex + i}:K{startIndex + i}",
                        FighterBlueRange = $"{sheetName}!K{startIndex + i + 4}:K{startIndex + i + 4}",
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
            var range = $"{sheetName}!M{startIndex}:N{lastStr}";
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
                        FighterRedRange = $"{sheetName}!N{startIndex + i}:N{startIndex + i}",
                        FighterBlueRange = $"{sheetName}!N{startIndex + i + 8}:N{startIndex + i + 8}",
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
            var range = $"{sheetName}!P{startIndex}:Q{lastStr}";
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
                        FighterRedRange = $"{sheetName}!Q{startIndex + i}:Q{startIndex + 16}",
                        FighterBlueRange = $"{sheetName}!Q{startIndex + i + 16}:Q{startIndex + i + 16}",
                    };
                    battlePairs.Add(item);
                }
            }
            return battlePairs;
        }

        public IEnumerable<BattlePair> GetBattlePairsForFightRound5(string sheetName, int participantsCount)
        {
            var startIndex = 23;
            var lastStr = participantsCount * 2 - 2 + startIndex;
            var range = $"{sheetName}!S{startIndex}:T{lastStr}";
            var values = ExcelReader.Read(_sheetId, range);
            if (values == null)
                return new List<BattlePair>();

            var battlePairs = new List<BattlePair>();

            for (var i = 0; i < lastStr - startIndex; i += 64)
            {
                var firstFighterStr = values[i];
                var secondFighterStr = values[i + 32];
                {
                    BattlePair item = new()
                    {
                        FighterRedName = firstFighterStr[0]?.ToString(),
                        FighterRedScore = string.IsNullOrEmpty(firstFighterStr[1].ToString()) ? 0 : Convert.ToInt32(firstFighterStr[1]),
                        FighterBlueScore = string.IsNullOrEmpty(secondFighterStr[1].ToString()) ? 0 : Convert.ToInt32(secondFighterStr[1]),
                        FighterBlueName = secondFighterStr[0]?.ToString(),
                        FighterRedRange = $"{sheetName}!T{startIndex + i}:T{startIndex + i}",
                        FighterBlueRange = $"{sheetName}!T{startIndex + i + 32}:T{startIndex + i + 32}",
                    };
                    battlePairs.Add(item);
                }
            }
            return battlePairs;
        }
    }
}

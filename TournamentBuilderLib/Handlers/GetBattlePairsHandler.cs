﻿using ExcelLib;
using TournamentBuilderLib.Models;

namespace TournamentBuilderLib.Handlers;

public interface IGetBattlePairsHandler
{
    public IEnumerable<BattlePair> Execute(string sheetName, int participantsCount);
}

public class GetBattlePairsHandler : IGetBattlePairsHandler
{
    private readonly string SHEET_ID = GoogleSheet.Default.SHEET_ID;

    public IEnumerable<BattlePair> Execute(string sheetName, int participantsCount)
    {
        var range = $"{sheetName}!D1:H{participantsCount/2}";

        var values = ExcelReader.Read(SHEET_ID, range);
        if (values == null)
            return new List<BattlePair>();

        var battlePairs = new List<BattlePair>();
        int i = 1;
        foreach (var value in values)
        {
            BattlePair item = new()
            {
                FighterRedName = value[0]?.ToString(),
                FighterRedScore = string.IsNullOrEmpty(value[1].ToString()) ? 0 : Convert.ToInt32(value[1]),
                FighterBlueScore = string.IsNullOrEmpty(value[2].ToString()) ? 0 : Convert.ToInt32(value[2]),
                FighterBlueName = value[3]?.ToString(),
                IsStarted = value[4]?.ToString() == "1",
                Range = $"{sheetName}!D{i}:H{i}",
            };
            battlePairs.Add(item);
            i++;
        }
        return battlePairs;
    }
}

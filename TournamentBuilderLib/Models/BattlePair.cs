﻿namespace TournamentBuilderLib.Models;

public class BattlePair
{
    public BattlePair(string fighter1Name, string fighter2Name)
    {
        if (DateTime.Now.Ticks % 2 == 0)
        {
            FighterRedName = fighter1Name;
            FighterBlueName = fighter2Name;
        }
        else
        {
            FighterRedName = fighter2Name;
            FighterBlueName = fighter1Name;
        }
    }

    public BattlePair()
    {}

    public string FighterRedName { get; set; }

    public int FighterRedScore { get; set; }

    public string FighterBlueName { get; set; }

    public int FighterBlueScore { get; set; }

    public bool IsStarted { get; set; }

    public string Range { get; set; }

    public string Caption => $"{FighterRedName} - {FighterBlueName}";

    public bool IsDraw => FighterBlueScore == FighterRedScore;

    public string? LooserName => IsDraw ? null : FighterBlueScore > FighterRedScore ? FighterRedName : FighterBlueName;
}
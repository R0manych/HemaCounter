namespace TournamentBuilderLib.Participants.Models;

public class BattlePair
{
    public string FighterRedName { get; set; }

    public string FighterRedNameAddress { get;set; }

    public int FighterRedScore { get; set; }

    public string FighterRedScoreAddress { get; set; }

    public string FighterBlueName { get; set; }

    public int FighterBlueScore { get; set; }

    public string FighterBlueNameAddress { get; set; }

    public string FighterBlueScoreAddress { get; set; }

    public string Range { get; set; }

    public Winner Winner { get; set; }
}

public enum Winner
{
    Red = 0,
    Blue = 1,
}
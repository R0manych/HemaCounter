namespace TournamentBuilderLib.Models;

public class TeamParticipant : IParticipant
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<string> Fighters { get; set; } = new List<string>();
}

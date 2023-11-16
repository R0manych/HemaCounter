using System;

namespace HEMACounter.Models;

public class Stage
{
    public int Id { get; set; }

    public TimeSpan Duration { get; set; }

    public int MaxScore { get; set; }

    public int MaxDoubles {get; set;}
}

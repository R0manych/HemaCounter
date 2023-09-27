using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEMACounter.Models
{
    public class Stage
    {
        public int Id { get; set; }

        public TimeSpan Duration { get; set; }

        public int MaxScore { get; set; }

        public int MaxDoubles {get; set;}
    }
}

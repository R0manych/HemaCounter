using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEMACounter
{
    public static class Settings
    {
        public static Dictionary<string, string> Current = new Dictionary<string, string>();
        public static string CurrentSheetId => Current["DOC_URL"];
    }
}

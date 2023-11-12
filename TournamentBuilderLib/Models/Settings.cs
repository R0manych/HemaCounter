using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEMACounter
{
    public static class Settings
    {
        public static Dictionary<string, string> Current = new Dictionary<string, string>();
        public static string SheetId => Current["DOC_URL"];

        public static string CoverFileName => Current["COVER_FILENAME"];

        public static int? ScoresPerRound => Current["SCORES_PER_ROUND"].ToNullableInt();

        public static int? MaxDoubles => Current["MAX_DOUBLES"].ToNullableInt();

        public static int? StagesCount => Current["STAGES_COUNT"].ToNullableInt();

        public static TimeSpan? RoundTime => Current["ROUND_TIME"].ToNullableTimeSpan();

        public static bool TechDefeatByDoublesEnabled => Current["DOUBLES_TECHDEFEAT"].Trim().ToLower() == "да";

        public static double DoublesPenalty => Current["DOUBLES_PENALTY"].ToNullableDouble() ?? 0;
    }

    public static class ParseExtensions
    {
        public static int ToInt(this string source) => int.TryParse(source, out int result) ? result : 0;
        public static int? ToNullableInt(this string source) => int.TryParse(source, out int result) ? result : null;
        public static double? ToNullableDouble(this string source) => double.TryParse(source.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : null;
        public static TimeSpan? ToNullableTimeSpan(this string source) => int.TryParse(source, out int result) ? TimeSpan.FromSeconds(result) : null;
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GCore.Data.Filter;

public class BlackWhitePathFilter : BlackWhiteRegexFilter
{
    public BlackWhitePathFilter(IEnumerable<string> black, IEnumerable<string> white)
        : base(black.Select(PatternToRegex), white.Select(PatternToRegex)) { }

    public static string PatternToRegex(string pattern)
        => "^" + Regex.Escape(pattern)
            .Replace("\\\\", "/")
            .Replace("\\*\\*", ".*")
            .Replace("\\*", "[^/]*")
            .Replace("\\?", "[^/]{1}") + "$";
    
    public override bool Passes(string elem) => base.Passes(elem.Replace("\\", "/"));
}
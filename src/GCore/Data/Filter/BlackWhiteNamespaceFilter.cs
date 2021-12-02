using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GCore.Data.Filter;

public class BlackWhiteNamespaceFilter : BlackWhiteRegexFilter
{
    public BlackWhiteNamespaceFilter(IEnumerable<string> black ,IEnumerable<string> white) 
        : base(black.Select(PatternToRegex), white.Select(PatternToRegex)) {}

    public static string PatternToRegex(string pattern) 
        => "^" + Regex.Escape(pattern)
            .Replace("\\*\\*", ".*")
            .Replace("\\*", "[^\\.]*")
            .Replace("\\?", "[^\\.]{1}") + "$";

}
using System.Text.RegularExpressions;

namespace GCore.Data.Filter;

public class RegexMatchFilter : IFilter<string>
{
    public Regex Pattern { get; private set; }

    public RegexMatchFilter(Regex pattern)
    {
        Pattern = pattern;
    }

    public RegexMatchFilter(string pattern) : this(new Regex(pattern)) { }

    public bool Passes(string elem) => Pattern.IsMatch(elem);
}
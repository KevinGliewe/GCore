using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GCore.Data.Filter;

public class BlackWhiteRegexFilter : IFilter<string>
{
    protected Regex[] _black;
    protected Regex[] _white;

    protected BlackWhiteRegexFilter() { }
    public BlackWhiteRegexFilter(IEnumerable<Regex> black, IEnumerable<Regex> white)
    {
        _black = black.ToArray();
        _white = white.ToArray();
    }

    public BlackWhiteRegexFilter(IEnumerable<string> black, IEnumerable<string> white)
        : this(black.Select(str => new Regex(str)), white.Select(str => new Regex(str)))
    {}

    public virtual bool Passes(string elem)
    {
        foreach (var blk in _black)
            if (blk.IsMatch(elem))
                return false;

        foreach (var wht in _white)
            if (wht.IsMatch(elem))
                return true;

        return false;
    }
}
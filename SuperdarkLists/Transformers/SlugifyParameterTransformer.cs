using System.Text.RegularExpressions;

namespace SuperdarkLists.Transformers;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is null)
        {
            return null;
        }

        string slugifyString = Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower();

        return slugifyString;
    }
}
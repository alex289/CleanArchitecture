using System;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Application.ViewModels.Sorting;

public sealed class SortQuery
{
    private string? _query = string.Empty;

    public ReadOnlyCollection<SortParameter> Parameters { get; private set; } = new(Array.Empty<SortParameter>());


    [FromQuery(Name = "order_by")]
    public string? Query
    {
        get => _query;
        set
        {
            _query = value;
            Parameters = ParseQuery(_query);
        }
    }

    public static ReadOnlyCollection<SortParameter> ParseQuery(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ReadOnlyCollection<SortParameter>(Array.Empty<SortParameter>());
        }

        if (value.Length > short.MaxValue)
        {
            throw new ArgumentException($"sort query can not be longer than {short.MaxValue} characters");
        }

        value = value.ToLower();

        if (!value.Contains(','))
        {
            return new ReadOnlyCollection<SortParameter>(new[] { GetParam(value) });
        }

        var @params = value.Split(',');
        var parsedParams = new SortParameter[@params.Length];

        for (var i = 0; i < @params.Length; i++)
        {
            parsedParams[i] = GetParam(@params[i]);
        }

        return new ReadOnlyCollection<SortParameter>(parsedParams);
    }

    private static SortParameter GetParam(string value)
    {
        value = value.Trim();

        var queryInfo = FindTokens(value);

        if (queryInfo.OpeningBracketIndex > 0)
        {
            // asc(name), desc(name), ascending(name), descending(name)
            return GetSortParamFromFunctionalStyle(value, queryInfo);
        }

        // i.e. "name asc", "name descending" or similar
        if (queryInfo.FirstSpaceIndex >= 0)
        {
            return GetSortParamFromSentence(value, queryInfo);
        }

        // name, +name, -name, name+, name-
        return GetSortParamFromSingleWord(value, queryInfo);
    }

    private static SortParameter GetSortParamFromSentence(string value, QueryInfo info)
    {
        var secondWordStartIndex = FindNextNonWhitespaceCharacter(value, info.FirstSpaceIndex + 1);

        if (secondWordStartIndex < 0)
        {
            throw new ArgumentException("Expected query string in form of \"{param} asc/desc\"");
        }

        var paramName = value[..info.FirstSpaceIndex];
        var orderName = value[secondWordStartIndex..];

        if (orderName == "asc" || orderName == "ascending")
        {
            return new SortParameter(paramName, SortOrder.Ascending);
        }

        if (orderName == "desc" || orderName == "descending")
        {
            return new SortParameter(paramName, SortOrder.Descending);
        }

        throw new ArgumentException(
            $"Unsupported sort order {orderName}. Valid are 'asc', 'ascending', 'desc' or 'descending'");
    }

    private static SortParameter GetSortParamFromSingleWord(string value, QueryInfo info)
    {
        if (info.PlusSignIndex < 0 && info.MinusSignIndex < 0)
        {
            return new SortParameter(value, SortOrder.Ascending);
        }

        var order = info.PlusSignIndex >= 0 ? SortOrder.Ascending : SortOrder.Descending;
        var indicatorIndex = Math.Max(info.MinusSignIndex, info.PlusSignIndex);

        if (indicatorIndex == 0)
        {
            return new SortParameter(value[1..], order);
        }

        return new SortParameter(value[..indicatorIndex], order);
    }

    private static SortParameter GetSortParamFromFunctionalStyle(string value, QueryInfo info)
    {
        var param = value
            .Substring(info.OpeningBracketIndex + 1, info.ClosingBracketIndex - info.OpeningBracketIndex - 1)
            .Trim();

        if (string.IsNullOrWhiteSpace(param))
        {
            throw new FormatException("Parameter name could not be extracted");
        }

        if (value.StartsWith("asc(") || value.StartsWith("ascending("))
        {
            return new SortParameter(param, SortOrder.Ascending);
        }

        if (value.StartsWith("desc(") || value.StartsWith("descending("))
        {
            return new SortParameter(param, SortOrder.Descending);
        }

        throw new FormatException("Unparsable sort query");
    }

    private static QueryInfo FindTokens(string query)
    {
        short plusSignIndex = -1;
        short minusSignIndex = -1;
        short firstSpaceIndex = -1;
        short openingBracketIndex = -1;
        short closingBracketIndex = -1;

        for (short i = 0; i < query.Length; i++)
        {
            switch (query[i])
            {
                case '(':
                    if (openingBracketIndex >= 0)
                    {
                        throw new FormatException("Only one bracket is allowed in functional style queries");
                    }

                    if (plusSignIndex >= 0 || minusSignIndex >= 0)
                    {
                        throw new FormatException(
                            "Order indicator (\"+\", \"-\") can not be used together with functional style (i.e.\"(name)\")");
                    }

                    if (firstSpaceIndex >= 0)
                    {
                        throw new FormatException($"Unexpected whitespace at position {firstSpaceIndex + 1}");
                    }

                    openingBracketIndex = i;
                    break;

                case ')':
                    if (closingBracketIndex >= 0)
                    {
                        throw new FormatException("Only one closing bracket is allowed in functional style queries");
                    }

                    if (openingBracketIndex < 0)
                    {
                        throw new FormatException("Closing brackets can only be places after opening brackets");
                    }

                    closingBracketIndex = i;
                    break;

                case '+':
                    if (plusSignIndex >= 0)
                    {
                        throw new FormatException("Only one positive order indicator \"+\" is allowed per query");
                    }

                    if (minusSignIndex >= 0)
                    {
                        throw new FormatException("Only one order indicator (\"+\", \"-\") is allowed per query");
                    }

                    if (firstSpaceIndex >= 0)
                    {
                        throw new FormatException($"Unexpected whitespace at position {firstSpaceIndex + 1}");
                    }

                    plusSignIndex = i;
                    break;

                case '-':
                    if (minusSignIndex >= 0)
                    {
                        throw new FormatException("Only one negative order indicator \"-\" is allowed per query");
                    }

                    if (plusSignIndex >= 0)
                    {
                        throw new FormatException("Only one order indicator (\"+\", \"-\") is allowed per query");
                    }

                    if (firstSpaceIndex >= 0)
                    {
                        throw new FormatException($"Unexpected whitespace at position {firstSpaceIndex + 1}");
                    }

                    minusSignIndex = i;
                    break;

                case ' ':
                    if (firstSpaceIndex == -1)
                    {
                        firstSpaceIndex = i;
                    }

                    if (minusSignIndex >= 0 || plusSignIndex >= 0)
                    {
                        throw new FormatException($"Unexpected whitespace at position {i + 1}");
                    }

                    break;

                default:
                    // Check for stuff after query end like "asc(name)blabla
                    // "+" and "-" can be either at the start or at the end, we are only interested 
                    // in the case where it's at the end.
                    if (plusSignIndex > 0 || minusSignIndex > 0 || closingBracketIndex >= 0)
                    {
                        var endOfQuery = Math.Max(Math.Max(plusSignIndex, minusSignIndex), closingBracketIndex);

                        throw new FormatException($"End of query expected at {endOfQuery}");
                    }

                    break;
            }
        }

        return new QueryInfo(
            plusSignIndex,
            minusSignIndex,
            firstSpaceIndex,
            openingBracketIndex,
            closingBracketIndex);
    }

    private static int FindNextNonWhitespaceCharacter(string value, int startIndex)
    {
        for (var i = startIndex; i < value.Length; i++)
        {
            if (!char.IsWhiteSpace(value[i]))
            {
                return i;
            }
        }

        return -1;
    }

    private readonly struct QueryInfo
    {
        public readonly short PlusSignIndex;
        public readonly short MinusSignIndex;
        public readonly short FirstSpaceIndex;
        public readonly short OpeningBracketIndex;
        public readonly short ClosingBracketIndex;

        public QueryInfo(
            short plusSignIndex,
            short minusSignIndex,
            short firstSpaceIndex,
            short openingBracketIndex,
            short closingBracketIndex)
        {
            PlusSignIndex = plusSignIndex;
            MinusSignIndex = minusSignIndex;
            FirstSpaceIndex = firstSpaceIndex;
            OpeningBracketIndex = openingBracketIndex;
            ClosingBracketIndex = closingBracketIndex;
        }
    }
}
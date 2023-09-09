namespace CleanArchitecture.Application.ViewModels.Sorting;

public readonly struct SortParameter
{
    public SortOrder Order { get; }
    public string ParameterName { get; }

    public SortParameter(string parameterName, SortOrder order)
    {
        Order = order;
        ParameterName = parameterName;
    }
}
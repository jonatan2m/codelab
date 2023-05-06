// See https://aka.ms/new-console-template for more information
public class SolutionWeightComparer : IComparer<Solution>
{
    //The "!" symbol is called the null-forgiving operator and 
    //is used to tell the compiler that a nullable variable
    //will never be null at that point in the code.
    //In the given code, it is used to indicate that x will never be null 
    //and avoid a possible null reference exception.
    public int Compare(Solution? x, Solution? y)
        => x!.TotalWeight.CompareTo(y!.TotalWeight);    
}

public class SolutionValueAndWeightComparer : IComparer<Solution>
{
    //The "!" symbol is called the null-forgiving operator and 
    //is used to tell the compiler that a nullable variable
    //will never be null at that point in the code.
    //In the given code, it is used to indicate that x will never be null 
    //and avoid a possible null reference exception.
    public int Compare(Solution? x, Solution? y)
    {
        var result = x!.TotalValue.CompareTo(y!.TotalValue);

        result = result != 0 
        ? result 
        : x!.TotalWeight.CompareTo(y!.TotalWeight) * -1;

        return result;
    }
}

public class SolutionValueAndWeightAndItemsComparer : IComparer<Solution>
{
    //The "!" symbol is called the null-forgiving operator and 
    //is used to tell the compiler that a nullable variable
    //will never be null at that point in the code.
    //In the given code, it is used to indicate that x will never be null 
    //and avoid a possible null reference exception.
    public int Compare(Solution? x, Solution? y)
    {
        var result = x!.TotalValue.CompareTo(y!.TotalValue);

        if(result == 0)        
            result = x!.TotalWeight.CompareTo(y!.TotalWeight) * -1;

        if(result == 0)        
            result = x!.ItemsCount.CompareTo(y!.ItemsCount) * -1;

        return result;
    }
}
namespace _21stMortgageInterviewApplication;

using System;
using System.Collections.Generic;
using System.Linq;

internal class MainWindowViewModel : BaseViewModel
{
    #region InputString
    private IEnumerable<long> longs; // parsed longs
    private string inputString = string.Empty;
    public string InputString
    {
        get => inputString;
        set => SetField(ref inputString, value, nameof(InputString));
    }
    #endregion InputString

    #region Result
    private long result;
    public long Result
    {
        get => result;
        set
        {
            if (SetField(ref result, value, nameof(Result))) 
                OnPropertyChanged(nameof(ResultIsPositive));
        }
    }

    #endregion Result

    public bool ResultIsPositive => Result >= 0;

    public MainWindowViewModel()
    {
        FindLargestValueCommand = new RelayCommand(ExecuteFindLargestValueCommand, CanFindLargestValueCommand);
        FindSumOfOddNumbersCommand = new RelayCommand(ExecuteFindSumOfOddNumbersCommand, CanFindSumOfOddNumbersCommand);
        FindSumOfEvenNumbersCommand = new RelayCommand(ExecuteFindSumOfEvenNumbersCommand, CanFindSumOfEvenNumbersCommand);
    }

    #region FindLargestValueCommand
    public RelayCommand FindLargestValueCommand { get; }
    public bool CanFindLargestValueCommand(object p)
    {
        return string.IsNullOrWhiteSpace(InputString) || longs != null;
    }
    public void ExecuteFindLargestValueCommand(object p)
    {
        Result = string.IsNullOrWhiteSpace(InputString) ? 0 : longs.Max();
    }
    #endregion FindLargestValueCommand
    
    #region FindSumOfOddNumbersCommand
    public RelayCommand FindSumOfOddNumbersCommand { get; }
    public bool CanFindSumOfOddNumbersCommand(object p)
    {
        return string.IsNullOrWhiteSpace(InputString) || longs != null;
    }
    public void ExecuteFindSumOfOddNumbersCommand(object p)
    {
        Result = string.IsNullOrWhiteSpace(InputString) ? 0 : longs.Sum(x => x % 2 == 0 ? 0 : x);
    }
    #endregion FindSumOfOddNumbersCommand

    #region FindSumOfEvenNumbersCommand
    public RelayCommand FindSumOfEvenNumbersCommand { get; }
    public bool CanFindSumOfEvenNumbersCommand(object p)
    {
        return string.IsNullOrWhiteSpace(InputString) || longs != null;
    }
    public void ExecuteFindSumOfEvenNumbersCommand(object p)
    {
        Result = string.IsNullOrWhiteSpace(InputString) ? 0 : longs.Sum(x => x % 2 == 0 ? x : 0);
    }
    #endregion FindSumOfEvenNumbersCommand

    public override void ValidateProperty(string propertyName)
    {
        base.ValidateProperty(propertyName);
        
        switch (propertyName)
        {
            case nameof(InputString):
                try
                {
                    longs = SplitValues(inputString);
                }
                catch (Exception e)
                {
                    longs = null;
                    AddError(nameof(InputString), e.Message);
                }
                break;
        }
        FindLargestValueCommand.RaiseCanExecuteChanged();
        FindSumOfOddNumbersCommand.RaiseCanExecuteChanged();
        FindSumOfEvenNumbersCommand.RaiseCanExecuteChanged();
    }

    private static IEnumerable<long> SplitValues(string s)
    {
        var values = s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var longs = new List<long>();
        foreach (var str in values)
        {
            if (!long.TryParse(str, out _))
                throw new Exception($"\"{str}\" is not a number");
            longs.Add(long.Parse(str));
        }
        return longs;
    }
}

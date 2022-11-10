namespace _21stMortgageInterviewApplication;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

internal class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    #region Property Change
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Do not call directly.
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, string propertyName)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        ValidatePropertyInternal(propertyName);
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion

    #region Errors

    private readonly Dictionary<string, List<string>> errorsByPropertyName = new();
    
    public bool HasErrors => errorsByPropertyName.Any();
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public IEnumerable GetErrors(string propertyName)
    {
        return errorsByPropertyName.ContainsKey(propertyName) ?
            errorsByPropertyName[propertyName] : null;
    }

    protected void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    private void ValidatePropertyInternal(string propertyName)
    {
        ClearErrors(propertyName);
        ValidateProperty(propertyName);
        OnErrorsChanged(propertyName);
    }
    
    /// <summary>
    /// Use AddError(propertyName, error) to record an error.
    /// </summary>
    /// <param name="propertyName"></param>
    public virtual void ValidateProperty(string propertyName)
    {
        
    }
    
    /// <summary>
    /// Use to add errors inside ValidateProperty().
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="error"></param>
    protected void AddError(string propertyName, string error)
    {
        if (!errorsByPropertyName.ContainsKey(propertyName))
            errorsByPropertyName[propertyName] = new List<string>();

        if (!errorsByPropertyName[propertyName].Contains(error))
        {
            errorsByPropertyName[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }

    private void ClearErrors(string propertyName)
    {
        if (errorsByPropertyName.ContainsKey(propertyName))
        {
            errorsByPropertyName.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
    }

    #endregion
}

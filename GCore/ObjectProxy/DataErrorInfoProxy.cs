
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Dynamic;

namespace GCore.ObjectProxy
{
  public class DataErrorInfoProxy : ValidatingProxy, IDataErrorInfo
  {
    protected override bool Validate(PropertyInfo propertyInfo, object value)
    {
      var returnValue = base.Validate(propertyInfo, value);
      return returnValue;
    }

    #region constructor
    public DataErrorInfoProxy() : base() { }

    public DataErrorInfoProxy(object proxiedObject) : base(proxiedObject) { }
    #endregion

    #region public methods
    public override bool TryConvert(ConvertBinder binder, out object result)
    {
      if (binder.Type == typeof(IDataErrorInfo))
      {
        result = this;
        return true;
      }
      else
        return base.TryConvert(binder, out result);
    }
    #endregion

    #region IDataErrorInfo Member
    public string Error
    {
      get
      {
        var returnValue = new StringBuilder();

        foreach (var item in _validationResults)
          foreach (var validationResult in item.Value)
            returnValue.AppendLine(validationResult.ErrorMessage);

        return returnValue.ToString();
      }
    }

    public string this[string columnName]
    {
      get
      {
        return _validationResults.ContainsKey(columnName) ?
          string.Join(Environment.NewLine, _validationResults[columnName]) :
          string.Empty;
      }
    }
    #endregion
  }
}

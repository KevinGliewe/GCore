using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.ObjectModel;

namespace GCore.ObjectProxy
{
  public class ValidatingProxy : EditableProxy
  {
    #region protected members
    protected Dictionary<string, Collection<ValidationResult>> _validationResults = new Dictionary<string, Collection<ValidationResult>>();
    #endregion

    #region protected methods
    protected override void SetMember(string propertyName, object value)
    {
      if (ValidateOnChange)
        Validate(propertyName, value);

      base.SetMember(propertyName, value);      
    }

    protected virtual IEnumerable<ValidationAttribute> GetValidationAttributes(PropertyInfo propertyInfo)
    {
      var validationAttributes = new List<ValidationAttribute>();

      foreach (ValidationAttribute item in propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true))
        validationAttributes.Add(item);

      return validationAttributes;
    }

    protected virtual bool Validate(PropertyInfo propertyInfo, object value)
    {
      var validationAttributes = GetValidationAttributes(propertyInfo);
      if (validationAttributes.Count<ValidationAttribute>() == 0)
        return true;

      var validationContext = new ValidationContext(ProxiedObject, null, null);
      var validationResults = new Collection<ValidationResult>();

      var returnValue = Validator.TryValidateValue(
        value,
        validationContext,
        validationResults,
        validationAttributes);

      if (returnValue)
      {
        if (_validationResults.ContainsKey(propertyInfo.Name))
          _validationResults.Remove(propertyInfo.Name);
      }
      else
      {
        if (_validationResults.ContainsKey(propertyInfo.Name))
          _validationResults[propertyInfo.Name] = validationResults;
        else
          _validationResults.Add(propertyInfo.Name, validationResults);
      }

      return returnValue;
    }

    protected virtual bool Validate(string propertyName, object value)
    {
      return Validate(GetPropertyInfo(propertyName), value);
    }
    #endregion

    #region constructor
    public ValidatingProxy() : base() 
    { 
      ValidateOnChange = true; 
    }

    public ValidatingProxy(object proxiedObject) : base(proxiedObject) 
    {
      ValidateOnChange = true;
    }
		#endregion

    #region public methods
    public virtual bool Validate(PropertyInfo propertyInfo)
    {
      return Validate(propertyInfo, GetMember(propertyInfo.Name));
    }

    public virtual bool Validate(string propertyName)
    {
      return Validate(GetPropertyInfo(propertyName));
    }

    public virtual bool Validate()
    {
      var propertiesToValidate = ProxiedObject.GetType().GetProperties().Where(
        pi => pi.GetCustomAttributes(typeof(ValidationAttribute), true).Length > 0);

      var returnValue = false;
      foreach (var item in propertiesToValidate)
        returnValue &= Validate(item);

      return returnValue;
    }
    #endregion

    #region public properties
    public bool ValidateOnChange { get; set; }
    public bool HasErrors { get { return _validationResults.Count > 0; } }
    #endregion
  }
}

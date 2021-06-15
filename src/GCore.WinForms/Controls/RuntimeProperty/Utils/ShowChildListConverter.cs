/* ****************************************************************************
 *  RuntimeObjectEditor
 * 
 * Copyright (c) 2005 Corneliu I. Tusnea
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author be held liable for any damages arising from 
 * the use of this software.
 * Permission to use, copy, modify, distribute and sell this software for any 
 * purpose is hereby granted without fee, provided that the above copyright 
 * notice appear in all copies and that both that copyright notice and this 
 * permission notice appear in supporting documentation.
 * 
 * Corneliu I. Tusnea (corneliutusnea@yahoo.com.au)
 * www.acorns.com.au
 * ****************************************************************************/


using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace GCore.WinForms.Controls.RuntimeProperty.Utils
{
	/// <summary>
	/// Summary description for ShowChildListConverter.
	/// </summary>
	public class ShowChildListConverter : TypeConverter
	{
		private readonly TypeConverter originalConverter;

		public ShowChildListConverter(TypeConverter originalConverter)
		{
			this.originalConverter = originalConverter;
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			// try to convert to IList
			if (context != null && (context.PropertyDescriptor.GetValue(context.Instance) is ICollection))
			{
				return true;
			}
			else
			{
				return originalConverter.GetPropertiesSupported(context);
			}
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection propCollection = originalConverter.GetProperties(context, value, attributes);
			if (value is IEnumerable)
			{
				// build a descriptor for each item!!!
				IEnumerable valueCollection = value as IEnumerable;
				ArrayList newProps = new ArrayList();
				int index = 0;
				foreach (object child in valueCollection)
				{
					newProps.Add(new SimpleChildDescriptor(value, index, child));
					index++;
				}
				PropertyDescriptorCollection itemsCollection = PropertyDescriptorUtils.GetProperties(newProps);
				return itemsCollection;
				//return PropertyDescriptorUtils.MergeProperties(itemsCollection, propCollection);
			}
			else
			{
				return propCollection;
			}
		}

		#region Delegates

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return originalConverter.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return originalConverter.CanConvertTo(context, destinationType);
		}


		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return originalConverter.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return originalConverter.ConvertTo(context, culture, value, destinationType);
		}


		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return originalConverter.CreateInstance(context, propertyValues);
		}

//		protected Exception GetConvertFromException(object value)
//		{
//			return originalConverter.GetConvertFromException(value);
//		}
//
//		protected Exception GetConvertToException(object value, Type destinationType)
//		{
//			return originalConverter.GetConvertToException(value, destinationType);
//		}


		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return originalConverter.GetCreateInstanceSupported(context);
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return originalConverter.GetStandardValues(context);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return originalConverter.GetStandardValuesExclusive(context);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return originalConverter.GetStandardValuesSupported(context);
		}

		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			return originalConverter.IsValid(context, value);
		}

//		protected PropertyDescriptorCollection SortProperties(PropertyDescriptorCollection props, string[] names)
//		{
//			return originalConverter.SortProperties(props, names);
//		}

		#endregion

		#region SimpleChildDescriptor

		public class SimpleChildDescriptor : AbstractPropertyDescriptor, IRealValueHolder
		{
			private readonly object value;
			private readonly int index;
			private readonly object childValue;

			public SimpleChildDescriptor(object value, int index, object childValue)
				: base("Item:" + index.ToString())
			{
				this.value = value;
				this.index = index;
				this.childValue = childValue;
			}

			public override object GetValue(object component)
			{
				return this;
			}

			public override Type ComponentType
			{
				get { return value.GetType(); }
			}

			public override Type PropertyType
			{
				get { return this.GetType(); }
			}

			#region IRealValueHolder Members

			public object RealValue
			{
				get { return childValue; }
			}

			#endregion

			public override string ToString()
			{
				if (childValue != null)
				{
					return childValue.ToString();
				}
				else
				{
					return null;
				}
			}

		}

		#endregion
	}
}
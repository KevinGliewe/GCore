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
using System.ComponentModel;
using System.Globalization;
using GCore.WinForms.Controls.RuntimeProperty.Utils;

namespace GCore.WinForms.Controls.RuntimeProperty.Tabs.ProcessInfo
{
	/// <summary>
	/// ChildTypeConverter 
	/// </summary>
	public class ChildTypeConverter : TypeConverter
	{
		public ChildTypeConverter()
		{
		}

		#region Convert

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return true;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return true;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return value;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof (string) && value != null)
			{
				return value.ToString();
			}
			else
			{
				return value;
			}
		}

		#endregion

		#region Get

		//public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		//{
		//    return true;
		//}
		//public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		//{
		//    return true;
		//}
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			return true;
		}

		//public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		//{
		//    ArrayList actions = new ArrayList();
		//    actions.Add("Invoke Now");
		//    return new TypeConverter.StandardValuesCollection(actions);
		//}

		#endregion

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
			//if (method.ParametersCount != 0)
			//{
			//    return true;
			//}
			//else
			//{
			//    if (method.MethodInfo.ReturnType == typeof(void) || method.MethodInfo.ReturnType.IsPrimitive)
			//        return false;
			//    else
			//        return true;
			//}
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			//return method.GetChildProperties(null, null);
			return PropertyDescriptorUtils.GetAllProperties(null, value, attributes);
		}
	}
}
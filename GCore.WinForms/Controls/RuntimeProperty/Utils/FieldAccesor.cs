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
using System.Reflection;

namespace GCore.WinForms.Controls.RuntimeProperty.Utils
{
	/// <summary>
	/// Summary description for FieldAccesor.
	/// </summary>
	public class FieldAccesor
	{
		private readonly string fieldName;
		private readonly Type targetType;

		private object target;
		private FieldInfo fieldInfo;

		private object value;

		public FieldAccesor(object target, string fieldName)
			: this(target.GetType(), target, fieldName)
		{
		}

		public FieldAccesor(Type targetType, string fieldName)
			: this(targetType, null, fieldName)
		{
		}

		public FieldAccesor(Type targetType, object target, string fieldName)
		{
			this.target = target;
			this.targetType = targetType;
			this.fieldName = fieldName;

			do
			{
				TryReadField(BindingFlags.Default);
				TryReadField(BindingFlags.Instance | BindingFlags.FlattenHierarchy);
				TryReadField(BindingFlags.Static | BindingFlags.FlattenHierarchy);

				TryReadField(BindingFlags.NonPublic | BindingFlags.Instance);

				TryReadField(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
				TryReadField(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
				TryReadField(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

				TryReadField(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn | BindingFlags.Instance | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.SetField | BindingFlags.Static);

				TryReadField(BindingFlags.NonPublic | BindingFlags.Static);
				TryReadField(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
				TryReadField(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField);

				if (fieldInfo == null)
				{
					this.targetType = this.targetType.BaseType;
					if (this.targetType == typeof (object))
					{ // no chance
						break;
					}
				}

			} while (fieldInfo == null);
		}

		private void SearchForField(BindingFlags bindingFlags)
		{
			if (fieldInfo != null)
				return;

			FieldInfo[] allFields = targetType.GetFields(bindingFlags);
			foreach (FieldInfo field in allFields)
			{
				if (field.Name == this.fieldName)
				{
					this.fieldInfo = field;
					return;
				}
			}
		}


		private void TryReadField(BindingFlags bindingFlags)
		{
			if (fieldInfo != null)
				return;
			fieldInfo = targetType.GetField(fieldName, bindingFlags);
			if (fieldInfo == null)
			{
				SearchForField(bindingFlags);
			}
		}

		public void Save()
		{
			value = fieldInfo.GetValue(target);
		}

		public void Clear()
		{
			fieldInfo.SetValue(target, null);
		}

		public void Restore()
		{
			fieldInfo.SetValue(target, value);
		}

		public void Restore(object newValue)
		{
			fieldInfo.SetValue(target, newValue);
			this.value = newValue;
		}

		public object Target
		{
			get { return target; }
			set { this.target = value; }
		}

		public bool IsValid
		{
			get { return this.fieldInfo != null; }
		}

		public object Value
		{
			get { return this.value; }
		}
	}
}
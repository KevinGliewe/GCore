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

namespace GCore.WinForms.Controls.RuntimeProperty.Tabs.ProcessInfo
{
	/// <summary>
	/// Property Descriptor that can be applied on a different object than the target
	/// but will report it's part of the target object.
	/// </summary>
	public class ReroutedPropertyDescriptor : PropertyDescriptor
	{
		private object targetComponent;
		private object realValue;

		public ReroutedPropertyDescriptor(string name, object realValue, object targetComponent)
			: base(name, null)
		{
			this.targetComponent = targetComponent;
			this.realValue = realValue;
		}

		protected override void FillAttributes(IList attributeList)
		{
			base.FillAttributes(attributeList);
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override void ResetValue(object component)
		{
		}

		public override void SetValue(object component, object value)
		{
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		public override object GetValue(object component)
		{
			return realValue;
		}

		public override bool IsReadOnly
		{
			get { return true; }
		}

		public override Type PropertyType
		{
			get
			{
				if (realValue != null)
				{
					return realValue.GetType();
				}
				else
				{
					return null;
				}
			}
		}

		public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			return base.GetChildProperties(instance, filter);
		}

		public override Type ComponentType
		{
			get
			{
				if (targetComponent != null)
				{
					return targetComponent.GetType();
				}
				else
				{
					return null;
				}
			}
		}

		public override TypeConverter Converter
		{
			get
			{
				return base.Converter;
				//converter = new MethodEditingConverter(this);
			}
		}
	}
}
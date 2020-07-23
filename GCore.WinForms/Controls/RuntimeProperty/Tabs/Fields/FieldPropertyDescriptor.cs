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
using System.ComponentModel.Design;
using System.Reflection;
using GCore.WinForms.Controls.RuntimeProperty.Tabs.Methods;
using GCore.WinForms.Controls.RuntimeProperty.Utils;

namespace GCore.WinForms.Controls.RuntimeProperty.Tabs.Fields
{
	/// <summary>
	/// Summary description for FieldPropertyDescriptor.
	/// </summary>
	public class FieldPropertyDescriptor : AbstractPropertyDescriptor
	{
		private readonly object component;
		private readonly FieldInfo field;
		private readonly Type ownerType;
		private readonly int depth;

		public FieldPropertyDescriptor(object component, FieldInfo field, Type ownerType, int depth)
			: base(field.Name)
		{
			this.component = component;
			this.field = field;
			this.ownerType = ownerType;
			this.depth = depth;
		}

		#region PropertyDescriptor implementation

		public override Type ComponentType
		{
			get { return component.GetType(); }
		}

		public override bool IsReadOnly
		{
			get { return false; }
		}

		#endregion

		protected override void FillAttributes(System.Collections.IList attributeList)
		{
			base.FillAttributes (attributeList);

			attributeList.Add(new CategoryAttribute( depth.ToString() + ". " + ownerType.Name ));
			attributeList.Add(new RefreshPropertiesAttribute(RefreshProperties.Repaint));;
			attributeList.Add(new DesignerAttribute(typeof (MethodDesigner), typeof (IDesigner)));
			attributeList.Add(new DesignTimeVisibleAttribute(false));
		}

		public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			return base.GetChildProperties (instance, filter);
		}
		public override AttributeCollection Attributes
		{
			get
			{
				return base.Attributes;
			}
		}



		public override void SetValue(object component, object value)
		{
			this.field.SetValue(component, value);
		}

		public override object GetValue(object component)
		{
			return this.field.GetValue(component);
		}

		public override TypeConverter Converter
		{
			get { return base.Converter; }
		}

		public override Type PropertyType
		{
			get { return field.FieldType; }
		}

		#region Properties

		public object Component
		{
			get { return component; }
		}

		public FieldInfo FieldInfo
		{
			get { return field; }
		}

		#endregion
	}
}
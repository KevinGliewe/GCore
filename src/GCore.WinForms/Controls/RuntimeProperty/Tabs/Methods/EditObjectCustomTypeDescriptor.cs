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

namespace GCore.WinForms.Controls.RuntimeProperty.Tabs.Methods
{
	/// <summary>
	/// Summary description for CustomTypeDescriptor.
	/// </summary>
	public class EditObjectCustomTypeDescriptor : ICustomTypeDescriptor
	{
		private readonly object editedObject;
		private MethodEditor methodEditor;

		public EditObjectCustomTypeDescriptor(object editedObject)
		{
			this.editedObject = editedObject;
			methodEditor = new MethodEditor();
		}

		#region ICustomTypeDescriptor Members

		public TypeConverter GetConverter()
		{
			return new TypeConverter();
		}

		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(editedObject, true);
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return editedObject;
		}

		public AttributeCollection GetAttributes()
		{
			AttributeCollection attributes = TypeDescriptor.GetAttributes(editedObject, true);

			//attributes = AttributeUtils.ReplaceAttribute(attributes, new DesignerAttribute(typeof(MethodDesigner), typeof(IDesigner)));
			//attributes = AttributeUtils.ReplaceAttribute(attributes, new DesignTimeVisibleAttribute(true));
			//attributes = AttributeUtils.ReplaceAttribute(attributes, new DesignTimeVisibleAttribute(true));

			return attributes;
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(editedObject);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return GetEvents();
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			//return MethodUtils.GetMethodProperties(this.editedObject);
			return TypeDescriptor.GetProperties(editedObject);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			//return MethodUtils.GetMethodProperties(this.editedObject);
			return TypeDescriptor.GetProperties(editedObject);
		}

		public object GetEditor(Type editorBaseType)
		{
			return methodEditor;
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(editedObject);
		}

		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(editedObject);
		}

		public string GetClassName()
		{
			return editedObject.GetType().Name;
		}

		#endregion
	}
}
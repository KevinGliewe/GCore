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
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Reflection;
using GCore.WinForms.Controls.RuntimeProperty.Tabs.Methods;

namespace GCore.WinForms.Controls.RuntimeProperty.Utils
{
	/// <summary>
	/// Remaps a Propertie's Descriptor Component type
	/// </summary>
	public class RemapPropertyDescriptor : PropertyDescriptor
	{
		private PropertyDescriptor originalPropertyDescriptor;
		private object reamappedComponent;
		private object originalComponent;
		private string displayNamePrefix;
		private TypeConverter typeConverter;

		public RemapPropertyDescriptor(PropertyDescriptor originalPropertyDescriptor, object reamappedComponent, object originalComponent, string displayNamePrefix, TypeConverter typeConverter)
			: base(originalPropertyDescriptor.Name, null)
		{
			this.originalPropertyDescriptor = originalPropertyDescriptor;
			this.reamappedComponent = reamappedComponent;
			this.originalComponent = originalComponent;
			this.displayNamePrefix = displayNamePrefix;
			this.typeConverter = typeConverter;
		}

		protected override void FillAttributes(IList attributeList)
		{	// this method is not used - we overrride the Attributes property and create there a new set of attributes
			base.FillAttributes(attributeList);
			attributeList.Add(new EditorAttribute(typeof (UITypeEditor), typeof (UITypeEditor)));
			attributeList.Add(new RefreshPropertiesAttribute(RefreshProperties.Repaint));
			attributeList.Add(new DesignerAttribute(typeof (ComponentDesigner), typeof (IDesigner)));
		}

		protected override AttributeCollection CreateAttributeCollection()
		{
			return base.CreateAttributeCollection();
		}

		public override string Name
		{
			get { return originalPropertyDescriptor.Name; }
		}

		public override string Description
		{
			get { return originalPropertyDescriptor.Description; }
		}

		public override bool IsBrowsable
		{
			get { return base.IsBrowsable; }
		}

		public object RemappedComponent
		{
			get { return this.reamappedComponent; }
		}

		public object OriginalComponent
		{
			get
			{
				//return this.originalComponent;
				return originalPropertyDescriptor.GetValue(this.originalComponent);
			}
		}

		public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			//return base.GetChildProperties(instance, filter);
			return originalPropertyDescriptor.GetChildProperties(originalComponent);
		}

		public override AttributeCollection Attributes
		{
			get
			{
				ArrayList tempAttributes = new ArrayList();

				AttributeCollection originalAttributes = originalPropertyDescriptor.Attributes;
				//AttributeUtils.PrintAttributes(originalAttributes);
				tempAttributes.AddRange( originalAttributes );
				/*tempAttributes.AddRange( AttributeUtils.DeleteNonRelevatAttributes(originalAttributes) );

				tempAttributes.Add(new EditorAttribute(typeof (UITypeEditor), typeof (UITypeEditor)));
				tempAttributes.Add(new RefreshPropertiesAttribute(RefreshProperties.Repaint));
				tempAttributes.Add(new DesignerAttribute(typeof (ComponentDesigner), typeof (IDesigner)));
*/
				return AttributeUtils.GetAttributes(tempAttributes);
			}
		}

		public override bool DesignTimeOnly
		{
			get { return false; }
		}

		public override object GetEditor(Type editorBaseType)
		{
			return originalPropertyDescriptor.GetEditor(editorBaseType);
			//return new MethodEditor();
		}

		public override bool CanResetValue(object component)
		{
			return originalPropertyDescriptor.CanResetValue(originalComponent);
		}

		public override void ResetValue(object component)
		{
			originalPropertyDescriptor.ResetValue(originalComponent);
		}

		public override void SetValue(object component, object value)
		{
			originalPropertyDescriptor.SetValue(originalComponent, value);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		public override object GetValue(object component)
		{
			try
			{
				return originalPropertyDescriptor.GetValue(originalComponent);
			}
			catch (TargetInvocationException ex)
			{
				return "Ex:" + ex.InnerException.Message;
			}
			catch (Exception ex)
			{
				return "Ex:" + ex.Message;
			}
		}

		public override bool IsReadOnly
		{
			get { return originalPropertyDescriptor.IsReadOnly; }
		}

		public override Type PropertyType
		{
			get { return originalPropertyDescriptor.PropertyType; }
		}

		public override Type ComponentType
		{
			get { return this.reamappedComponent.GetType(); }
		}

		public override string DisplayName
		{
			get
			{
				if (displayNamePrefix != null)
				{
					return displayNamePrefix + ":" + originalPropertyDescriptor.DisplayName;
				}
				else
				{
					return originalPropertyDescriptor.DisplayName;
				}
			}
		}

		public override TypeConverter Converter
		{
			get
			{
				//return originalPropertyDescriptor.Converter;
				return this.typeConverter;
			}
		}
	}
}
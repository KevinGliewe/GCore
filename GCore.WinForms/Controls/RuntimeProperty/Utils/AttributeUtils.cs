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
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GCore.WinForms.Controls.RuntimeProperty.Utils
{
	/// <summary>
	/// AttributeUtils 
	/// </summary>
	public sealed class AttributeUtils
	{
		private static Hashtable acceptableAttributes = new Hashtable();
		static AttributeUtils()
		{
			acceptableAttributes.Add(typeof(CategoryAttribute), null);
			acceptableAttributes.Add(typeof(DesignerAttribute), null);
			acceptableAttributes.Add(typeof(DefaultValueAttribute), null);
			acceptableAttributes.Add(typeof(DesignerSerializerAttribute), null);
			acceptableAttributes.Add(typeof(DispIdAttribute), null);			
			acceptableAttributes.Add(typeof(DesignerSerializationVisibility), null);
			acceptableAttributes.Add(typeof(DescriptionAttribute), null);

			acceptableAttributes.Add(typeof(EditorAttribute), null);
			acceptableAttributes.Add(typeof(DesignerCategoryAttribute), null);
			acceptableAttributes.Add(typeof(ParenthesizePropertyNameAttribute), null);
			acceptableAttributes.Add(typeof(RefreshPropertiesAttribute), null);
			acceptableAttributes.Add(typeof(TypeConverterAttribute), null);
		}
		private AttributeUtils()
		{
		}

		public static AttributeCollection ReplaceAttribute(AttributeCollection collection, Attribute newAttribute)
		{
			collection = RemoveAttribute(collection, newAttribute.GetType());
			collection = AddAttribute(collection, newAttribute);
			return collection;
		}

		public static ArrayList GetAttributes(AttributeCollection collection)
		{
			ArrayList attributes = new ArrayList();
			foreach (Attribute attr in collection)
			{
				attributes.Add(attr);
			}
			return attributes;
		}

		public static AttributeCollection GetAttributes(ArrayList attributes)
		{
			AttributeCollection collection = new AttributeCollection((Attribute[]) attributes.ToArray(typeof (Attribute)));
			return collection;
		}

		public static AttributeCollection AddAttribute(AttributeCollection collection, Attribute newAttribute)
		{
			ArrayList attributes = GetAttributes(collection);
			attributes.Add(newAttribute);
			return GetAttributes(attributes);
		}

		public static AttributeCollection DeleteNonRelevatAttributes(AttributeCollection collection)
		{
			ArrayList attributes = GetAttributes(collection);

			ArrayList newAttributes = new ArrayList();
			foreach (Attribute attr in attributes)
			{
				if (acceptableAttributes.ContainsKey(attr.GetType())
					|| acceptableAttributes.ContainsKey(attr.GetType().BaseType))
				{
					newAttributes.Add(attr);
				}
			}
			return GetAttributes(newAttributes);
		}

		public static AttributeCollection RemoveAttribute(AttributeCollection collection, Type attributeType)
		{
			ArrayList attributes = GetAttributes(collection);

			ArrayList newAttributes = new ArrayList();
			foreach (Attribute attr in attributes)
			{
				if (attr.GetType() != attributeType)
				{
					newAttributes.Add(attr);
				}
			}
			return GetAttributes(attributes);
		}

		public static void PrintAttributes(ICollection attributes)
		{
			foreach ( Attribute attr in attributes )
			{
				Trace.WriteLine(string.Format("-> {0,-30} {1}", attr.GetType().Name,attr.ToString()));
			}
		}
	}
}
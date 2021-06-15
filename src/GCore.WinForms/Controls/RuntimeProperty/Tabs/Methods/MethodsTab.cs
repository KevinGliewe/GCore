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
using System.Drawing;
using System.Windows.Forms.Design;

namespace GCore.WinForms.Controls.RuntimeProperty.Tabs.Methods
{
	/// <summary>
	/// MethodsTab - Tab showing all the methods of an object
	/// </summary>
	internal class MethodsTab : PropertyTab
	{
		public MethodsTab()
		{
		}

		public override Bitmap Bitmap
		{
			get { return (Bitmap) Bitmap.FromStream(GetType().Assembly.GetManifestResourceStream("GCore.WinForms.Controls.RuntimeProperty.Resources.Methods.bmp")); }
		}

		public override string TabName
		{
			get { return "Methods"; }
		}

		public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			if (component != null)
			{
				return MethodUtils.GetMethodProperties(component);
			}
			return null;
		}
	}
}
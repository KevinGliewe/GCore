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


using System.ComponentModel;

namespace GCore.WinForms.Controls.RuntimeProperty.Utils
{
	public abstract class AbstractPropertyDescriptor : PropertyDescriptor
	{
		protected AbstractPropertyDescriptor(string name)
			: base(name, null)
		{
		}

		#region PropertyDescriptor

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override bool IsReadOnly
		{
			get { return false; }
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

		#endregion
	}
}
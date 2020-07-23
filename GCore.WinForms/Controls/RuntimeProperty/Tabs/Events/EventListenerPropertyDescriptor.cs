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
using GCore.WinForms.Controls.RuntimeProperty.Utils;

namespace GCore.WinForms.Controls.RuntimeProperty.Tabs.Events
{
	public class EventListenerPropertyDescriptor : AbstractPropertyDescriptor, IRealValueHolder
	{
		private EventPropertyDescriptor eventDescriptor;
		private Delegate handler;

		public EventListenerPropertyDescriptor(EventPropertyDescriptor eventDescriptor, Delegate handler)
			: base(handler.Method.Name)
		{
			this.eventDescriptor = eventDescriptor;
			this.handler = handler;
		}

		protected override void FillAttributes(IList attributeList)
		{
			base.FillAttributes(attributeList);
			attributeList.Add(new RefreshPropertiesAttribute(RefreshProperties.All));
		}

		#region AbstractPropertyDescriptor

		public override Type ComponentType
		{
			get { return eventDescriptor.ComponentType; }
		}

		public override object GetValue(object component)
		{
			return handler.Target;
		}

		public override Type PropertyType
		{
			get { return typeof (string); }
		}

		#endregion

		#region IRealValueHolder Members

		public object RealValue
		{
			get { return handler.Target; }
		}

		#endregion
	}
}
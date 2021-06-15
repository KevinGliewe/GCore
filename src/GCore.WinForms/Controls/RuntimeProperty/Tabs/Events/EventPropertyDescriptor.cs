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
using System.Reflection;
using GCore.WinForms.Controls.RuntimeProperty.Utils;

namespace GCore.WinForms.Controls.RuntimeProperty.Tabs.Events
{
	/// <summary>
	/// EventPropertyDescriptor 
	/// </summary>
	public class EventPropertyDescriptor : AbstractPropertyDescriptor
	{
		#region EnumHelperEnum

		public enum EnumHelperEnum
		{
			RefreshListeners,
			//AddListener,
			//RemoveListener
		}

		#endregion

		private object component;
		private EventInfo eventInfo;
		private EventHandlerList eventHandlerList;
		private EventInfoConverter converter;

		public EventPropertyDescriptor(object component, EventInfo eventInfo, EventHandlerList eventHandlerList)
			: base(eventInfo.Name)
		{
			this.component = component;
			this.eventInfo = eventInfo;
			this.eventHandlerList = eventHandlerList;

			this.converter = new EventInfoConverter(this);
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

			object[] allEventAttributes = eventInfo.GetCustomAttributes(false);
			
			foreach ( Attribute attr in allEventAttributes)
			{
				attributeList.Add(attr);
			}
		}


		public override void SetValue(object component, object value)
		{
			this.converter.ReadListeners();
		}

		public override object GetValue(object component)
		{
			return this.converter;
		}

		public override TypeConverter Converter
		{
			get { return this.converter; }
		}

		public override Type PropertyType
		{
			get
			{
				return typeof (EnumHelperEnum);
			}
		}

		#region Properies

		public object Component
		{
			get { return component; }
		}

		public EventInfo EventInfo
		{
			get { return eventInfo; }
		}

		public EventHandlerList EventHandlerList
		{
			get { return eventHandlerList; }
		}

		#endregion

		public override AttributeCollection Attributes
		{
			get
			{
				AttributeCollection attr = base.Attributes;
				return attr;
			}
		}

	}
}
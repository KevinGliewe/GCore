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
using System.Diagnostics;
using System.Windows.Forms;

namespace GCore.WinForms.Controls.RuntimeProperty.Hotkey
{
	/// <summary>
	/// Summary description for HotKeyWatch.
	/// </summary>
	internal class HotKeyWatch : Control
	{
		public event EventHandler HotKeyPressed;

		private const int WM_HOTKEY = 0x312;
		private int hotKeyValue = 0;

		public HotKeyWatch()
		{
		}

		public bool RegisterHotKey(string hotKey)
		{
			if (!this.IsHandleCreated)
				base.CreateControl();
			return Register(hotKey);
		}

		public void UnregisterKey()
		{
			Unregister();
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_HOTKEY)
			{
				// fire up
				if (HotKeyPressed != null)
					HotKeyPressed(this, EventArgs.Empty);
				return;
			}
			base.WndProc(ref m);
		}

		private void Unregister()
		{
			if (hotKeyValue != 0)
			{
				HotKeyUtils.UnregisterKey(this, hotKeyValue);
				HotKeyUtils.GlobalDeleteAtom(hotKeyValue);
			}

		}

		private bool Register(string key)
		{
			Unregister();

			int hotKeyValue = HotKeyUtils.GlobalAddAtom("RE:" + key.ToString());
			if (hotKeyValue == 0)
			{
				Trace.WriteLine("Could not register atom for hotkey!");
				return false;
			}
			return HotKeyUtils.RegisterKey(this, hotKeyValue, key);
		}
	}
}
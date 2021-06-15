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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GCore.WinForms.Controls.RuntimeProperty.Hotkey
{
	internal sealed class HotKeyUtils
	{
		// HotKey
		[Flags()]
		private enum FsKeyModifiers
		{
			None = 0x0000,
			Alt = 0x0001,
			Control = 0x0002,
			Shift = 0x0004,
			Windows = 0x0008
		}

		[DllImport("user32.dll", SetLastError=true)]
		private static extern bool RegisterHotKey(
			IntPtr hWnd,
			int keyId,
			FsKeyModifiers fsModifiers,
			Keys vk
			);

		[DllImport("user32.dll", SetLastError=true)]
		private static extern bool UnregisterHotKey(
			IntPtr hWnd,
			int id
			);

		[DllImport("kernel32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		internal static extern int GlobalAddAtom(string lpString);

		[DllImport("kernel32.dll", SetLastError=true, ExactSpelling=true)]
		internal static extern ushort GlobalDeleteAtom(int nAtom);

		private HotKeyUtils()
		{
		}

		private static FsKeyModifiers CheckModifier(Keys key, Keys keyModifier, FsKeyModifiers fsModifier)
		{
			if ((key & keyModifier) == keyModifier)
				return fsModifier;
			else
				return FsKeyModifiers.None;
		}

		public static bool RegisterKey(Control parentForm, int id, Keys key)
		{
			FsKeyModifiers modifiers = FsKeyModifiers.None;

			modifiers |= CheckModifier(key, Keys.Control, FsKeyModifiers.Control);
			modifiers |= CheckModifier(key, Keys.Alt, FsKeyModifiers.Alt);
			modifiers |= CheckModifier(key, Keys.Shift, FsKeyModifiers.Shift);
			modifiers |= CheckModifier(key, Keys.LWin, FsKeyModifiers.Windows);
			modifiers |= CheckModifier(key, Keys.RWin, FsKeyModifiers.Windows);

			return RegisterHotKey(parentForm.Handle, id, modifiers, (key & (~Keys.Modifiers)));
		}

		//the keys must be separated BY  + EX: Control + Shift + L
		//if keys is empty the shorcut it's Control + Shift + X
		public static bool RegisterKey(Control parentForm, int hotKeyId, string keys)
		{
			if (keys == null)
			{
				keys = "";
			}

			Keys param = Keys.None;
			try
			{
				if (keys.Trim().Length == 0)
				{
					Trace.WriteLine("keys is empty!");
				}
				else
				{
					string[] keyArray = keys.Split('+');
					bool first = true;

					foreach (string key in keyArray)
					{
						if (first)
						{
							param = (Keys) Enum.Parse(typeof (Keys), key);
							first = false;
						}
						else
						{
							param |= (Keys) Enum.Parse(typeof (Keys), key);
						}

					}
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.Message);
			}

			if (param == Keys.None)
			{
				return false;
			}
			else
			{
				return RegisterKey(parentForm, hotKeyId, param);
			}
		}

		public static bool UnregisterKey(Control parentForm, int hotKeyId)
		{
			return UnregisterHotKey(parentForm.Handle, hotKeyId);
		}
	}
}
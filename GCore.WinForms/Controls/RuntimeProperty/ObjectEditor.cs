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
using GCore.WinForms.Controls.RuntimeProperty.Hotkey;

namespace GCore.WinForms.Controls.RuntimeProperty
{
	/// <summary>
	/// Singleton class that takes care of showing the Runtime ObjectEditor form with it's window finder.
	/// To use this you have to enable it:
	/// <code>RuntimeObjectEditor.ObjectEditor.Instance.Enable();</code>
	/// The default shortcut key used it "Control+Shift+R".
	/// If you want to use a different shortcut, change the <see cref="ObjectEditor.HotKey"/>
	/// </summary>
	public sealed class ObjectEditor
	{
		#region Instance

		private static ObjectEditor instance = new ObjectEditor();

		/// <summary>
		/// Singleton instance of the ObjectEditor.
		/// </summary>
		public static ObjectEditor Instance
		{
			get { return instance; }
		}

		#endregion

		private bool enabled = false;
		private string hotKey = "Control+Shift+R";
		private HotKeyWatch hotKeyWatch = null;
		private RuntimeEditor runtimeEditor;

		private ObjectEditor()
		{
		}

		/// <summary>
		/// Gets or sets the default shortcut used to popup the Object Editor.
		/// Default value is: Control+Shift+R.
		/// You can specify any key combination like: Control+Shift+Alt+F1.
		/// </summary>
		public string HotKey
		{
			get { return hotKey; }
			set { hotKey = value; }
		}

		/// <summary>
		/// Enable the Object Editor to listen for the shortcut key.
		/// </summary>
		/// <returns></returns>
		public bool Enable()
		{
			if (enabled)
				return true; // already enabled.

			hotKeyWatch = new HotKeyWatch();
			if (!hotKeyWatch.RegisterHotKey(hotKey))
				return false; // didn't work

			hotKeyWatch.HotKeyPressed += new EventHandler(hotKeyWatch_HotKeyPressed);
			enabled = true;
			Trace.WriteLine("ObjectEditor enabled on shorcut:" + hotKey);
			return true;
		}

		/// <summary>
		/// Disable the object editor.
		/// </summary>
		public void Disable()
		{
			if (!enabled)
				return;

			hotKeyWatch.HotKeyPressed -= new EventHandler(hotKeyWatch_HotKeyPressed);
			hotKeyWatch.UnregisterKey();
			hotKeyWatch = null;
			enabled = false;
			Trace.WriteLine("ObjectEditor disabled.");
		}

		private void hotKeyWatch_HotKeyPressed(object sender, EventArgs e)
		{
			Show();
		}

		private void runtimeEditor_Closed(object sender, EventArgs e)
		{
			runtimeEditor = null;
		}

		/// <summary>
		/// Show the object editor form.
		/// </summary>
		public void Show()
		{
			object activeSelectedObject = null;
			if (runtimeEditor != null)
			{
				activeSelectedObject = runtimeEditor.SelectedObject;
				runtimeEditor.Close();
			}

			runtimeEditor = new RuntimeEditor();
			runtimeEditor.Show();
			runtimeEditor.Closed += new EventHandler(runtimeEditor_Closed);
			runtimeEditor.SelectedObject = activeSelectedObject;
		}

		/// <summary>
		/// Show the editor with the selectedObject selected.
		/// </summary>
		/// <param name="selectObject">The object to be selected in the editor</param>
		public void Show(object selectObject)
		{
			if (runtimeEditor != null)
			{
				runtimeEditor.Close();
			}

			runtimeEditor = new RuntimeEditor();
			runtimeEditor.Show();
			runtimeEditor.Closed += new EventHandler(runtimeEditor_Closed);
			runtimeEditor.SelectedObject = selectObject;
		}

		internal RuntimeEditor ActiveEditor
		{
			get { return this.runtimeEditor; }
			set { this.runtimeEditor = value; }
		}

		public object SelectedObject
		{
			get { return runtimeEditor.SelectedObject; }
			set { runtimeEditor.SelectedObject = value; }
		}

		public Form CreateEditor()
		{
			if (runtimeEditor != null)
			{
				runtimeEditor.Close();
			}
			runtimeEditor = new RuntimeEditor();
			return runtimeEditor;
		}
	}
}
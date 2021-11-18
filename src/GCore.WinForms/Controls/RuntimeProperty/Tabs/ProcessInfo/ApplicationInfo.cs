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
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace GCore.WinForms.Controls.RuntimeProperty.Tabs.ProcessInfo
{
	/// <summary>
	/// ApplicationInfo - wrapper for the Application object.
	/// </summary>
	public class ApplicationInfo
	{
		private GCDetails gcInfo = new GCDetails();

		public ApplicationInfo()
		{
		}

		#region Process

		[Category("Process")]
		public Process CurrentProcess
		{
			get { return Process.GetCurrentProcess(); }
		}

		//[Category("Process")]
		//public string CurrentProcessName
		//{
		//    get { return CurrentProcess.ProcessName; }
		//}

		#endregion

		#region Thread

        [Category("Thread")]
		public IPrincipal CurrentPrincipal
		{
			get { return Thread.CurrentPrincipal; }
		}

		[Category("Thread")]
		public Thread CurrentThread
		{
			get { return Thread.CurrentThread; }
		}

		//[Category("Thread")]
		//public int CurrentThreadId
		//{
		//    get { return Thread.CurrentThread.ManagedThreadId; }
		//}
		//[Category("Thread")]
		//public string CurrentThreadName
		//{
		//    get { return Thread.CurrentThread.Name; }
		//}

		#endregion

		#region Domain

		[Category("Domain")]
		public AppDomain CurrentDomain
		{
			get { return AppDomain.CurrentDomain; }
		}

		#endregion

		#region Application

		[Category("Application")]
		public object CurrentApplication
		{
			get { return Application.ProductName; }
		}

		#endregion

		#region GC

		public GCDetails GCInfo
		{
			get { return this.gcInfo; }
		}

		#endregion

		public class GCDetails
		{
			public GCDetails()
			{
			}

			[Description("Returns the number of times garbage collection has occurred for the generation 0 of objects")]
			[RefreshProperties(RefreshProperties.All)]
			public int CollectionCount_0
			{
				get
				{
#if NET2
					return GC.CollectionCount(0);
#else
					return 0;
#endif
				}
			}

			[Description("Returns the number of times garbage collection has occurred for the generation 1 of objects")]
			[RefreshProperties(RefreshProperties.All)]
			public int CollectionCount_1
			{
				get
				{
#if NET2
					return GC.CollectionCount(1);
#else
					return 0;
#endif
				}
			}

			[Description("Returns the number of times garbage collection has occurred for the generation 2 of objects")]
			[RefreshProperties(RefreshProperties.All)]
			public int CollectionCount_2
			{
				get
				{
#if NET2
					return GC.CollectionCount(2);
#else
					return 0;
#endif
				}
			}

			[Description("Retrieves the number of bytes currently thought to be allocated (after a collect)")]
			[RefreshProperties(RefreshProperties.All)]
			public string TotalMemoryKb
			{
				get { return Format(GC.GetTotalMemory(true)/1024); }
			}

			[Description("Gets the maximum allowable working set size for the associated process.")]
			[RefreshProperties(RefreshProperties.All)]
			public int MaxWorkingSetKb
			{
				get { return Process.GetCurrentProcess().MaxWorkingSet.ToInt32()/1024; }
				set { Process.GetCurrentProcess().MaxWorkingSet = new IntPtr(value*1024); }
			}

			[Description("Gets or Sets the Minimum allowable working set size for the associated process.")]
			[RefreshProperties(RefreshProperties.All)]
			public int MinWorkingSetKb
			{
				get { return Process.GetCurrentProcess().MinWorkingSet.ToInt32()/1024; }
				set { Process.GetCurrentProcess().MinWorkingSet = new IntPtr(value*1024); }
			}

			[Description("Gets the amount of physical memory allocated for the associated process.")]
			public string WorkingSetKb
			{
				get
				{
#if NET2
					return Format(Process.GetCurrentProcess().WorkingSet64/1024);
#else
					return Format(Process.GetCurrentProcess().WorkingSet/1024);
#endif
				}
			}

			public override string ToString()
			{
				return "{Mem:" + TotalMemoryKb + " Max:" + WorkingSetKb + "}";
			}

			private string Format(long value)
			{
				return value.ToString("###,##0");
			}
		}
	}
}
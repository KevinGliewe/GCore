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


using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace GCore.WinForms.Controls.RuntimeProperty.Utils.Generate
{
	/// <summary>
	/// Simple Generator for a class
	/// </summary>
	public class ClassGenerator
	{
		/*private Assembly generatedAssembly;
		private AssemblyName assemblyName;
		private AssemblyBuilder assemblyBuilder;
*/

		public ClassGenerator()
		{
		}

		public void Generate()
		{
		}

		#region Assembly

		private void GenerateAssembly()
		{
/*			if (generatedAssembly != null)
			{
				return;
			}

			assemblyName = new AssemblyName();
			assemblyName.Name = "James";

			assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
*/
		}

		#endregion
	}
}
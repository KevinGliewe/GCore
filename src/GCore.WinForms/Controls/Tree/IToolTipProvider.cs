using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.WinForms.Controls.Tree
{
	public interface IToolTipProvider
	{
		string GetToolTip(TreeNodeAdv node);
	}
}

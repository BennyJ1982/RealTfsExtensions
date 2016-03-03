//------------------------------------------------------------------------------
// <copyright file="PendingChangesWindow.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace RealTfsExtensions.Windows
{
	using System.Runtime.InteropServices;
	using Microsoft.VisualStudio.Shell;

	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	/// </summary>
	/// <remarks>
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
	/// usually implemented by the package implementer.
	/// <para>
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its
	/// implementation of the IVsUIElementPane interface.
	/// </para>
	/// </remarks>
	[Guid("25b34c3d-a758-4dbe-95cf-fe885e953c27")]
	public class PendingChangesWindow : ToolWindowPane
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PendingChangesWindow"/> class.
		/// </summary>
		public PendingChangesWindow() : base(null)
		{
			this.Caption = "Pending Changes";

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
			// the object returned by the Content property.
			this.Content = new PendingChangesWindowControl();
		}

		public object InstanceIdentifier { get; private set; }

		public void PrepareWindow(object instanceIdentifier, object dataContext)
		{
			this.InstanceIdentifier = instanceIdentifier;
			this.Caption = "Pending Changes - " + instanceIdentifier.ToString();
			((PendingChangesWindowControl)this.Content).DataContext = dataContext;
		}
	}
}

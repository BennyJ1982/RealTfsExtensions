//------------------------------------------------------------------------------
// <copyright file="PendingChangesWindowCommand.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace RealTfsExtensions.Windows
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class PendingChangesWindowCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 258;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("735cdfa7-eb0c-4a07-8d6f-b8ac20df0489");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package package;

		/// <summary>
		/// Initializes a new instance of the <see cref="PendingChangesWindowCommand"/> class.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private PendingChangesWindowCommand(Package package)
		{
			if (package == null)
			{
				throw new ArgumentNullException("package");
			}

			this.package = package;
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static PendingChangesWindowCommand Instance
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package)
		{
			Instance = new PendingChangesWindowCommand(package);
		}

		/// <summary>
		/// Shows the tool window for the specified instanceIdentifer and assigns the specified data context.
		/// </summary>
		public void ShowToolWindow(object instanceIdentifier, object dataContext)
		{
			for (int i = 0; i < 50; i++)
			{
				var window = this.package.FindToolWindow(typeof(PendingChangesWindow), i, false) as PendingChangesWindow;
				if (window == null || window.InstanceIdentifier.Equals(instanceIdentifier))
				{
					if (window == null)
					{
						// Create the window with the first free ID. 
						window = this.package.FindToolWindow(typeof(PendingChangesWindow), i, true) as PendingChangesWindow;
						if ((null == window) || (null == window.Frame))
						{
							throw new NotSupportedException("Cannot create tool window");
						}
					}

					var windowFrame = (IVsWindowFrame)window.Frame;
					Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
					window.PrepareWindow(instanceIdentifier, dataContext);
					break;
				}
			}
		}
	}
}

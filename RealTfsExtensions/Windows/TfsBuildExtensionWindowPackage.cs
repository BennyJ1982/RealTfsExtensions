//------------------------------------------------------------------------------
// <copyright file="TfsBuildExtensionWindowPackage.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace RealTfsExtensions.Windows
{
	using System;
	using System.Windows;
	using Microsoft.VisualStudio.TeamFoundation;
	using RealTfsExtensions.Shared.Messages;

	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the
	/// IVsPackage interface and uses the registration attributes defined in the framework to
	/// register itself and its components with the shell. These attributes tell the pkgdef creation
	/// utility what data to put into .pkgdef file.
	/// </para>
	/// <para>
	/// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
	/// </para>
	/// </remarks>
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideToolWindow(typeof(TfsBuildExtensionWindow))]
	[ProvideToolWindow(typeof(UsersWindow))]
	[ProvideToolWindow(typeof(PendingChangesWindow), MultiInstances=true, DocumentLikeTool = true)]
	[Guid(TfsBuildExtensionWindowPackage.PackageGuidString)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	public sealed class TfsBuildExtensionWindowPackage : Package
	{
		private TeamFoundationServerExt teamFoundationServer;

		/// <summary>
		/// TfsBuildExtensionWindowPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "ac203ec0-bd27-4a6e-abc4-f0dd88ad2764";

		/// <summary>
		/// Initializes a new instance of the <see cref="TfsBuildExtensionWindow"/> class.
		/// </summary>
		public TfsBuildExtensionWindowPackage()
		{
			// Inside this method you can place any initialization code that does not require
			// any Visual Studio service because at this point the package object is created but
			// not sited yet inside Visual Studio environment. The place to do all the other
			// initialization is the Initialize method.
		}

		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			TfsBuildExtensionWindowCommand.Initialize(this);
			UsersWindowCommand.Initialize(this);
			PendingChangesWindowCommand.Initialize(this);
			base.Initialize();

			AppContext.ServiceProvider.Messenger.Register<OpenPendingChangesViewMessage>(this, HandleOpenPendingChangesViewMessage);
			this.WireUpTeamFoundationContext();
		}

		protected override void Dispose(bool disposing)
		{
			AppContext.ServiceProvider.Messenger.Unregister(this);
			this.teamFoundationServer.ProjectContextChanged -= this.OnProjectContextChanged;
			this.teamFoundationServer = null;

			base.Dispose(disposing);
		}

		#endregion

		private void WireUpTeamFoundationContext()
		{
			var dte = (EnvDTE.DTE)GetGlobalService(typeof(EnvDTE.DTE));
			this.teamFoundationServer = (TeamFoundationServerExt)dte.GetObject("Microsoft.VisualStudio.TeamFoundation.TeamFoundationServerExt");
			this.teamFoundationServer.ProjectContextChanged += this.OnProjectContextChanged;
		}

		private void OnProjectContextChanged(object sender, System.EventArgs e)
		{
			var tfsUri = this.teamFoundationServer.ActiveProjectContext.DomainUri;
			var tfsProject = this.teamFoundationServer.ActiveProjectContext.ProjectName;
			AppContext.ServiceProvider.TfsContext.SetContext(tfsUri == null ? null : new Uri(tfsUri), tfsProject);
		}

		private static void HandleOpenPendingChangesViewMessage(OpenPendingChangesViewMessage message)
		{
			// open new pending changes windows whenever we receive the respective message
			PendingChangesWindowCommand.Instance.ShowToolWindow(message.User.QualifiedAccount, message.ViewModelToUse.Value);
		}
	}
}

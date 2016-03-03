namespace RealTfsExtensions.Shared.Services
{
	using System.ComponentModel;
	using System.Windows;
	using GalaSoft.MvvmLight.Messaging;
	using RealTfsExtensions.Shared.Services.Abstract;
	using RealTfsExtensions.Shared.Services.Abstract.Build;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.Services.Concrete;
	using RealTfsExtensions.Shared.Services.Concrete.Build;
	using RealTfsExtensions.Shared.Services.Concrete.VersionControl;
	using RealTfsExtensions.Shared.ViewModels;
	using RealTfsExtensions.Shared.ViewModels.Commands;

	public class ServiceProvider : IServiceProvider
	{
		private TfsContext tfsContext;

		private IProjectCollectionService projectCollectionService;

		private IBuildDefinitionService buildDefinitionService;

		private IProjectUserService projectUserService;

		private IShelvesetService shelvesetService;

		private IPendingChangesService pendingChangesService;

		private ViewModelLocator viewModelLocator;

		private CommandFactory commandFactory;

		public TfsContext TfsContext => this.tfsContext ?? (this.tfsContext = new TfsContext());

		public IProjectCollectionService ProjectCollectionService
			=> this.projectCollectionService ?? (this.projectCollectionService = new ProjectCollectionService(this.TfsContext));


		public IBuildDefinitionService BuildDefinitionService
			=>
				this.buildDefinitionService
				?? (this.buildDefinitionService = new BuildDefinitionService(this.ProjectCollectionService, this.TfsContext));

		public IProjectUserService ProjectUserService
			=> this.projectUserService ?? (this.projectUserService = new ProjectUserService(this.ProjectCollectionService));

		public IShelvesetService ShelvesetService
			=> this.shelvesetService ?? (this.shelvesetService = new ShelvesetService(this.ProjectCollectionService));

		public IPendingChangesService PendingChangesService
			=> this.pendingChangesService ?? (this.pendingChangesService = new PendingChangesService(this.ProjectCollectionService));

		public ViewModelLocator ViewModelLocator => this.viewModelLocator ?? (this.viewModelLocator = new ViewModelLocator(this));

		public IMessenger Messenger { get; } = new Messenger();

		public CommandFactory CommandFactory
			=>
				this.commandFactory
				?? (this.commandFactory =
					new CommandFactory(this.ProjectCollectionService, this.PendingChangesService, this.Messenger, this.ViewModelLocator));

		#region Singleton instance

		private static IServiceProvider instance;

		public static IServiceProvider Instance
			=> instance ?? (instance = (DesignerProperties.GetIsInDesignMode(new DependencyObject()) ? 
				(IServiceProvider)new DesignServiceProvider() : new ServiceProvider()));

		#endregion
	}
}

namespace RealTfsExtensions.Shared.Services
{
	using GalaSoft.MvvmLight.Messaging;
	using RealTfsExtensions.Shared.Services.Abstract;
	using RealTfsExtensions.Shared.Services.Abstract.Build;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.Services.DesignServices;
	using RealTfsExtensions.Shared.ViewModels;
	using RealTfsExtensions.Shared.ViewModels.Commands;

	public class DesignServiceProvider : IServiceProvider
	{
		public DesignServiceProvider()
		{
			this.TfsContext = new TfsContext();
			this.ProjectCollectionService = null;
			this.BuildDefinitionService = new DesignBuildDefinitionService();
			this.ProjectUserService = new DesignProjectUserService();
			this.ShelvesetService = new DesignShelvesetService();
			this.PendingChangesService = new DesignPendingChangesSevice();
			this.ViewModelLocator = new ViewModelLocator(this);
			this.Messenger = new Messenger();
			this.CommandFactory = new CommandFactory(
				this.ProjectCollectionService,
				this.PendingChangesService,
				this.Messenger,
				this.ViewModelLocator);
		}

		public TfsContext TfsContext { get; }

		public IProjectCollectionService ProjectCollectionService { get; }

		public IBuildDefinitionService BuildDefinitionService { get; }

		public IProjectUserService ProjectUserService { get; }

		public IShelvesetService ShelvesetService { get; }

		public IPendingChangesService PendingChangesService { get; }

		public ViewModelLocator ViewModelLocator { get; }

		public IMessenger Messenger { get; }

		public CommandFactory CommandFactory { get; }
	}
}

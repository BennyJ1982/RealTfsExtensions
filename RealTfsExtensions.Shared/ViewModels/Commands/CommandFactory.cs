namespace RealTfsExtensions.Shared.ViewModels.Commands
{
	using System.Windows.Input;
	using GalaSoft.MvvmLight.Messaging;
	using RealTfsExtensions.Shared.Services.Abstract;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.ViewModels.Commands.Sources;

	public class CommandFactory
	{
		private readonly IProjectCollectionService collectionService;

		private readonly IPendingChangesService pendingChangesService;

		private readonly IMessenger messenger;

		private readonly ViewModelLocator locator;

		public CommandFactory(IProjectCollectionService collectionService, IPendingChangesService pendingChangesService, IMessenger messenger, ViewModelLocator locator)
		{
			this.collectionService = collectionService;
			this.pendingChangesService = pendingChangesService;
			this.messenger = messenger;
			this.locator = locator;
		}

		public ICommand CreateQueueNewBuildCommand(IQueueNewBuildCommandSource commandSource)
		{
			return new QueueNewBuildCommand(commandSource, this.collectionService);
		}

		public ICommand CreateViewPendingChangesCommand(IViewPendingChangesCommandSource commandSource)
		{
			return new ViewPendingChangesCommand(this.locator, this.messenger, commandSource);
		}

		public ICommand CreateRemoveLocksCommand(IRemoveLocksCommandSource commandSource)
		{
			return new RemoveLocksCommand(commandSource, this.pendingChangesService);
		}
	}
}

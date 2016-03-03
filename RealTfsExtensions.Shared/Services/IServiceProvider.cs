namespace RealTfsExtensions.Shared.Services
{
	using GalaSoft.MvvmLight.Messaging;
	using RealTfsExtensions.Shared.Services.Abstract;
	using RealTfsExtensions.Shared.Services.Abstract.Build;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.ViewModels;
	using RealTfsExtensions.Shared.ViewModels.Commands;

	public interface IServiceProvider
	{
		TfsContext TfsContext { get; }

		IProjectCollectionService ProjectCollectionService { get; }

		IBuildDefinitionService BuildDefinitionService { get; }

		IProjectUserService ProjectUserService { get; }

		IShelvesetService ShelvesetService { get; }

		IPendingChangesService PendingChangesService { get; }

		ViewModelLocator ViewModelLocator { get; }

		IMessenger Messenger { get; }

		CommandFactory CommandFactory { get; }
	}
}

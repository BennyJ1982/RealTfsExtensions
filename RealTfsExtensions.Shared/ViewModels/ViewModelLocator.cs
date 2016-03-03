namespace RealTfsExtensions.Shared.ViewModels
{
	using GalaSoft.MvvmLight;
	using RealTfsExtensions.Shared.Models;
	using Services = RealTfsExtensions.Shared.Services;

	public class ViewModelLocator : ObservableObject
	{
		private readonly Services.IServiceProvider serviceProvider;
		
		private BuildViewModel buildViewModel;

		private UsersViewModel usersViewModel;

		public ViewModelLocator(Services.IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Returns the BuildViewModel (only one instance per Locator)
		/// </summary>
		public BuildViewModel BuildViewModel
			=>
				this.buildViewModel
				?? (this.buildViewModel =
					new BuildViewModel(
						this.serviceProvider.Messenger,
						this.serviceProvider.BuildDefinitionService,
						this.serviceProvider.ProjectUserService,
						this.serviceProvider.CommandFactory,
						this.serviceProvider.ShelvesetService));

		/// <summary>
		/// Returns the UsersViewModel (only one instance per Locator)
		/// </summary>
		public UsersViewModel UsersViewModel
			=>
				this.usersViewModel
				?? (this.usersViewModel =
					new UsersViewModel(this.serviceProvider.Messenger, this.serviceProvider.ProjectUserService, this.serviceProvider.CommandFactory));

		/// <summary>
		/// Returns a new PendingChangesViewModel (each call returns a new instance).
		/// </summary>
		public PendingChangesViewModel CreatePendingChangesViewModel(IIdentity user)
			=>
				new PendingChangesViewModel(
					this.serviceProvider.Messenger,
					this.serviceProvider.PendingChangesService,
					this.serviceProvider.CommandFactory,
					user);

	}
}

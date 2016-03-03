namespace RealTfsExtensions.Shared.ViewModels
{
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using GalaSoft.MvvmLight.Messaging;
	using Microsoft.TeamFoundation.Build.Client;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services;
	using RealTfsExtensions.Shared.Services.Abstract.Build;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.ViewModels.Commands;
	using RealTfsExtensions.Shared.ViewModels.Commands.Sources;

	public class BuildViewModel : ViewModelBase, IQueueNewBuildCommandSource
	{
		private readonly IBuildDefinitionService buildDefinitionService;

		private readonly IProjectUserService userService;

		private readonly CommandFactory commandFactory;

		private readonly IShelvesetService shelvesetService;

		public BuildViewModel(
			IMessenger messenger,
			IBuildDefinitionService buildDefinitionService,
			IProjectUserService userService,
			CommandFactory commandFactory,
			IShelvesetService shelvesetService)
			: base(messenger)
		{
			this.buildDefinitionService = buildDefinitionService;
			this.userService = userService;
			this.commandFactory = commandFactory;
			this.shelvesetService = shelvesetService;

			this.InitCommands();
			this.InitServices();

			this.QueueAndWait(this.LoadLookupData);
		}

		#region Commands

		public ICommand QueueBuild { get; private set; }

		public SimpleCommand RefreshOwnedShelvesets { get; private set; }

		private void InitCommands()
		{
			this.QueueBuild = this.commandFactory.CreateQueueNewBuildCommand(this);
			this.RefreshOwnedShelvesets = new SimpleCommand(this.UpdateOwnedShelvesets, () => !this.IsLoading && this.SelectedOwner != null);

			this.AddActionToRunWhenIsLoadingChanged(this.RefreshOwnedShelvesets.RaiseCanExecuteChanged);
		}

		#endregion

		#region Properties


		private ObservableCollection<IBuildDefinition> buildDefinitions;
		public ObservableCollection<IBuildDefinition> BuildDefinitions
		{
			get
			{
				return this.buildDefinitions;
			}
			private set
			{
				this.buildDefinitions = value;
				this.RaisePropertyChanged(() => this.BuildDefinitions);
				this.SelectedBuildDefinition = this.buildDefinitions.FirstOrDefault();
			}
		}

		private ObservableCollection<IIdentity> users;
		public ObservableCollection<IIdentity> Users
		{
			get
			{

				return this.users;
			}
			private set
			{
				this.users = value;
				this.RaisePropertyChanged(() => this.Users);
				this.SelectedOwner = this.Users.GetCurrentIdentity() ?? this.Users.FirstOrDefault();
			}
		}

		private ObservableCollection<Shelveset> ownedShelvesets;
		public ObservableCollection<Shelveset> OwnedShelvesets
		{
			get
			{

				return this.ownedShelvesets;
			}
			private set
			{
				this.ownedShelvesets = value;
				this.RaisePropertyChanged(() => this.OwnedShelvesets);
				this.SelectedShelveset = this.OwnedShelvesets.FirstOrDefault();
			}
		}

		private IIdentity selectedOwner;
		public IIdentity SelectedOwner
		{
			get
			{

				return this.selectedOwner;
			}
			set
			{
				this.selectedOwner = value;
				this.RaisePropertyChanged(() => this.SelectedOwner);
				this.UpdateOwnedShelvesets();
			}
		}

		private IIdentity selectedUserToCheckInFor;
		public IIdentity SelectedUserToCheckInFor
		{
			get
			{

				return this.selectedUserToCheckInFor;
			}
			set
			{
				this.selectedUserToCheckInFor = value;
				this.RaisePropertyChanged(() => this.SelectedUserToCheckInFor);
			}
		}

		private Shelveset selectedShelveset;
		public Shelveset SelectedShelveset
		{
			get
			{

				return this.selectedShelveset;
			}
			set
			{
				this.selectedShelveset = value;
				this.RaisePropertyChanged(() => this.SelectedShelveset);
			}
		}

		private IBuildDefinition selectedBuildDefinition;
		public IBuildDefinition SelectedBuildDefinition
		{
			get
			{

				return this.selectedBuildDefinition;
			}
			set
			{
				this.selectedBuildDefinition = value;
				this.RaisePropertyChanged(() => this.SelectedBuildDefinition);
			}
		}

		#endregion

		#region UI logic       

		private async void UpdateOwnedShelvesets()
		{
			this.SelectedShelveset = null;
			if (this.SelectedOwner == null)
			{
				this.OwnedShelvesets = new ObservableCollection<Shelveset>();
			}
			else
			{
				await this.QueueAndWait(this.LoadOwnedShelvesets);
			}
		}

		#endregion

		#region Services

		private void InitServices()
		{
			this.buildDefinitionService.DependenciesHaveChanged += async (s, e) => await this.RunOnce(() => this.LoadLookupData());
			this.userService.DependenciesHaveChanged += async (s, e) => await this.RunOnce(() => this.LoadLookupData());
		}

		private async Task LoadLookupData()
		{
			var loadBuildDefinitions =
				this.buildDefinitionService.LoadAsync().ContinueWith(d => this.BuildDefinitions = new ObservableCollection<IBuildDefinition>(d.Result));
			var loadUsers = this.userService.LoadAsync().ContinueWith(u => this.Users = new ObservableCollection<IIdentity>(u.Result));
			await Task.WhenAll(loadBuildDefinitions, loadUsers);
		}

		private async Task LoadOwnedShelvesets()
		{
			await this.shelvesetService.GetShelvesetsByUserAsync(this.SelectedOwner)
				.ContinueWith(d => this.OwnedShelvesets = new ObservableCollection<Shelveset>(d.Result));
		}

		#endregion
	}
}

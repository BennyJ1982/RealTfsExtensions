namespace RealTfsExtensions.Shared.ViewModels
{
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using GalaSoft.MvvmLight.Messaging;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.ViewModels.Commands;
	using RealTfsExtensions.Shared.ViewModels.Commands.Sources;

	public class UsersViewModel : ViewModelBase, IViewPendingChangesCommandSource
	{
		private readonly IProjectUserService userService;

		public UsersViewModel(IMessenger messenger, IProjectUserService userService, CommandFactory commandFactory)
			: base(messenger)
		{
			this.userService = userService;
			userService.DependenciesHaveChanged += async (s, e) => await this.QueueAndWait(this.LoadLookupData);

			this.ViewPendingChanges = commandFactory.CreateViewPendingChangesCommand(this);

			this.QueueAndWait(this.LoadLookupData);
		}

		#region Commands

		public ICommand ViewPendingChanges { get; }

		#endregion

		#region Properties

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
				this.SelectedUser = this.Users.FirstOrDefault();
			}
		}

		private IIdentity selectedUser;
		public IIdentity SelectedUser
		{
			get
			{

				return this.selectedUser;
			}
			private set
			{
				this.selectedUser = value;
				this.RaisePropertyChanged(() => this.SelectedUser);
			}
		}
	
		#endregion

		private async Task LoadLookupData()
		{
			await this.userService.LoadAsync().ContinueWith(u => this.Users = new ObservableCollection<IIdentity>(u.Result));
		}
	}
}

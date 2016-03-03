namespace RealTfsExtensions.Shared.ViewModels
{
	using System.Collections.ObjectModel;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using GalaSoft.MvvmLight.Messaging;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.ViewModels.Commands;
	using RealTfsExtensions.Shared.ViewModels.Commands.Sources;

	public class PendingChangesViewModel : ViewModelBase, IRemoveLocksCommandSource
	{
		private readonly IPendingChangesService pendingChangesService;

		private readonly CommandFactory commandFactory;

		public PendingChangesViewModel(
			IMessenger messenger,
			IPendingChangesService pendingChangesService,
			CommandFactory commandFactory,
			IIdentity user)
			: base(messenger)
		{
			this.pendingChangesService = pendingChangesService;
			this.commandFactory = commandFactory;
			this.User = user;

			this.InitCommands();
			this.QueueAndWait(this.LoadPendingChanges);
			pendingChangesService.DependenciesHaveChanged += async (s, e) => await this.QueueAndWait(this.LoadPendingChanges);
		}

		#region Commands

		public SimpleCommand Refresh { get; private set; }

		public ICommand RemoveLocks { get; private set; }

		private void InitCommands()
		{
			this.RemoveLocks = this.commandFactory.CreateRemoveLocksCommand(this);
			this.Refresh = new SimpleCommand(async () => await this.QueueAndWait(this.LoadPendingChanges), () => !this.IsLoading);

			this.AddActionToRunWhenIsLoadingChanged(this.Refresh.RaiseCanExecuteChanged);
		}

		#endregion

		#region Properties

		public IIdentity User { get; }

		private ObservableCollection<IPendingChange> pendingChanges;
		public ObservableCollection<IPendingChange> PendingChanges
		{
			get
			{

				return this.pendingChanges;
			}
			private set
			{
				this.pendingChanges = value;
				this.RaisePropertyChanged(() => this.PendingChanges);
			}
		}

		#endregion

		private async Task LoadPendingChanges()
		{
			await this.pendingChangesService.GetPendingChangesByUserAsync(this.User)
				.ContinueWith(p => this.PendingChanges = new ObservableCollection<IPendingChange>(p.Result));
		}
	}
}

namespace RealTfsExtensions.Shared.ViewModels.Commands
{
	using System.ComponentModel;
	using System.Linq;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.ViewModels.Commands.Sources;

	public class RemoveLocksCommand : CommandBase
	{
		private readonly IRemoveLocksCommandSource commandSource;

		private readonly IPendingChangesService pendingChangesService;

		public RemoveLocksCommand(IRemoveLocksCommandSource commandSource, IPendingChangesService pendingChangesService)
		{
			this.commandSource = commandSource;
			this.pendingChangesService = pendingChangesService;

			this.commandSource.PropertyChanged += this.OnCommandSourcePropertyChanged;
		}

		public override bool CanExecute(object parameter)
		{
			return this.commandSource.User != null && !this.commandSource.IsLoading && this.commandSource.PendingChanges != null
					&& this.commandSource.PendingChanges.Any(p => p.IsLocked);
		}

		public override void Execute(object parameter)
		{
			this.pendingChangesService.RemoveLocks(this.commandSource.PendingChanges, this.commandSource.User);
		}

		private void OnCommandSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "User" || e.PropertyName=="IsLoading" || e.PropertyName=="PendingChanges" )
			{
				this.RaiseCanExecuteChanged();
			}
		}
	}
}

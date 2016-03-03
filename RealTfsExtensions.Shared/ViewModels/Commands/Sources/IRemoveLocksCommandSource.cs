namespace RealTfsExtensions.Shared.ViewModels.Commands.Sources
{
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using RealTfsExtensions.Shared.Models;

	public interface IRemoveLocksCommandSource : INotifyPropertyChanged
	{
		IIdentity User { get; }

		bool IsLoading { get; }

		ObservableCollection<IPendingChange> PendingChanges { get; }
	}
}

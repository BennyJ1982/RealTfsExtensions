namespace RealTfsExtensions.Shared.ViewModels.Commands.Sources
{
	using System.ComponentModel;
	using RealTfsExtensions.Shared.Models;

	public interface IViewPendingChangesCommandSource : INotifyPropertyChanged
	{
		IIdentity SelectedUser { get; }
	}
}

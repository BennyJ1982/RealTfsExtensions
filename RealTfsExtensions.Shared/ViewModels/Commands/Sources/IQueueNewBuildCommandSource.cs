namespace RealTfsExtensions.Shared.ViewModels.Commands.Sources
{
	using System.ComponentModel;
	using Microsoft.TeamFoundation.Build.Client;
	using RealTfsExtensions.Shared.Models;

	public interface IQueueNewBuildCommandSource : INotifyPropertyChanged
	{
		IIdentity SelectedOwner { get; }
		IBuildDefinition SelectedBuildDefinition { get; }
		bool IsLoading { get; }
	}
}

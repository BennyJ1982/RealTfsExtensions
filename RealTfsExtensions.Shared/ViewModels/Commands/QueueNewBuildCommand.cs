namespace RealTfsExtensions.Shared.ViewModels.Commands
{
	using System.ComponentModel;
	using System.Windows;
	using Microsoft.TeamFoundation.Build.Client;
	using RealTfsExtensions.Shared.Services.Abstract;
	using RealTfsExtensions.Shared.ViewModels.Commands.Sources;

	public class QueueNewBuildCommand : CommandBase
	{
		private readonly IQueueNewBuildCommandSource commandSource;

		private readonly IProjectCollectionService collectionService;

		public QueueNewBuildCommand(IQueueNewBuildCommandSource commandSource, IProjectCollectionService collectionService)
		{
			this.commandSource = commandSource;
			this.collectionService = collectionService;

			this.commandSource.PropertyChanged += this.OnCommandSourcePropertyChanged;
		}

		private bool isQueueing;

		public bool IsQueueing
		{
			get
			{
				return this.isQueueing;
			}
			private set
			{
				this.isQueueing = value;
				this.RaiseCanExecuteChanged();
			}
		}

		public override bool CanExecute(object parameter)
		{
			return !this.isQueueing && this.commandSource.SelectedOwner != null && this.commandSource.SelectedBuildDefinition != null
					&& !this.commandSource.IsLoading;
		}

		public override void Execute(object parameter)
		{
			this.QueueBuildAsync();
		}

		private async void QueueBuildAsync()
		{
			this.IsQueueing = true;
			try
			{
				var buildServer = (await this.collectionService.LoadAsync()).GetService<IBuildServer>();
				MessageBox.Show(buildServer.BuildServerVersion.ToString());
			}
			finally
			{
				this.IsQueueing = false;
			}
		}

		private void OnCommandSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "SelectedOwner" || e.PropertyName == "SelectedBuildDefinition" || e.PropertyName == "IsLoading")
			{
				this.RaiseCanExecuteChanged();
			}
		}
	}
}

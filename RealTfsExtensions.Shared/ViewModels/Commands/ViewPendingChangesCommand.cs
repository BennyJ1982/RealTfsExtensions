namespace RealTfsExtensions.Shared.ViewModels.Commands
{
	using System;
	using System.ComponentModel;
	using GalaSoft.MvvmLight.Messaging;
	using RealTfsExtensions.Shared.Messages;
	using RealTfsExtensions.Shared.ViewModels.Commands.Sources;

	public class ViewPendingChangesCommand : CommandBase
	{
		private readonly ViewModelLocator locator;

		private readonly IMessenger messenger;

		private readonly IViewPendingChangesCommandSource commandSource;

		public ViewPendingChangesCommand(ViewModelLocator locator, IMessenger messenger, IViewPendingChangesCommandSource commandSource)
		{
			this.locator = locator;
			this.messenger = messenger;
			this.commandSource = commandSource;

			this.commandSource.PropertyChanged += this.OnCommandSourcePropertyChanged;
		}

		public override bool CanExecute(object parameter)
		{
			return this.commandSource.SelectedUser != null;
		}

		public override void Execute(object parameter)
		{
			var user = this.commandSource.SelectedUser;
			var viewModel = new Lazy<ViewModelBase>(() => this.locator.CreatePendingChangesViewModel(user));
			this.messenger.Send(new OpenPendingChangesViewMessage(viewModel, user));
		}

		private void OnCommandSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "SelectedUser" )
			{
				this.RaiseCanExecuteChanged();
			}
		}
	}
}

namespace RealTfsExtensions.Shared.ViewModels.Commands
{
	using System;
	using System.Windows.Input;
	using System.Windows.Threading;

	public abstract class CommandBase : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public abstract bool CanExecute(object parameter);

		public abstract void Execute(object parameter);

		public virtual void RaiseCanExecuteChanged()
		{
			if (this.CanExecuteChanged != null)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => this.CanExecuteChanged(this, EventArgs.Empty)));
			}
		}
	}
}

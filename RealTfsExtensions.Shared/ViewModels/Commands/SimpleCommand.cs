namespace RealTfsExtensions.Shared.ViewModels.Commands
{
	using System;

	public class SimpleCommand : CommandBase
	{
		private readonly Action execute;

		private readonly Func<bool> canExecute;

		public SimpleCommand(Action execute, Func<bool> canExecute)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public override bool CanExecute(object parameter)
		{
			return this.canExecute();
		}

		public override void Execute(object parameter)
		{
			this.execute();
		}
	}

	public class SimpleCommand<T> : CommandBase
	{
		private readonly Action<T> execute;

		private readonly Func<T, bool> canExecute;

		public SimpleCommand(Action<T> execute, Func<T, bool> canExecute)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public override bool CanExecute(object parameter)
		{
			return this.canExecute((T) parameter);
		}

		public override void Execute(object parameter)
		{
			this.execute((T) parameter);
		}
	}
}

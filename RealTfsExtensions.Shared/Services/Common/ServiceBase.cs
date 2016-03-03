namespace RealTfsExtensions.Shared.Services.Common
{
	using System;
	using System.Threading.Tasks;
	using GalaSoft.MvvmLight;

	public abstract class ServiceBase : ObservableObject, IService
	{
		private bool isBusy;

		public bool IsBusy
		{
			get
			{
				return this.isBusy;
			}
			set
			{
				this.isBusy = value;
				this.RaisePropertyChanged(() => this.IsBusy);
			}
		}

		public event EventHandler DependenciesHaveChanged;

		protected virtual void RaiseDependenciesHaveChanged()
		{
			if (this.DependenciesHaveChanged != null)
			{
				this.DependenciesHaveChanged(this, EventArgs.Empty);
			}
		}

		protected virtual async Task<T> RunWhileBeingBusy<T>(Task<T> task)
		{
			this.IsBusy = true;
			await task;
			this.IsBusy = false;

			return await task;
		}

		protected static Task<T> RunAsTask<T>(Func<T> action)
		{
			return Task.Factory.StartNew(action);
		}
	}
}

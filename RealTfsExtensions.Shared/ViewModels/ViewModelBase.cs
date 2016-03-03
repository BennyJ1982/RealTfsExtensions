namespace RealTfsExtensions.Shared.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using GalaSoft.MvvmLight.Messaging;

	public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
	{
		private readonly Dictionary<string, Task> pendingLoads = new Dictionary<string, Task>();
		private readonly List<Action> actionsToRunWhenIsLoadingChanged = new List<Action>();

		protected ViewModelBase(IMessenger messenger)
			: base(messenger)
		{
		}

		public bool IsLoading
		{
			get
			{
				lock (this.pendingLoads)
				{
					return this.pendingLoads.Any();
				}
			}
		}

		protected internal void QueuePendingLoad(string key, Task task)
		{
			var isLoadingChanged = false;
			lock (this.pendingLoads)
			{
				this.pendingLoads.Add(key, task);
				isLoadingChanged = this.pendingLoads.Count == 1;
			}

			this.RaisePropertyChanged(() => this.IsLoading);
			if (isLoadingChanged)
			{
				this.RunActionsToRunWhenIsLoadingChanged();
			}
		}

		protected internal void DropPendingLoad(string key)
		{
			var isLoadingChanged = false;
			lock (this.pendingLoads)
			{
				this.pendingLoads.Remove(key);
				isLoadingChanged = this.pendingLoads.Count ==0;
			}

			this.RaisePropertyChanged(() => this.IsLoading);
			if (isLoadingChanged)
			{
				this.RunActionsToRunWhenIsLoadingChanged();
			}
		}

		protected internal async Task ReturnRunningTaskOrQueueAndWait(string key, Func<Task> action)
		{
			Task existing = null;
			lock (this.pendingLoads)
			{
				this.pendingLoads.TryGetValue(key, out existing);
			}

			if (existing != null)
			{
				await existing;
			}
			else
			{
				await this.QueueAndWait(key, action());
			}
		}

		protected internal async Task QueueAndWait(string key, Task task)
		{
			this.QueuePendingLoad(key, task);
			try
			{
				await task;
			}
			finally 
			{
				this.DropPendingLoad(key);
			}
		}

		protected void AddActionToRunWhenIsLoadingChanged(Action action)
		{
			this.actionsToRunWhenIsLoadingChanged.Add(action);
		}


		private void RunActionsToRunWhenIsLoadingChanged()
		{
			this.actionsToRunWhenIsLoadingChanged.ForEach(action => action());
		}
	}
}

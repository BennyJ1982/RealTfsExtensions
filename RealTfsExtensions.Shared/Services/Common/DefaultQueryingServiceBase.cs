namespace RealTfsExtensions.Shared.Services.Common
{
	using System.Threading.Tasks;

	public abstract class DefaultQueryingServiceBase<TResult> : ServiceBase, IDefaultQueryingService<TResult>
	{
		private Task<TResult> task;

		public async Task<TResult> LoadAsync()
		{
			var runningTask = this.task;
			if (this.IsBusy && runningTask != null)
			{
				return await runningTask;
			}

			this.task = runningTask = this.RunWhileBeingBusy(this.CreateDefaultQueryTask());
			return await runningTask;
		}

		protected virtual Task<TResult> CreateDefaultQueryTask()
		{
			return RunAsTask(this.InternalLoad);
		}

		protected abstract TResult InternalLoad();

		protected override void RaiseDependenciesHaveChanged()
		{
			this.task = null;
			base.RaiseDependenciesHaveChanged();
		}
	}
}

namespace RealTfsExtensions.Shared.Services.Common
{
	using System;
	using System.Threading.Tasks;

	public abstract class ContinuationServiceBase<TParentResult> : ServiceBase 
	{
		protected abstract Task<TParentResult> GetParentTask();

		protected async Task<T> ContinueWith<T>(Func<TParentResult, T> continuation)
		{
			var task= this.GetParentTask().ContinueWith(p => continuation(p.Result));
			return await this.RunWhileBeingBusy(task);
		}
	}

	public abstract class ContinuationServiceBase<TParentService, TParentResult> : ContinuationServiceBase<TParentResult>
		where TParentService : IDefaultQueryingService<TParentResult>
	{
		private readonly TParentService parentService;

		protected ContinuationServiceBase(TParentService parentService)
		{
			parentService.DependenciesHaveChanged += this.OnParentServiceDependenciesHaveChanged;
			this.parentService = parentService;
		}

		protected override Task<TParentResult> GetParentTask()
		{
			return this.parentService.LoadAsync();
		}

		protected virtual void OnParentServiceDependenciesHaveChanged(object sender, EventArgs e)
		{
			this.RaiseDependenciesHaveChanged();
		}
	}
}

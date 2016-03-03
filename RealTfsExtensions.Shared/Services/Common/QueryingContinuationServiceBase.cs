namespace RealTfsExtensions.Shared.Services.Common
{
	using System;
	using System.Threading.Tasks;

	public abstract class QueryingContinuationServiceBase<TResult, TParentResult> : DefaultQueryingServiceBase<TResult>
	{
		private TParentResult parentResult;

		protected override Task<TResult> CreateDefaultQueryTask()
		{
			return this.GetParentAndContiueWith(
				result =>
					{
						this.parentResult = result;
						try
						{
							return this.InternalLoad();
						}
						finally
						{
							this.parentResult = default(TParentResult);
						}
					});
		}

		protected async Task<TResult> GetParentAndContiueWith(Func<TParentResult, TResult> continuation)
		{
			return await this.GetParentTask().ContinueWith(p => continuation(p.Result));
		}

		protected override TResult InternalLoad()
		{
			return this.InternalLoad(this.parentResult);
		}

		protected abstract Task<TParentResult> GetParentTask();

		protected abstract TResult InternalLoad(TParentResult parentResult);

	}


	public abstract class QueryingContinuationServiceBase<TResult, TParentService, TParentResult> :
		QueryingContinuationServiceBase<TResult, TParentResult>
		where TParentService : IDefaultQueryingService<TParentResult>
	{
		private readonly TParentService parentService;

		protected QueryingContinuationServiceBase(TParentService parentService)
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

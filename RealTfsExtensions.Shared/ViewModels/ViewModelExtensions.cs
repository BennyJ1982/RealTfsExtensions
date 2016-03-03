namespace RealTfsExtensions.Shared.ViewModels
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading.Tasks;

	public static class ViewModelExtensions
	{
		/// <summary>
		/// Checks if the specified method returning a task is already queued. If so, returns the existing one, otherwise queues and runs it.
		/// </summary>
		internal static async Task RunOnce<T>(this T viewModel,Expression<Func<Task>> action) where T : ViewModelBase
		{
			var method = GetMethoInfo(action);
			var actualAction = new Func<Task>(() => (Task)method.Invoke(viewModel, null));

			await viewModel.ReturnRunningTaskOrQueueAndWait(method.Name, actualAction);
		}

		internal static async Task QueueAndWait<T>(this T viewModel, Func<Task> action) where T : ViewModelBase
		{
			await viewModel.ReturnRunningTaskOrQueueAndWait(Guid.NewGuid().ToString(), action);
		}


		public static MethodInfo GetMethoInfo(LambdaExpression expression)
		{
			var methodCallExpression = (MethodCallExpression)expression.Body;
			var methodInfo = (MethodInfo)methodCallExpression.Method;
		
			return methodInfo;
		}
	}
}

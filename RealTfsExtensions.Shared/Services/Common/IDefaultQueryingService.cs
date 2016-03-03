namespace RealTfsExtensions.Shared.Services.Common
{
	using System.Threading.Tasks;

	public interface IDefaultQueryingService<TResult> : IService
	{
		Task<TResult> LoadAsync();
	}
}

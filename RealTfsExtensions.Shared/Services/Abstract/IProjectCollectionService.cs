namespace RealTfsExtensions.Shared.Services.Abstract
{
	using Microsoft.TeamFoundation.Client;
	using RealTfsExtensions.Shared.Services.Common;

	public interface IProjectCollectionService : IDefaultQueryingService<TfsTeamProjectCollection>
	{
	}
}

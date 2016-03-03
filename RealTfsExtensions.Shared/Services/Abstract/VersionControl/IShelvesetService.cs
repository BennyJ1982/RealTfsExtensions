namespace RealTfsExtensions.Shared.Services.Abstract.VersionControl
{
	using System.Threading.Tasks;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services.Common;

	public interface IShelvesetService : IService
	{
		Task<Shelveset[]> GetShelvesetsByUserAsync(IIdentity user);
	}
}

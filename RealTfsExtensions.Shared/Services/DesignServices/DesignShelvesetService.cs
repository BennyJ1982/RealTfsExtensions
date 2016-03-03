namespace RealTfsExtensions.Shared.Services.DesignServices
{
	using System.Threading.Tasks;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.Services.Common;

	public class DesignShelvesetService : DesignServiceBase<Shelveset[]>, IShelvesetService
	{
		public Task<Shelveset[]> GetShelvesetsByUserAsync(IIdentity user)
		{
			return RunAsTask(() => new Shelveset[0]);
		}
	}
}

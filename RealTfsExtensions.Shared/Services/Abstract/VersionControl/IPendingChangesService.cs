namespace RealTfsExtensions.Shared.Services.Abstract.VersionControl
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services.Common;

	public interface IPendingChangesService : IService
	{
		Task<IPendingChange[]> GetPendingChangesByUserAsync(IIdentity user);

		Task<bool> RemoveLocks(IEnumerable<IPendingChange> pendingChanges, IIdentity owner);
	}
}

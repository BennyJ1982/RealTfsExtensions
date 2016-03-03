namespace RealTfsExtensions.Shared.Services.DesignServices
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.Services.Common;

	public class DesignPendingChangesSevice : DesignServiceBase<IPendingChange[]>, IPendingChangesService
	{
		public Task<IPendingChange[]> GetPendingChangesByUserAsync(IIdentity user)
		{
			return RunAsTask(() => new IPendingChange[0]);
		}

		public Task<bool> RemoveLocks(IEnumerable<IPendingChange> pendingChanges, IIdentity owner)
		{
			throw new System.NotImplementedException();
		}
	}
}

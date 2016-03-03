namespace RealTfsExtensions.Shared.Services.Concrete.VersionControl
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.TeamFoundation.Client;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services.Abstract;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.Services.Common;

	public class PendingChangesService : ContinuationServiceBase<IProjectCollectionService, TfsTeamProjectCollection>, IPendingChangesService
	{
		private readonly ItemSpec[] itemSpecs = new[] { new ItemSpec("$/", RecursionType.Full) };

		public PendingChangesService(IProjectCollectionService parentService)
			: base(parentService)
		{
		}

		public async Task<IPendingChange[]> GetPendingChangesByUserAsync(IIdentity user)
		{
			return await this.ContinueWith(
				collection =>
					{
						if (collection == null)
						{
							return new IPendingChange[0];
						}

						var versionControlServer = collection.GetService<VersionControlServer>();
						return SortAndFilterPendingChanges(versionControlServer.QueryPendingSets(this.itemSpecs, null, user.QualifiedAccount));
					});
		}

		public async Task<bool> RemoveLocks(IEnumerable<IPendingChange> pendingChanges, IIdentity owner)
		{
			return await this.ContinueWith(
				collection =>
				{
					if (collection == null)
					{
						return false;
					}

					var versionControlServer = collection.GetService<VersionControlServer>();
					var groupedChanges = pendingChanges.Where(p => p.IsLocked).GroupBy(p => p.WorkspaceName);

					foreach (var groupedChange in groupedChanges)
					{
						var workspace = versionControlServer.GetWorkspace(groupedChange.Key, owner.QualifiedAccount);
						workspace.PendEdit(groupedChange.Select(g => g.TfsPendingChange.ServerItem).ToArray(), RecursionType.None, null, LockLevel.None);
						//workspace.SetLock(groupedChange.Select(g => g.TfsPendingChange.ServerItem).ToArray(), LockLevel.None);
					}

					return true;
				});
		}

		private static IPendingChange[] SortAndFilterPendingChanges(IEnumerable<PendingSet> pendingSets)
		{
			return pendingSets
				.Where(p => p.Type == PendingSetType.Workspace)
				.SelectMany(p => p.PendingChanges.Select(c => (IPendingChange)new TfsPendingChangeWrapper(c, p.Name)))
				.OrderBy(c => c.FileName).ToArray();
		}
	}
}

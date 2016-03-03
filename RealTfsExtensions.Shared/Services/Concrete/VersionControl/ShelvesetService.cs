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

	public class ShelvesetService : ContinuationServiceBase<IProjectCollectionService, TfsTeamProjectCollection>, IShelvesetService
	{
		public ShelvesetService(IProjectCollectionService parentService)
			: base(parentService)
		{
		}

		public async Task<Shelveset[]> GetShelvesetsByUserAsync(IIdentity user)
		{
			return await this.ContinueWith(
				collection =>
					{
						if (collection == null)
						{
							return new Shelveset[0];
						}

						var versionControlServer = collection.GetService<VersionControlServer>();
						return SortAndFilterShelvesets(versionControlServer.QueryShelvesets(null, user.QualifiedAccount));
					});
		}

		private static Shelveset[] SortAndFilterShelvesets(IEnumerable<Shelveset> shelvesets)
		{
			return shelvesets.OrderByDescending(s => s.CreationDate).ToArray();
		}
	}
}

namespace RealTfsExtensions.Shared.Services.Concrete.VersionControl
{
	using System;
	using System.Linq;
	using Microsoft.TeamFoundation.Client;
	using Microsoft.TeamFoundation.Framework.Client;
	using Microsoft.TeamFoundation.Framework.Common;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services.Abstract;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.Services.Common;

	public class ProjectUserService : QueryingContinuationServiceBase<IIdentity[], IProjectCollectionService, TfsTeamProjectCollection>, IProjectUserService
	{
		public ProjectUserService(IProjectCollectionService parentService)
			: base(parentService)
		{
		}

		protected override IIdentity[] InternalLoad(TfsTeamProjectCollection parentResult)
		{
			try
			{
				if (parentResult != null)
				{
					var identityService = parentResult.GetService<IIdentityManagementService>();

					var everyone = identityService.ReadIdentity(
						GroupWellKnownDescriptors.EveryoneGroup,
						MembershipQuery.Expanded,
						ReadIdentityOptions.None);
					var result = identityService.ReadIdentities(everyone.Members, MembershipQuery.Direct, ReadIdentityOptions.None);

					return result
						.Where(r => !r.IsContainer)
						.Select(r => (IIdentity)new TfsIdentityWrapper(r))
						.OrderBy(r => r.DisplayName)
						.ToArray();

				}
			}
			catch (Exception)
			{
				// TODO
			}

			return new IIdentity[0];
		}
	}
}

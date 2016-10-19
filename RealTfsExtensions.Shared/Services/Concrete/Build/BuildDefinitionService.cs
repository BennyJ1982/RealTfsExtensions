namespace RealTfsExtensions.Shared.Services.Concrete.Build
{
	using System;
	using System.Linq;
	using Microsoft.TeamFoundation.Build.Client;
	using Microsoft.TeamFoundation.Client;
	using RealTfsExtensions.Shared.Services.Abstract;
	using RealTfsExtensions.Shared.Services.Abstract.Build;
	using RealTfsExtensions.Shared.Services.Common;

	public class BuildDefinitionService :
		QueryingContinuationServiceBase<IBuildDefinition[], IProjectCollectionService, TfsTeamProjectCollection>,
		IBuildDefinitionService
	{
		private readonly TfsContext tfsContext;

		public BuildDefinitionService(IProjectCollectionService collectionService, TfsContext tfsContext)
			: base(collectionService)
		{
			this.tfsContext = tfsContext;
		}

		protected override IBuildDefinition[] InternalLoad(TfsTeamProjectCollection parentResult)
		{
			try
			{
				if (parentResult != null && !string.IsNullOrEmpty(this.tfsContext.CurrentTfsProject))
				{
					var buildServer = parentResult.GetService<IBuildServer>();
					return buildServer.QueryBuildDefinitions(this.tfsContext.CurrentTfsProject).OrderBy(b => b.Name).ToArray();
				}
			}
			catch (Exception)
			{
				// TODO
			}

			return new IBuildDefinition[0];
		}
	}
}

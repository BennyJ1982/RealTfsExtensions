namespace RealTfsExtensions.Shared.Services.Concrete
{
	using System;
	using Microsoft.TeamFoundation.Client;
	using RealTfsExtensions.Shared.Services.Abstract;
	using RealTfsExtensions.Shared.Services.Common;

	public class ProjectCollectionService : DefaultQueryingServiceBase<TfsTeamProjectCollection>, IProjectCollectionService
	{
		private readonly TfsContext tfsContext;

		public ProjectCollectionService(TfsContext tfsContext)
		{
			this.tfsContext = tfsContext;
			this.tfsContext.TfsContextChanged += this.OnTfsContextChanged;
		}

		protected override TfsTeamProjectCollection InternalLoad()
		{
			try
			{
				if (this.tfsContext.CurrentTfsUri != null)
				{
					var collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(this.tfsContext.CurrentTfsUri);
					collection.Authenticate();
					return collection;
				}
			}
			catch (Exception)
			{
				// TODO
			}

			return null;
		}

		private void OnTfsContextChanged(object sender, EventArgs e)
		{
			this.RaiseDependenciesHaveChanged();
		}
	}
}

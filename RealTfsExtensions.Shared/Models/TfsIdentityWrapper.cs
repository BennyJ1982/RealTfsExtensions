namespace RealTfsExtensions.Shared.Models
{
	using Microsoft.TeamFoundation.Framework.Client;

	public class TfsIdentityWrapper : IIdentity
	{
		private readonly TeamFoundationIdentity identity;

		public TfsIdentityWrapper(TeamFoundationIdentity identity)
		{
			this.identity = identity;
			this.Domain = identity.GetAttribute("Domain", string.Empty);
			this.Account = identity.GetAttribute("Account", string.Empty);
		}

		public string DisplayName => this.identity.DisplayName;

		public string Domain { get; }

		public string Account { get; }

		public string QualifiedAccount => this.Domain + "\\" + this.Account;

		public TeamFoundationIdentity TeamFoundationIdentity => this.identity;
	}
}

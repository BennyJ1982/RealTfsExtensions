namespace RealTfsExtensions.Shared.Models
{
	using Microsoft.TeamFoundation.Framework.Client;

	public interface IIdentity
	{
		string DisplayName { get; }

		string Domain { get; }

		string Account { get; }

		string QualifiedAccount { get; }

		TeamFoundationIdentity TeamFoundationIdentity { get; }
	}
}

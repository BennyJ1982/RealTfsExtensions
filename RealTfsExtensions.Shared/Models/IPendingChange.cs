namespace RealTfsExtensions.Shared.Models
{
	using Microsoft.TeamFoundation.VersionControl.Client;

	public interface IPendingChange 
	{
		PendingChange TfsPendingChange { get; }

		string WorkspaceName { get; }

		string FileName { get; }

		string LocalOrServerFolder { get; }

		bool IsLocked { get; }
	}
}

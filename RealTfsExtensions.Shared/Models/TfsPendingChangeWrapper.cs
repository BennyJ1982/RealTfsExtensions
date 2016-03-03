namespace RealTfsExtensions.Shared.Models
{
	using Microsoft.TeamFoundation.VersionControl.Client;

	public class TfsPendingChangeWrapper : IPendingChange
	{
		public TfsPendingChangeWrapper(PendingChange tfsPendingChange, string workspaceName)
		{
			this.TfsPendingChange = tfsPendingChange;
			this.WorkspaceName = workspaceName;
			this.FileName = tfsPendingChange.FileName;
			this.LocalOrServerFolder = tfsPendingChange.LocalOrServerFolder;
			this.IsLocked = tfsPendingChange.IsLock;
		}

		public PendingChange TfsPendingChange { get; }

		public string WorkspaceName { get; }

		public string FileName { get; }

		public string LocalOrServerFolder { get; }

		public bool IsLocked { get; }
	}
}

namespace RealTfsExtensions.Shared
{
	using System;

	public class TfsContext
	{
		public Uri CurrentTfsUri { get; private set; }

		public string CurrentTfsProject { get; private set; }

		public event EventHandler TfsContextChanged;

		public void SetContext(Uri tfsUri, string tfsProject)
		{
			this.CurrentTfsUri = tfsUri;
			this.CurrentTfsProject = tfsProject;

			if (this.TfsContextChanged != null)
			{
				this.TfsContextChanged(this, EventArgs.Empty);
			}
		}
	}
}

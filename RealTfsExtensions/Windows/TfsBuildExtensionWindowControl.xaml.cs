namespace RealTfsExtensions.Windows
{
	using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for TfsBuildExtensionWindowControl.
	/// </summary>
	public partial class TfsBuildExtensionWindowControl : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TfsBuildExtensionWindowControl"/> class.
		/// </summary>
		public TfsBuildExtensionWindowControl(object dataContext)
		{
			this.InitializeComponent();
			this.DataContext = dataContext;
		}
	}
}
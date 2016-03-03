using System.Windows;

namespace RealTfsExtensions.WpfClient
{
	using GalaSoft.MvvmLight;

	/// <summary>
	/// Interaction logic for PendingChangesWindow.xaml
	/// </summary>
	public partial class PendingChangesWindow : Window
	{
		public PendingChangesWindow()
		{
			InitializeComponent();
		}

		public void Show(Window owner, ViewModelBase viewModel)
		{
			this.Owner = owner;
			this.PendingChangesView.DataContext = viewModel;
			this.Show();
		}
	}
}

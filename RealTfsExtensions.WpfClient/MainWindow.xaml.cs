using System.Windows;

namespace RealTfsExtensions.WpfClient
{
	using System;
	using RealTfsExtensions.Shared.Messages;
	using IServiceProvider = RealTfsExtensions.Shared.Services.IServiceProvider;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IServiceProvider serviceProvider;

		public MainWindow()
		{
			this.InitializeComponent();
			this.serviceProvider= (IServiceProvider)App.Current.Resources["ServiceProvider"];

			this.serviceProvider.Messenger.Register<OpenPendingChangesViewMessage>(this, this.HandleOpenPendingChangesViewMessage);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			this.serviceProvider.Messenger.Unregister(this);
		}

		private void btnConnext_Click(object sender, RoutedEventArgs e)
		{
			this.serviceProvider.TfsContext.SetContext(new Uri("http://tfs:8080/tfs/Core"), "Facton7");
		}

		private void btnDisconnect_Click(object sender, RoutedEventArgs e)
		{
			this.serviceProvider.TfsContext.SetContext(null, null);

		}

		private void HandleOpenPendingChangesViewMessage(OpenPendingChangesViewMessage message)
		{
			var window = new PendingChangesWindow();
			window.Show(this, message.ViewModelToUse.Value);
		}
	}
}

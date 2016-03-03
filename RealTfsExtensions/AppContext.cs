namespace RealTfsExtensions
{
	public static class AppContext
	{
		public static Shared.Services.IServiceProvider ServiceProvider => RealTfsExtensions.Shared.Services.ServiceProvider.Instance;
	}
}

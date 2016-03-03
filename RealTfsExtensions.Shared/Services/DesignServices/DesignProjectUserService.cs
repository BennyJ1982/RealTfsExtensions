namespace RealTfsExtensions.Shared.Services.DesignServices
{
	using Moq;
	using RealTfsExtensions.Shared.Models;
	using RealTfsExtensions.Shared.Services.Abstract.VersionControl;
	using RealTfsExtensions.Shared.Services.Common;

	public class DesignProjectUserService : DefaultQueryingServiceBase<IIdentity[]>, IProjectUserService
	{
		private readonly IIdentity identity;

		public DesignProjectUserService()
		{
			var mock = new Mock<IIdentity>();
			mock.Setup(m => m.DisplayName).Returns("Test User");
			this.identity = mock.Object;
		}

		protected override IIdentity[] InternalLoad()
		{
			return new[] { this.identity};
		}
	}
}

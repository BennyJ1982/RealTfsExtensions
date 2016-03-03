namespace RealTfsExtensions.Shared.Services.DesignServices
{
	using Microsoft.TeamFoundation.Build.Client;
	using Moq;
	using RealTfsExtensions.Shared.Services.Abstract.Build;
	using RealTfsExtensions.Shared.Services.Common;

	public class DesignBuildDefinitionService : DefaultQueryingServiceBase<IBuildDefinition[]>, IBuildDefinitionService
	{
		private readonly IBuildDefinition definition;

		public DesignBuildDefinitionService()
		{
			var mock = new Mock<IBuildDefinition>();
			mock.Setup(d => d.Description).Returns("Test Build Definition");
			this.definition = mock.Object;
		}

		protected override IBuildDefinition[] InternalLoad()
		{
			return new[] { this.definition };
		}
	}
}

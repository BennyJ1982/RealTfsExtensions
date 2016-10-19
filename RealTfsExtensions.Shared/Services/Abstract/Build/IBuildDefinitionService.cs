namespace RealTfsExtensions.Shared.Services.Abstract.Build
{
	using Microsoft.TeamFoundation.Build.Client;
	using RealTfsExtensions.Shared.Services.Common;

	public interface IBuildDefinitionService : IDefaultQueryingService<IBuildDefinition[]>
	{
	}
}

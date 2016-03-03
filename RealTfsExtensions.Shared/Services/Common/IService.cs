namespace RealTfsExtensions.Shared.Services.Common
{
	using System;

	public interface IService
	{
		bool IsBusy { get; }

		event EventHandler DependenciesHaveChanged;
	}
}

namespace RealTfsExtensions.Shared.Messages
{
	using System;
	using GalaSoft.MvvmLight.Messaging;
	using RealTfsExtensions.Shared.Models;
	using ViewModels;

	public class OpenPendingChangesViewMessage : MessageBase
	{
		public OpenPendingChangesViewMessage(Lazy<ViewModelBase> viewModelToUse, IIdentity user)
		{
			this.ViewModelToUse = viewModelToUse;
			this.User = user;
		}

		public IIdentity User { get; }

		public Lazy<ViewModelBase> ViewModelToUse { get; }
	}
}

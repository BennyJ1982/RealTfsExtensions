namespace RealTfsExtensions.Shared.Models
{
	using System.Collections.Generic;
	using System.Linq;

	public static class IdentityExtensions
	{
		public static IIdentity GetCurrentIdentity(this IEnumerable<IIdentity> identities)
		{
			var currentIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
			if (currentIdentity != null)
			{
				return identities.FirstOrDefault(i => i.QualifiedAccount == currentIdentity.Name);
			}

			return null;
		}
	}
}

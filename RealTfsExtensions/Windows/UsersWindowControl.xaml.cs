//------------------------------------------------------------------------------
// <copyright file="UsersWindowControl.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace RealTfsExtensions.Windows
{
	using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for UsersWindowControl.
	/// </summary>
	public partial class UsersWindowControl : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UsersWindowControl"/> class.
		/// </summary>
		public UsersWindowControl(object dataContext)
		{
			this.InitializeComponent();
			this.DataContext = dataContext;
		}
	}
}
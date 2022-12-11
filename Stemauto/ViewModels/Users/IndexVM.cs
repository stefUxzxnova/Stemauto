using Stemauto.Entities;
using System.ComponentModel;

namespace Stemauto.ViewModels.Users
{
    public class IndexVM
    {
		[DisplayName("Username: ")]
		public string Username { get; set; }
		[DisplayName("First Name: ")]
		public string FirstName { get; set; }
		[DisplayName("Last Name: ")]
		public string LastName { get; set; }
		[DisplayName("Email: ")]
		public string Email { get; set; }

		//skip and take
		public int Page { get; set; }
		public int ItemsPerPage { get; set; }


		//колко станици имаме 
		public int PagesCount { get; set; }

		public List<User> Items { get; set; }
	}
}

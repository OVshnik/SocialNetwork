using SocialNetwork.Data.Models;
using SocialNetwork.ViewModels.Account;

namespace SocialNetwork.ViewModels.Extensions
{
	public static class UserFromModel
	{
		public static User Convert(this User user, UserEditViewModel usereditvm)
		{
			if (!string.IsNullOrEmpty(usereditvm.Image))
			{
				user.Image = usereditvm.Image;
			}
			if (!string.IsNullOrEmpty(usereditvm.LastName))
			{
				user.LastName = usereditvm.LastName;
			}
			if (!string.IsNullOrEmpty(usereditvm.MiddleName))
			{
				user.MiddleName = usereditvm.MiddleName;
			}
			if (!string.IsNullOrEmpty(usereditvm.FirstName))
			{
				user.FirstName = usereditvm.FirstName;
			}
			if (!string.IsNullOrEmpty(usereditvm.Email))
			{
				user.Email = usereditvm.Email;
			}
				user.BirthDate = usereditvm.BirthDate;
			if (!string.IsNullOrEmpty(usereditvm.UserName))
			{
				user.UserName = usereditvm.UserName;
			}
			if (!string.IsNullOrEmpty(usereditvm.Status))
			{
				user.Status = usereditvm.Status;
			}
			if (!string.IsNullOrEmpty(usereditvm.About))
			{
				user.About = usereditvm.About;
			}
			return user;

		}
	}
}


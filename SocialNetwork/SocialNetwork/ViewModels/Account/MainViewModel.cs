namespace SocialNetwork.ViewModels.Account
{
	public class MainViewModel
	{
		public RegisterViewModel Register { get; set; }
		public LoginViewModel Login { get;set; }

		public MainViewModel ()
		{
			Login = new LoginViewModel();
			Register = new RegisterViewModel();
		}
	}
}

using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace SocialNetwork.ViewModels.Account
{
    public class RegisterViewModel
    {
		[Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; }

		[Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; }

		[Required(ErrorMessage = "Поле Email обязательно для заполнения")]
		[EmailAddress]
		[Display(Name = "Email")]
        public string EmailReg { get; set; }

		[Required(ErrorMessage = "Поле Год обязательно для заполнения")]
		[Display(Name = "Год")]
        public int Year { get; set; }

		[Required(ErrorMessage = "Поле День обязательно для заполнения")]
		[Display(Name = "День")]
        public int Date { get; set; }

		[Required(ErrorMessage = "Поле Месяц обязательно для заполнения")]
		[Display(Name = "Месяц")]
        public int Month { get; set; }

		[Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
		[DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string PasswordReg { get; set; }

		[Required(ErrorMessage = "Обязательно подтвердите пароль")]
		[Compare("PasswordReg", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

		public string Login => EmailReg;
	}
}

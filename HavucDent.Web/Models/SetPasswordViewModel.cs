using System.ComponentModel.DataAnnotations;

namespace HavucDent.Web.Models
{
	public class SetPasswordViewModel
	{
		public string UserId { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
		public string ConfirmPassword { get; set; }
	}
}
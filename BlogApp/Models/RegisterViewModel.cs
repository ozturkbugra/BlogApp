using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Required]
        [Display(Name = "Ad Soyad")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="E posta")]
        public string? Email { get; set; }


        [Required]
        [StringLength(10,ErrorMessage = "Max 10 Karakter Belirtiniz") ]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Parola Tekrar")]
        [Compare(nameof(Password), ErrorMessage = "Parolanız Eşleşmiyor")]//parola ile karşılaştırır
        public string? ConfirmPassword { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyStore.ViewModels.VM
{
   public class ChangeAddressVm
    {
        [Required(ErrorMessage = "Adres jest wymagany.")]
        [StringLength(200, ErrorMessage = "Adres nie może być dłuższy niż 200 znaków.")]
        [Display(Name = "Adres")]
        public string Address { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyStore.ViewModels.VM
{
    public class TransferPaymentVm
    {
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Potwierdzam wykonanie przelewu")]
        public bool Confirmed { get; set; }
    }
}

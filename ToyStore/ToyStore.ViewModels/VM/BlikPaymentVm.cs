using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyStore.ViewModels.VM
{
    public class BlikPaymentVm
    {
        public int OrderId { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$")]
        public string BlikCode { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*creating viewmodels here, this ViewModels are directly using in Views */
namespace BulkyBook.Models.ViewModels
{
    public class ProductVM
    {
        //for product model 
        public Product Product { get; set; }

        //this is for dropdown
        //92.9.35 ValidateNever is used here, basically its telling that dont validate the given property
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}

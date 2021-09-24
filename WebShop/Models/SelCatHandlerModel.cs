using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebShop.Models
{
    public class SelCatHandlerModel
    {
        public IEnumerable<SelectListItem> Categories { get; set; }

        public IEnumerable<string> SelectedCategories { get; set; }
    }
}

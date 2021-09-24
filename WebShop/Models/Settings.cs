using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class Settings
    {
        [Key]
        public int settingsKey { get; set; }
        public bool UseInMemoryStorage { get; set; }
    }
}

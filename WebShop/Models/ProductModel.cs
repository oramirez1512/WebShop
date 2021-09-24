﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }

        public int  Price { get; set; }

        public string Description { get; set; }

        public string Categories { get; set; }
    }
}

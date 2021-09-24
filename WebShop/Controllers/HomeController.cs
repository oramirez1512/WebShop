using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private List<ProductModel> products;

        private  WsDBContext _context;

        public HomeController(ILogger<HomeController> logger, WsDBContext context)
        {
            _logger = logger;
            _context = context;
            
            

        }

        public IEnumerable<SelectListItem> Categories()
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            categories.Add(new SelectListItem { Text = "cat1", Value = "cat1", Selected = true });
            categories.Add(new SelectListItem { Text = "cat2", Value = "cat2", Selected = true });
            categories.Add(new SelectListItem { Text = "cat3", Value = "cat3" });
            return categories;
        }

        public IActionResult Success() 
        {
            return View();
        }

        public IActionResult Index()
        {
            SelCatHandlerModel selectCategory = new SelCatHandlerModel();
            selectCategory.Categories = Categories();
            return View(selectCategory);
        }

        public IActionResult Products()
        {
            products = LoadData();
            return View(products);
        }

        public IActionResult Settings()
        {
            Models.Settings settings = _context.Settings.FirstOrDefault();
            return View(settings);
        }

        [HttpPost]
        public ActionResult Settings(Settings model) 
        {
            _context.Settings.FirstOrDefault().UseInMemoryStorage = model.UseInMemoryStorage;
            _context.SaveChanges();
            return View("Success");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult Index(string txtName, string txtDescription, int numPrice, SelCatHandlerModel Model)
        {
           
            Console.WriteLine(_context.Products.Count());

                SaveProduct( txtName,  txtDescription,  numPrice, Model.SelectedCategories);
            Index();
            return View("Success");
        }

        public void SaveProduct(string txtName, string txtDescription, int numPrice, IEnumerable<string> Selcategories) 
        {
            
            ProductModel product = new ProductModel();
            product.Name = txtName;
            product.Description = txtDescription;
            product.Price = numPrice;
            product.Categories = string.Join(",", Selcategories.ToArray());
            List<ProductModel> products = new List<ProductModel>();
            products.AddRange(LoadData());
            product.ProductId = products.Count + 1;
            products.Add(product);
            if (_context.Settings.FirstOrDefault().UseInMemoryStorage) 
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            else { 
                string jsonData = JsonConvert.SerializeObject(products);
                string path = @"/product.json";
                System.IO.File.WriteAllText(path, jsonData);
            }

        }

        public List<ProductModel> LoadData() 
        {

            
            string path = @"/product.json";
            List<ProductModel> products = new List<ProductModel>();
            bool usememory = _context.Settings.FirstOrDefault().UseInMemoryStorage;
            if (usememory) 
            {
                products = _context.Products.ToList();
            }
            else { 
                try 
                {
                 products = JsonConvert.DeserializeObject<List<ProductModel>>(System.IO.File.ReadAllText(path)).ToList();
                }
                catch 
                {
                    string jsonData = JsonConvert.SerializeObject(products);
                    System.IO.File.WriteAllText(path, jsonData);
                }
            }
            return products;
        }
    }
}

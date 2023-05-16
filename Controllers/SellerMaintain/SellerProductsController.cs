//using AspNetCore;
using ISpan.Project_02_DessertStory.Customer.Models.EF;
using ISpan.Project_02_DessertStory.Customer.Models.Repos;
using ISpan.Project_02_DessertStory.Customer.Models.Services;
using ISpan.Project_02_DessertStory.Customer.Models.Services.Dtos;
using ISpan.Project_02_DessertStory.Customer.ViewModels.Sellers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ISpan.Project_02_DessertStory.Customer.Controllers.SellerMaintain
{
    public class SellerProductsController : SellerSuperController
    {
        private readonly iSpanDessertShop2023MarchContext _db;
        private readonly SellerProductsRepository _repo;
        private readonly SellerProductsService _service;
        
        IWebHostEnvironment _enviro;

        public SellerProductsController(iSpanDessertShop2023MarchContext db, SellerProductsService service, IWebHostEnvironment p)
        {
            //var a = _seller;
            _db = db; // 注入
            _service = service;
            _repo = new SellerProductsRepository(_db);
            _enviro = p;
        }

        /// <summary>
        /// 後台，顯示該賣家的商品
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult List(int? Id)
        {
            Id = _sellerId;
            if (Id == null)
            {
                return View();
                //Id = _seller.Id;
            }
            if (Id != null)
            {
                var sellerProducts = _service.QueryBySellerId(Id);
                var list = new List<SellerProductsViewModel>();
                if (sellerProducts != null)
                {
                    foreach (var item in sellerProducts)
                    {
                        list.Add(item.ToSellerProductsViewModel());
                    }
                    return View(list);
                }
            }
            return View();
        }


        /// <summary>
        /// 後台，修改商品(顯示)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult Edit(int? Id)
        {
            var sellerProducts = _service.QueryByProductId(Id).ToSellerProductsViewModel();
            return View(sellerProducts);
        }

        /// <summary>
        /// 後台，修改商品(進資料庫)
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditSellerProducts(SellerProductsViewModel vm)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == vm.Id);
            if (product != null)
            {
                if (vm.photo1 != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/images/products/" + photoName;
                    vm.photo1.CopyTo(new FileStream(path, FileMode.Create));
                    product.Picture1 = photoName;
                }
                if (vm.photo2 != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/images/products/" + photoName;
                    vm.photo2.CopyTo(new FileStream(path, FileMode.Create));
                    product.Picture2 = photoName;
                }
                if (vm.photo3 != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/images/products/" + photoName;
                    vm.photo3.CopyTo(new FileStream(path, FileMode.Create));
                    product.Picture3 = photoName;
                }
                product.ProductName = vm.ProductName;
                product.CategoryId = vm.CategoryId;
                product.UnitPrice = vm.UnitPrice;
                product.Description = vm.Description;
                product.ProductStatus = vm.ProductStatus;
                product.UnitsInStock = vm.UnitsInStock;
                product.UnitPrice = vm.UnitPrice;
                _db.SaveChanges();
            }
            return RedirectToAction("List", new { Id = vm.SellerId });
        }

        /// <summary>
        /// 後台，新增商品(顯示)
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var createProductVm = new SellerProductsCreateViewModel();
            createProductVm.SellerId = _seller.Id;

            List<Category> list = new List<Category>();
            foreach (var item in _db.Categories)
            {
                list.Add(new Category
                {
                    Id = item.Id,
                    Name = item.Name,
                    DisplayOrder = item.DisplayOrder
                });
            }

            createProductVm.Category = list;
            return View(createProductVm);
        }
        /// <summary>
        /// 後台，新增商品(進資料庫)
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateSellerProducts(SellerProductsCreateViewModel vm)
        {
            
            if (!string.IsNullOrEmpty(vm.ProductName) && !string.IsNullOrEmpty(vm.ProductUnit))
            {
                var product = new Product()
                {
                    CategoryId = vm.CategoryId,
                    SellerId = vm.SellerId,
                    ProductName = vm.ProductName,
                    ProductStatus = vm.ProductStatus,
                    ProductUnit = vm.ProductUnit,
                    ProductSize = vm.ProductSize,
                    Description = vm.Description,
                    UnitsInStock = vm.UnitsInStock,
                    UnitPrice = vm.UnitPrice,
                };

                if (vm.photoCreate1 != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/images/products/" + photoName;
                    vm.photoCreate1.CopyTo(new FileStream(path, FileMode.Create));
                    product.Picture1 = photoName;
                }

                if (vm.photoCreate2 != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/images/products/" + photoName;
                    vm.photoCreate2.CopyTo(new FileStream(path, FileMode.Create));
                    product.Picture2 = photoName;
                }

                if (vm.photoCreate3 != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/images/products/" + photoName;
                    vm.photoCreate3.CopyTo(new FileStream(path, FileMode.Create));
                    product.Picture3 = photoName;
                }
                _db.Products.Add(product);
                _db.SaveChanges();
            }
            return RedirectToAction("List", new { Id = vm.SellerId });
        }
        /// <summary>
        /// 後台，刪除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var prod = _db.Products.FirstOrDefault(p => p.Id == id);
                if (prod != null)
                {
                    _db.Products.Remove(prod);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using ISpan.Project_02_DessertStory.Customer.Models.EF;
using ISpan.Project_02_DessertStory.Customer.ViewModels.Sellers;
using ISpan.Project_02_DessertStory.Customer.Models.Repos;
using System.Text.Json;
using ISpan.Project_02_DessertStory.Customer.Models.Services;

namespace ISpan.Project_02_DessertStory.Customer.Controllers.SellerMaintain
{
    /// <summary>
    /// 賣場相關設定
    /// </summary>

    public class StoreSettingController : SellerSuperController
    {
        private readonly iSpanDessertShop2023MarchContext _db;
        private readonly StoreMaintainRepository _repo;
        private readonly int _sellerId;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        IWebHostEnvironment _enviro;

        public StoreSettingController(iSpanDessertShop2023MarchContext db, IWebHostEnvironment p)
        {
            _db = db; // 注入
            _repo = new StoreMaintainRepository(_db);
            _enviro = p;
            _sellerId = _seller.Id;
        }

        /// <summary>
        /// 後台，顯示該賣家的商品
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult List(int? Id)
        {
            if (Id == null)
            {
                Id = _seller.Id;
            }
            var storeInformation = _repo.QueryByStoreId(Id);
            return View(storeInformation);
        }
        /// <summary>
        /// 後台，修改商品(顯示)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult Edit(int? Id)
        {

            var storeInformation = _repo.QueryByStoreId(Id);
            return View(storeInformation);
        }
        /// <summary>
        /// 後台，修改商品(儲存)
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditStoreInformation(StoreSettingViewModel vm)
        {
            var seller = _db.Sellers.FirstOrDefault(s => s.Id == vm.Id);
            if (seller != null)
            {
                if (vm.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/images/" + photoName;
                    vm.photo.CopyTo(new FileStream(path, FileMode.Create));
                    seller.Picture = photoName;
                }
                seller.StoreName = vm.StoreName;
                seller.Description = vm.Description;
                _db.SaveChanges();
            }
            return RedirectToAction("List", new { vm.Id });
        }
    }
}

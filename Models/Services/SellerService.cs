﻿using ISpan.Project_02_DessertStory.Customer.Models.EF;
using ISpan.Project_02_DessertStory.Customer.Models.Services.Interfaces;

namespace ISpan.Project_02_DessertStory.Customer.Models.Services
{
    public class SellerService: ISellerService
    {
        private readonly iSpanDessertShop2023MarchContext _db;
        public SellerService(iSpanDessertShop2023MarchContext db)
        {
            _db = db;
        }
        /// <summary>
        /// 檢查帳號是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsAccountExist(string account)
        {
            return _db.Sellers.Any(a => a.Account == account);
        }
    }
}

﻿using khoaLuan_webGiay.Data;
using khoaLuan_webGiay.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace khoaLuan_webGiay.Models.Components
{
    public class MenuCategory : ViewComponent
    {
        private readonly KhoaLuanContext db;

        public MenuCategory(KhoaLuanContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Categories
                .Include(ca => ca.Products)
                .Select(ca => new MenuCategoryVM
                {
                    CategoryId = ca.CategoryId,
                    CategoryName = ca.CategoryName,
                    SoLuong = ca.Products.SelectMany(p => p.ProductSizes).Sum(ps => ps.Quantity)
                }).ToList();

            return View("_MenuCategory", data);
        }
    }
}

using duanwebsite.Data;
using duanwebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace duanwebsite.ViewComponents
{
    public class LoaiListViewComponent : ViewComponent
    {
        private readonly DungtmShopContext db;

        public LoaiListViewComponent(DungtmShopContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new LoaiListVM
            {
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            }).OrderBy(p => p.TenLoai);

            return View(data); // Default.cshtml
                               //return View("Default", data);
        }
    }
}

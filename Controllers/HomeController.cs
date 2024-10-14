using duanwebsite.Data;
using duanwebsite.Models;
using duanwebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace duanwebsite.Controllers
{
	public class HomeController : Controller
	{
        private readonly DungtmShopContext db;
        public HomeController(DungtmShopContext conetxt)
		{
            db = conetxt;
        }

        public IActionResult Index(int? loai, int page = 1, int pageSize = 8)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
				ViewBag.Loai = loai.Value;
			}

            int totalItems = hangHoas.Count(); // Total number of items
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Apply pagination
            var result = hangHoas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new HangHoaVM
                {
                    MaHh = p.MaHh,
                    TenHH = p.TenHh,
                    DonGia = p.DonGia ?? 0,
                    Hinh = p.Hinh ?? "",
                    MoTaNgan = p.MoTaDonVi ?? "",
                    TenLoai = p.MaLoaiNavigation.TenLoai
                }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(result);
        }

        [Route("/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

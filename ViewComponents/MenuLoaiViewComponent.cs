using duanwebsite.Data;
using duanwebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace duanwebsite.ViewComponents
{
	public class MenuLoaiViewComponent : ViewComponent
	{
		private readonly DungtmShopContext db;

		public MenuLoaiViewComponent(DungtmShopContext context) => db = context;

		public IViewComponentResult Invoke()
		{
			var data = db.Loais.Select(lo => new MenuLoaiVM
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

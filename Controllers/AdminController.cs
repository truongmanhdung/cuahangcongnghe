using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using duanwebsite.Data;
using Microsoft.AspNetCore.Authorization;
using duanwebsite.ViewModels;
using System.Drawing.Printing;

namespace duanwebsite.Controllers
{
    public class AdminController : Controller
    {
        private readonly DungtmShopContext _context;

        public AdminController(DungtmShopContext context)
        {
            _context = context;
        }

        // GET: Admin'
        [Authorize(Roles = "Admin")]
        public ActionResult AdminPage()
        {
            return View();
        }
        public Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            var hangHoas = _context.KhachHangs.AsQueryable();
            int totalItems = hangHoas.Count(); // Total number of items
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Apply pagination
            var result = hangHoas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new KhachHang
                {
                    MaKh = p.MaKh, // Giả sử MaKh là mã khách hàng
                    HoTen = p.HoTen, // Họ tên
                    NgaySinh = p.NgaySinh, // Ngày sinh
                    DiaChi = p.DiaChi, // Địa chỉ
                    DienThoai = p.DienThoai, // Điện thoại
                    Email = p.Email, // Email
                    Hinh = p.Hinh ?? "", // Hình ảnh
                    HieuLuc = p.HieuLuc, // Hiệu lực
                    VaiTro = p.VaiTro // Vai trò
                }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return Task.FromResult<IActionResult>(View(result));
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKh,MatKhau,HoTen,GioiTinh,NgaySinh,DiaChi,DienThoai,Email,Hinh,HieuLuc,VaiTro,RandomKey")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKh,MatKhau,HoTen,GioiTinh,NgaySinh,DiaChi,DienThoai,Email,Hinh,HieuLuc,VaiTro,RandomKey")] KhachHang khachHang)
        {
            if (id != khachHang.MaKh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKh))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(string id)
        {
            return _context.KhachHangs.Any(e => e.MaKh == id);
        }
    }
}

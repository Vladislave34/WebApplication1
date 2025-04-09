using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppAlionaContext _context;

        public HomeController(ILogger<HomeController> logger,
            AppAlionaContext context)
        {
            _logger = logger;
            _context = context;
            if (!_context.Banans.Any())
            {
                DataBaseManager dbm = new DataBaseManager();
                dbm.AddBanans(_context);
            }
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Thread main id " + Thread.CurrentThread.ManagedThreadId);
            List<Banan> list = await GetListBanansAsync();
            //Task<List<Banan>> list = GetListBanansAsync();
            return View(list);
        }

        [HttpGet] //Цей метод буде відображати сторінку де можвка вказаи інформацію про користувача
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile Photo, string LastName, string FirstName, string Phone, bool Sex)
        {
            string? photoUrl = null;

            if (Photo != null && Photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetFileName(Photo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(stream);
                }

                // Шлях, який зберігатимемо в базі
                photoUrl = "D:\\Git\\project_c_13\\WebApplication1\\wwwroot\\uploads\\" + fileName;
            }

            // Створюємо об'єкт
            var banan = new Banan
            {
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                Sex = Sex,
                Image = photoUrl
            };

            // Зберігаємо в базу
            await _context.Banans.AddAsync(banan);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private Task<List<Banan>> GetListBanansAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Child Thread: {Thread.CurrentThread.ManagedThreadId}");
                return _context.Banans.ToList();
            });
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
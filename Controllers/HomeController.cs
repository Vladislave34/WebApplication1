using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Data;
using WebApplication1.Models;
using SixLabors.ImageSharp.Formats.Webp;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;

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

        //[HttpPost]
        //public async Task<IActionResult> Create(IFormFile Photo, string LastName, string FirstName, string Phone, bool Sex)
        //{
        //    string? photoUrl = null;

        //    if (Photo != null && Photo.Length > 0)
        //    {
        //        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        //        Directory.CreateDirectory(uploadsFolder);

        //        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
        //        var filePath = Path.Combine(uploadsFolder, fileName);

        //        using var stream = Photo.OpenReadStream();
        //        using var newImage = await Image.LoadAsync(stream); // ImageSharp завантажує з потоку

        //        newImage.Mutate(x => x.Resize(new ResizeOptions
        //        {
        //            Size = new Size(800, 600),
        //            Mode = ResizeMode.Max
        //        }));

        //        await newImage.SaveAsync(filePath); // автоматично визначає формат за розширенням

        //        // Шлях, який зберігатимемо в базі
        //        photoUrl = fileName;
        //    }

        //    // Створюємо об'єкт
        //    var banan = new Banan
        //    {
        //        FirstName = FirstName,
        //        LastName = LastName,
        //        Phone = Phone,
        //        Sex = Sex,
        //        Image = photoUrl
        //    };

        //    // Зберігаємо в базу
        //    await _context.Banans.AddAsync(banan);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile Photo, string LastName, string FirstName, string Phone, bool Sex)
        {
            string? photoUrl = null;

            if (Photo != null && Photo.Length > 0)
            {
                var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var uniqueFolderName = Guid.NewGuid().ToString();
                var targetFolder = Path.Combine(baseFolder, uniqueFolderName);
                Directory.CreateDirectory(targetFolder);

                var extension = ".webp"; // Зберігаємо виключно як webp

                using var stream = Photo.OpenReadStream();
                using var image = await Image.LoadAsync(stream); // ImageSharp

                int[] sizes = new[] { 100, 200, 400, 600, 800, 1200 };

                var encoder = new WebpEncoder
                {
                    Quality = 75 // можна налаштувати якість
                };

                foreach (var size in sizes)
                {
                    var resized = image.Clone(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(size, size),
                        Mode = ResizeMode.Max
                    }));

                    var resizedFileName = $"{size}{extension}";
                    var resizedFilePath = Path.Combine(targetFolder, resizedFileName);

                    await resized.SaveAsync(resizedFilePath, encoder);

                    if (size == 800)
                    {
                        photoUrl = Path.Combine(uniqueFolderName, resizedFileName).Replace("\\", "/");
                    }
                }
            }

            var banan = new Banan
            {
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                Sex = Sex,
                Image = photoUrl // шлях до 800.webp
            };

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
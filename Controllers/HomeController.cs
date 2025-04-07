using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
            if (!_context.News.Any())
            {
                DataBaseManager dbm = new DataBaseManager();
                dbm.AddNews(_context);
            }
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Thread main id " + Thread.CurrentThread.ManagedThreadId);
            List<News> list = await GetListBanansAsync();
            //Task<List<Banan>> list = GetListBanansAsync();
            return View(list);
        }

        private Task<List<News>> GetListBanansAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Child Thread: {Thread.CurrentThread.ManagedThreadId}");
                return _context.News.ToList();
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

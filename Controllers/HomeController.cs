using BlogSeoAi.Data;
using BlogSeoAi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogSeoAi.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
        var last10 = await _db.Posts
            .AsNoTracking()
            .Where(p => p.Status == PostStatus.Published)
            .OrderByDescending(p => p.PublishedAtUtc)
            .Take(10)
            .ToListAsync();

        return View(last10);
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

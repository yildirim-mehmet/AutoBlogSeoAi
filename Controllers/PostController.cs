using BlogSeoAi.Data;
using BlogSeoAi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSeoAi.Controllers;

public class PostController : Controller
{
    private readonly AppDbContext _db;
    public PostController(AppDbContext db) => _db = db;

    //public async Task<IActionResult> Details(string slug)
    //{
    //    var post = await _db.Posts
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync(p => p.Slug == slug && p.Status == PostStatus.Published);

    //    if (post is null) return NotFound();

    //    return View(post);
    //}
    //// BLOG LİSTE
    //[HttpGet("/blog")]
    //public async Task<IActionResult> Index(int page = 1)
    //{
    //    const int pageSize = 10;

    //    var query = _db.Posts
    //        .AsNoTracking()
    //        .Where(p => p.Status == PostStatus.Published)
    //        .OrderByDescending(p => p.PublishedAtUtc);

    //    var total = await query.CountAsync();

    //    var posts = await query
    //        .Skip((page - 1) * pageSize)
    //        .Take(pageSize)
    //        .ToListAsync();

    //    ViewBag.Page = page;
    //    ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

    //    return View(posts);
    //}

    // DETAy 
    [HttpGet("/blog/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var post = await _db.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.Slug == slug &&
                p.Status == PostStatus.Published);

        if (post == null)
            return NotFound();

        return View(post);
    }


    [HttpGet("/blog")]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 25; // <-- 10 yerine 25

        var query = _db.Posts
            .AsNoTracking()
            .Where(p => p.Status == PostStatus.Published)
            .OrderByDescending(p => p.PublishedAtUtc);

        var total = await query.CountAsync();

        var posts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.Page = page;
        ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

        return View(posts);
    }


}
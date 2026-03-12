using BlogSeoAi.Data;
using BlogSeoAi.Models;
using BlogSeoAi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSeoAi.Controllers.Admin;

[Route("admin/posts")]
public class PostsController : Controller
{
    private readonly AppDbContext _db;
    private readonly ISlugService _slug;

    public PostsController(AppDbContext db, ISlugService slug)
    {
        _db = db;
        _slug = slug;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var posts = await _db.Posts
            .OrderByDescending(p => p.PublishedAtUtc ?? p.CreatedAtUtc)
            .ToListAsync();

        return View(posts);
    }

    [HttpGet("create")]
    public IActionResult Create() => View(new PostEditVm());

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PostEditVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var slug = string.IsNullOrWhiteSpace(vm.Slug)
            ? await _slug.GenerateUniqueSlugAsync(vm.Title)
            : await _slug.GenerateUniqueSlugAsync(vm.Slug!);

        var post = new Post
        {
            Title = vm.Title,
            Slug = slug,
            ContentHtml = vm.ContentHtml,
            Excerpt = vm.Excerpt,
            SeoTitle = vm.SeoTitle,
            SeoDescription = vm.SeoDescription,
            FeaturedImageUrl = vm.FeaturedImageUrl,
            Status = vm.Status,
            PublishedAtUtc = vm.Status == PostStatus.Published
                ? (vm.PublishedAtLocal?.ToUniversalTime() ?? DateTime.UtcNow)
                : null
        };

        _db.Posts.Add(post);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var post = await _db.Posts.FindAsync(id);
        if (post is null) return NotFound();

        var vm = new PostEditVm
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            ContentHtml = post.ContentHtml,
            Excerpt = post.Excerpt,
            SeoTitle = post.SeoTitle,
            SeoDescription = post.SeoDescription,
            FeaturedImageUrl = post.FeaturedImageUrl,
            Status = post.Status,
            PublishedAtLocal = post.PublishedAtUtc?.ToLocalTime()
        };

        return View(vm);
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PostEditVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var post = await _db.Posts.FindAsync(id);
        if (post is null) return NotFound();

        post.Title = vm.Title;

        // Slug değiştiyse unique üret
        if (!string.IsNullOrWhiteSpace(vm.Slug) && vm.Slug != post.Slug)
            post.Slug = await _slug.GenerateUniqueSlugAsync(vm.Slug!, post.Id);
        else if (string.IsNullOrWhiteSpace(vm.Slug))
            post.Slug = await _slug.GenerateUniqueSlugAsync(vm.Title, post.Id);

        post.ContentHtml = vm.ContentHtml;
        post.Excerpt = vm.Excerpt;
        post.SeoTitle = vm.SeoTitle;
        post.SeoDescription = vm.SeoDescription;
        post.FeaturedImageUrl = vm.FeaturedImageUrl;
        post.Status = vm.Status;

        post.PublishedAtUtc = vm.Status == PostStatus.Published
            ? (vm.PublishedAtLocal?.ToUniversalTime() ?? post.PublishedAtUtc ?? DateTime.UtcNow)
            : null;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _db.Posts.FindAsync(id);
        if (post is null) return NotFound();

        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
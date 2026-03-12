using BlogSeoAi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogSeoAi.Controllers.Admin;

public class AutomationController : Controller
{
    private readonly IDailyPostGenerator _gen;

    public AutomationController(IDailyPostGenerator gen)
    {
        _gen = gen;
    }

    // ✅ SADECE /admin/automation
    [HttpGet("/admin/automation")]
    public IActionResult Index()
    {
        return View();
    }

    // ✅ SADECE /admin/automation/run-today
    [HttpPost("/admin/automation/run-today")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RunToday(CancellationToken ct)
    {
        var post = await _gen.GenerateAndPublishForTodayAsync(ct);

        TempData["Success"] = $"Post oluşturuldu: {post.Title}";
        return Redirect("/admin/automation");
    }

    // ✅ Butona basmadan: sayfaya girince otomatik üretir
    [HttpGet("/admin/automation/autorun")]
    public async Task<IActionResult> AutoRun(CancellationToken ct)
    {
        var expected = "lacasadetakicom";
        var got = Request.Query["key"].ToString();

        if (string.IsNullOrWhiteSpace(expected) || got != expected)
            return Forbid(); // 403
        var post = await _gen.GenerateAndPublishForTodayAsync(ct);
        return Redirect($"/blog/{post.Slug}");
    }
}
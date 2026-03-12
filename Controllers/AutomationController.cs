using BlogSeoAi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogSeoAi.Controllers;

public class AutomationController : Controller
{
    //private readonly IDailyPostGenerator _gen;

    //public AutomationController(IDailyPostGenerator gen) => _gen = gen;



    private readonly IDailyPostGenerator _gen;

    public AutomationController(IDailyPostGenerator gen) => _gen = gen;

    //  Sayfaya gidince otomatik çalışır güvenlik için sonuna etiket ekledim kendi sitene göre değiştir u değeri 
   // [HttpGet("/admin/automation/autorun")]
    public async Task<IActionResult> AutoRun(CancellationToken ct)
    {

        var expected = "lacasadetakicom";
        var got = Request.Query["key"].ToString();

        if (string.IsNullOrWhiteSpace(expected) || got != expected)
            return Forbid(); // 403

        var post = await _gen.GenerateAndPublishForTodayAsync(ct);

        // oluşturulan içeriğe git
        return Redirect($"/blog/{post.Slug}");
    }




    [HttpGet("/admin/automation")]
    public IActionResult Index() => View();

    [HttpPost("/admin/automation/run-today")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RunToday(CancellationToken ct)
    {
        var expected = "lacasadetakicom";
        var got = Request.Query["key"].ToString();

        if (string.IsNullOrWhiteSpace(expected) || got != expected)
            return Forbid(); // 403

        var post = await _gen.GenerateAndPublishForTodayAsync(ct);
        TempData["Success"] = $"Post oluşturuldu: {post.Title}";
        return RedirectToAction(nameof(Index));
    }
}
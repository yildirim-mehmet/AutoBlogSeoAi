using System.Globalization;
using System.Text;
using System.Xml;
using BlogSeoAi.Data;
using BlogSeoAi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSeoAi.Controllers;

public class SitemapController : Controller
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _cfg;

    public SitemapController(AppDbContext db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }


    [HttpGet("/sitemap.xml")]
    public async Task SitemapXml()
    {
        var baseUrl = (_cfg["Site:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}")
            .TrimEnd('/');

        var posts = await _db.Posts
            .AsNoTracking()
            .Where(p => p.Status == PostStatus.Published)
            .OrderByDescending(p => p.PublishedAtUtc)
            .Select(p => new { p.Slug, p.PublishedAtUtc })
            .ToListAsync();

        var settings = new XmlWriterSettings
        {
            Indent = true,
            Encoding = Encoding.UTF8,
            OmitXmlDeclaration = false
        };

        using (var ms = new MemoryStream())
        {
            using (var xw = XmlWriter.Create(ms, settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                WriteUrl(xw, $"{baseUrl}/blog", DateTime.UtcNow, "daily", "0.8");

                foreach (var p in posts)
                {
                    var loc = $"{baseUrl}/blog/{p.Slug}";
                    var lastMod = p.PublishedAtUtc ?? DateTime.UtcNow;
                    WriteUrl(xw, loc, lastMod, "weekly", "0.7");
                }

                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush(); // MemoryStream'e yazmayı garanti altına al
            }

            ms.Position = 0;
            Response.ContentType = "application/xml; charset=utf-8";
            Response.ContentLength = ms.Length;
            await ms.CopyToAsync(Response.Body);
        }
    }


    //[HttpGet("/sitemap.xml")]
    ////[HttpGet("/sitemap.xml")]
    //public async Task SitemapXml()
    //{
    //    var baseUrl = (_cfg["Site:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}")
    //        .TrimEnd('/');

    //    var posts = await _db.Posts
    //        .AsNoTracking()
    //        .Where(p => p.Status == PostStatus.Published)
    //        .OrderByDescending(p => p.PublishedAtUtc)
    //        .Select(p => new { p.Slug, p.PublishedAtUtc })
    //        .ToListAsync();

    //    var settings = new XmlWriterSettings
    //    {
    //        Indent = true,
    //        Encoding = Encoding.UTF8,
    //        OmitXmlDeclaration = false
    //    };

    //    Response.ContentType = "application/xml; charset=utf-8";

    //    using (var xw = XmlWriter.Create(Response.Body, settings))
    //    {
    //        xw.WriteStartDocument();
    //        xw.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

    //        WriteUrl(xw, $"{baseUrl}/blog", DateTime.UtcNow, "daily", "0.8");

    //        foreach (var p in posts)
    //        {
    //            var loc = $"{baseUrl}/blog/{p.Slug}";
    //            var lastMod = p.PublishedAtUtc ?? DateTime.UtcNow;
    //            WriteUrl(xw, loc, lastMod, "weekly", "0.7");
    //        }

    //        xw.WriteEndElement();
    //        xw.WriteEndDocument();
    //    }

    //    // void method olduğu için return yok
    //}


    //public async Task<IActionResult> SitemapXml()
    //{
    //    var baseUrl = (_cfg["Site:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}")
    //        .TrimEnd('/');

    //    var posts = await _db.Posts
    //        .AsNoTracking()
    //        .Where(p => p.Status == PostStatus.Published)
    //        .OrderByDescending(p => p.PublishedAtUtc)
    //        .Select(p => new { p.Slug, p.PublishedAtUtc })
    //        .ToListAsync();

    //    var settings = new XmlWriterSettings
    //    {
    //        Indent = true,
    //        Encoding = Encoding.UTF8,
    //        OmitXmlDeclaration = false
    //    };

    //    using var sw = new StringWriter(CultureInfo.InvariantCulture);
    //    using (var xw = XmlWriter.Create(sw, settings))
    //    {
    //        xw.WriteStartDocument();
    //        xw.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

    //        // Blog index (opsiyonel ama iyi)
    //        WriteUrl(xw, $"{baseUrl}/blog", DateTime.UtcNow, changeFreq: "daily", priority: "0.8");

    //        foreach (var p in posts)
    //        {
    //            var loc = $"{baseUrl}/blog/{p.Slug}";
    //            var lastMod = p.PublishedAtUtc ?? DateTime.UtcNow;

    //            // postlar genelde "weekly" iyi bir varsayım
    //            WriteUrl(xw, loc, lastMod, changeFreq: "weekly", priority: "0.7");
    //        }

    //        xw.WriteEndElement(); // urlset
    //        xw.WriteEndDocument();
    //    }

    //    return Content(sw.ToString(), "application/xml; charset=utf-8");
    //}

    private static void WriteUrl(XmlWriter xw, string loc, DateTime lastModUtc, string changeFreq, string priority)
    {
        xw.WriteStartElement("url");
        xw.WriteElementString("loc", loc);
        xw.WriteElementString("lastmod", lastModUtc.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        xw.WriteElementString("changefreq", changeFreq);
        xw.WriteElementString("priority", priority);
        xw.WriteEndElement();
    }

    [HttpGet("/sitemap")]
    public async Task<IActionResult> SitemapHtml()
    {
        var posts = await _db.Posts
            .AsNoTracking()
            .Where(p => p.Status == PostStatus.Published)
            .OrderByDescending(p => p.PublishedAtUtc)
            .Select(p => new { p.Title, p.Slug, p.PublishedAtUtc })
            .ToListAsync();

        return View(posts);
    }
}
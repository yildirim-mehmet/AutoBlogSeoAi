using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using BlogSeoAi.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSeoAi.Services;

public interface ISlugService
{
    Task<string> GenerateUniqueSlugAsync(string title, int? postId = null);
}

public class SlugService : ISlugService
{
    private readonly AppDbContext _db;
    public SlugService(AppDbContext db) => _db = db;

    public async Task<string> GenerateUniqueSlugAsync(string title, int? postId = null)
    {
        var baseSlug = ToSlug(title);
        var slug = baseSlug;
        var i = 2;

        while (await _db.Posts.AnyAsync(p => p.Slug == slug && p.Id != (postId ?? 0)))
        {
            slug = $"{baseSlug}-{i++}";
        }

        return slug;
    }

    private static string ToSlug(string text)
    {
        text = text.Trim().ToLowerInvariant();

        // TR karakterleri normalize
        text = text
            .Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s")
            .Replace("ı", "i").Replace("ö", "o").Replace("ç", "c");

        // diacritics temizle
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark) sb.Append(c);
        }

        var clean = sb.ToString().Normalize(NormalizationForm.FormC);

        // harf/rakam dışını tirele
        clean = Regex.Replace(clean, @"[^a-z0-9\s-]", "");
        clean = Regex.Replace(clean, @"\s+", "-");
        clean = Regex.Replace(clean, @"-+", "-").Trim('-');

        return string.IsNullOrWhiteSpace(clean) ? "post" : clean;
    }
}
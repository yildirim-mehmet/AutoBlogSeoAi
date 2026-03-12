using System.ComponentModel.DataAnnotations;
using BlogSeoAi.Models;

namespace BlogSeoAi.Models;

public class PostEditVm
{
    public int? Id { get; set; }

    [Required, MaxLength(180)]
    public string Title { get; set; } = "";

    // Slug manuel girilebilir; boşsa otomatik üret
    [MaxLength(220)]
    public string? Slug { get; set; }

    [Required]
    public string ContentHtml { get; set; } = "";

    [MaxLength(300)]
    public string? Excerpt { get; set; }

    [MaxLength(180)]
    public string? SeoTitle { get; set; }

    [MaxLength(300)]
    public string? SeoDescription { get; set; }

    [MaxLength(500)]
    public string? FeaturedImageUrl { get; set; }

    public PostStatus Status { get; set; } = PostStatus.Draft;

    public DateTime? PublishedAtLocal { get; set; }
}
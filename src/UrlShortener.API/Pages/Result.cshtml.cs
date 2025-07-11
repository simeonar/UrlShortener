using Microsoft.AspNetCore.Mvc.RazorPages;

public class ResultModel : PageModel
{
    public string? ShortUrl { get; set; }
    public string? ShortCode { get; set; }

    public void OnGet(string? shortUrl, string? shortCode)
    {
        ShortUrl = shortUrl;
        ShortCode = shortCode;
    }
}

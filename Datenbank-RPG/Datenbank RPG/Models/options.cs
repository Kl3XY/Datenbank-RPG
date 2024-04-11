using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using System.Security.Principal;

namespace Datenbank_RPG.Models
{
    public class IndexModel : PageModel
    {
        public static string SessionKeyName = "_Name";
        public static string SessionKeyAge = "_Age";

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                HttpContext.Session.SetString(SessionKeyName, "Martin Gaffke");
                HttpContext.Session.SetInt32(SessionKeyAge, 73);
            }

            var name = HttpContext.Session.GetString(SessionKeyName);
            var age = HttpContext.Session.GetInt32(SessionKeyAge);

            _logger.LogInformation("Session Name: {Name}", name);
            _logger.LogInformation("Session Age: {Age}", age);
        }
    }
}

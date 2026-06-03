using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SistemaRegistros.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Login");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

public class IndexModel : PageModel
{
    private static List<ClassInformationModel> classList = new();
    private static int currentId = 1;

    [BindProperty]
    public ClassInformationModel ClassInfo { get; set; } = new();

    public List<ClassInformationModel> ClassList => classList;

    public void OnGet()
    {
    }

    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        ClassInfo.Id = currentId++;
        classList.Add(ClassInfo);
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        var item = classList.FirstOrDefault(c => c.Id == id);
        if (item != null)
        {
            classList.Remove(item);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostEdit(int id)
    {
        var item = classList.FirstOrDefault(c => c.Id == id);
        if (item != null)
        {
            ClassInfo = item;
        }
        return Page();
    }
}

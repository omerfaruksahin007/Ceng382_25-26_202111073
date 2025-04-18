using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using RAZOR_PAGE_KOPYASI.Models;

namespace RAZOR_PAGE_KOPYASI.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public string ClassNameFilter { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)] public string InstructorFilter { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)] public string CapacityFilter { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)] public string SemesterFilter { get; set; } = string.Empty;
        
        // 'Page' özelliği yerine 'CurrentPage' ismini kullanıyoruz
        [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;
        
        [BindProperty] public string SelectedColumns { get; set; } = string.Empty;

        public List<ClassInformationTable> FilteredData { get; set; } = new();
        public int TotalPages { get; set; }

        public void OnGet()
        {
            var allData = GetAllData();
            var filtered = ApplyFilters(allData);

            int pageSize = 10;
            TotalPages = (int)Math.Ceiling(filtered.Count / (double)pageSize);
            CurrentPage = CurrentPage <= 0 ? 1 : (CurrentPage > TotalPages ? TotalPages : CurrentPage);

            FilteredData = filtered.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public IActionResult OnPostExportJson()
        {
            // Filtrelenmiş veriyi alıyoruz
            var filtered = ApplyFilters(GetAllData());

            var selectedCols = string.IsNullOrWhiteSpace(SelectedColumns)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(SelectedColumns);

            bool noFilter = string.IsNullOrWhiteSpace(ClassNameFilter)
                && string.IsNullOrWhiteSpace(InstructorFilter)
                && string.IsNullOrWhiteSpace(CapacityFilter)
                && string.IsNullOrWhiteSpace(SemesterFilter);

            bool noSelection = selectedCols == null || !selectedCols.Any();

            List<Dictionary<string, object?>> exportData;

            if (noFilter && noSelection)
            {
                // **Sayfadaki 10 veri export edilir**
                exportData = FilteredData.Skip((CurrentPage - 1) * 10).Take(10).ToList()
                    .Select(item =>
                    {
                        var dict = new Dictionary<string, object?>();
                        dict["ClassName"] = item.ClassName;
                        dict["Instructor"] = item.Instructor;
                        dict["Capacity"] = item.Capacity;
                        dict["Semester"] = item.Semester;
                        return dict;
                    }).ToList();
                string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
                return File(Encoding.UTF8.GetBytes(json), "application/json", "current-page.json");
            }

            if (!noSelection)
            {
                // Seçilen sütunlarla filtreleme
                exportData = filtered.Skip((CurrentPage - 1) * 10).Take(10)  // Sayfa bazlı 10 veri
                    .Select(item =>
                    {
                        var dict = new Dictionary<string, object?>();
                        foreach (var col in selectedCols)
                        {
                            var prop = item.GetType().GetProperty(col);
                            if (prop != null)
                                dict[col] = prop.GetValue(item);
                        }
                        return dict;
                    }).ToList();
                string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
                return File(Encoding.UTF8.GetBytes(json), "application/json", "filtered.json");
            }

            // Filtrelenmiş sayfadaki verileri JSON olarak döndür
            string filteredJson = JsonSerializer.Serialize(filtered.Skip((CurrentPage - 1) * 10).Take(10).ToList(), new JsonSerializerOptions { WriteIndented = true });
            return File(Encoding.UTF8.GetBytes(filteredJson), "application/json", "filtered-page.json");
        }

        private List<ClassInformationTable> GetAllData()
        {
            var list = new List<ClassInformationTable>();
            for (int i = 1; i <= 100; i++)
            {
                list.Add(new ClassInformationTable
                {
                    Id = i,
                    ClassName = $"Class {i}",
                    Instructor = $"Instructor {(i % 10) + 1}",
                    Capacity = 10 + i,
                    Semester = $"Spring {2020 + (i % 5)}"
                });
            }
            return list;
        }

        private List<ClassInformationTable> ApplyFilters(List<ClassInformationTable> data)
        {
            return data.Where(d =>
                (string.IsNullOrWhiteSpace(ClassNameFilter) || d.ClassName.Contains(ClassNameFilter, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(InstructorFilter) || d.Instructor.Contains(InstructorFilter, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(CapacityFilter) || d.Capacity.ToString().Contains(CapacityFilter)) &&
                (string.IsNullOrWhiteSpace(SemesterFilter) || d.Semester.Contains(SemesterFilter, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
    }
}

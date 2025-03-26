using System.ComponentModel.DataAnnotations;

public class ClassInformationModel
{
    public int Id { get; set; }
    
    [Required]
    public string ClassName { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 1000, ErrorMessage = "Student count must be between 1 and 1000.")]
    public int StudentCount { get; set; }
    
    [Required]
    public string Description { get; set; } = string.Empty;
}

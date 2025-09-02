using System.ComponentModel.DataAnnotations;

namespace interview_azure_function_app.Models;


public class ApiPostNumItem
{
    [Required]
    public int Number { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace number_Minimal_API.Models;

public class NumItem
{
    public int Id { get; set; }

    [Required]
    public int Number { get; set; }

    public string? CreatedTime => DateTime.Now.ToLongTimeString();
}

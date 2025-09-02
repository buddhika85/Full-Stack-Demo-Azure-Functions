using System.ComponentModel.DataAnnotations;

namespace docker_number_Minimal_API.Models;

public class NumItem
{
    public int Id { get; set; }

    [Required]
    public int Number { get; set; }

    public string? CreatedTime => DateTime.Now.ToString();

    public string Source => "ODD Number - Azure Container Instance - Docker Hub Image";
}

using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

public class CreateClientDTO
{
    [Required]
    public string FirstName { get; set; } = null!;
    
    [Required]
    public string LastName { get; set; } = null!;
    
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Telephone { get; set; } = null!;
    
    [Required]
    public string Pesel { get; set; } = null!;
}
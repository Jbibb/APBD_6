using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace solution.DTOs;
public record ReplaceAnimalRequest (
    [Required] [MaxLength(200)] string Name,
    [MaxLength(200)] string Description,
    [Required] [MaxLength(200)] string Category,
    [Required] [MaxLength(200)] string Area
);

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mundial.Modelo;

[Table("Participacion")]
public class Participacion
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(40)]
    public string Sede { get; set; }

    [Required]
    [StringLength(40)]
    public string Año { get; set; }

    [Required]
    [StringLength(40)]
    public string Instancia { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mundial.Modelo;

[Table("Pais")]
public class Pais
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(40)]
    public string Nombre { get; set; }

    public List<Participacion> Participaciones { get; set; }

    public Pais()
    {
        Participaciones = new List<Participacion>();
    }
}

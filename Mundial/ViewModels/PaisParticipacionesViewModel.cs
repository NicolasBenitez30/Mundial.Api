namespace Mundial.ViewModels;

public class PaisParticipacionesViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public List<ParticipacionDtoViewModel> Participaciones { get; set; }
}

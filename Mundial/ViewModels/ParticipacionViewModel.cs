namespace Mundial.ViewModels;

public class ParticipacionViewModel
{
    public string Sede { get; set; }

    public int Año { get; set; }

    public string Instancia { get; set; }
}

public class ParticipacionDtoViewModel : ParticipacionViewModel
{
    public int Id { get; set; }
    public int NroInstancia { get; set; }
}
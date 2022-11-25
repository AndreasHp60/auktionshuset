namespace vareAPI;

public class Vare
{
    public int id { get; set; }

    public string? name { get; set; }

    public string? beskrivelse { get; set; }

    public double? vurdering  { get; set; }
    
   public Vare(int id, string name, string beskrivelse, double vurdering )
   {
        this.id = id;
        this.name = name;
        this.beskrivelse = beskrivelse;
        this.vurdering = vurdering;
   }

    public Vare()
    {
    }
}   



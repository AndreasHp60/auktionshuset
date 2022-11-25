using Microsoft.AspNetCore.Mvc;
using auktionsAPI;
using System.Linq;

namespace auktionsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuktionController : ControllerBase
{
private List<Vare> _vare = new List<Vare>(){
          new Vare() {  
          Id = 1, 
          Navn = "Glas", 
          Beskrivelse = "skrald 8", 
          Vurdering = 2000, 

      }
};

    private readonly ILogger<AuktionController> _ilogger;

    public AuktionController(ILogger<AuktionController> logger)
    {
        _ilogger = logger;
    }

  [HttpGet(Name = "GetvareById")] 
  public Vare Get(int vareId) 
  { 
      return _vare.Where(c => c.Id == vareId).First(); 
  } 
}

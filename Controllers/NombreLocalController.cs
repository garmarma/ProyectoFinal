using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Controllers.DTOS;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class NombreLocalController : ControllerBase
    {


        [HttpGet(Name = "TraerNombreDelLocal")]
        public string TraerNombreDelLocal()
        {
            return "Tienda Lo de Pepe";
        }
    }
}

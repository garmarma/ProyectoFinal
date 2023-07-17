using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Controllers.DTOS;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class LoginController : ControllerBase
    {


        [HttpGet(Name = "InicioDeSesion")]
        public Usuario InicioDeSesion(string nombreUsuario, string contraseña)
        {
            return UsuarioHandler.InicioDeSesion(nombreUsuario, contraseña);
        }
    }
}

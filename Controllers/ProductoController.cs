using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Controllers.DTOS;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductoController : ControllerBase
    {
        [HttpGet(Name = "TraerProductos")]
        public List<Producto> TraerProductos()
        {
            return ProductoHandler.TraerProductos();
        }
        [HttpPut(Name = "ModificarProducto")]

        public bool ModificarProducto([FromBody] PutProducto producto)
        {
            try
            {
                return ProductoHandler.ModificarProducto(
                    new Producto
                    {
                        Id = producto.Id,
                        Descripciones = producto.Descripciones,
                        Costo = producto.Costo,
                        PrecioVenta = producto.PrecioVenta,
                        Stock = producto.Stock,
                        IdUsuario = producto.IdUsuario
                    }
                );
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpPost(Name = "CrearProducto")]
        public bool CrearProducto([FromBody] PostProducto producto)
        {
            try
            {
                return ProductoHandler.CrearProducto(
                    new Producto
                    {
                        Descripciones = producto.Descripciones,
                        Costo = producto.Costo,
                        PrecioVenta = producto.PrecioVenta,
                        Stock = producto.Stock,
                        IdUsuario = producto.IdUsuario
                    }
                );
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpDelete]

        public bool EliminarProducto([FromBody] long idProducto)
        {
            try
            {
                ProductoVendidoHandler.EliminarProductoVendido(idProducto);
                return ProductoHandler.EliminarProducto(idProducto);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

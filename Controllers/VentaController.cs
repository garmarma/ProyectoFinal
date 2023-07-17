using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Controllers.DTOS;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class VentaController : ControllerBase
    {


        [HttpGet(Name = "TraerVentas")]
        public List<ProductoVendido> TraerVentas()
        {
            return VentaHandler.TraerVentas();
        }
        [HttpPost(Name = "CargarVenta")]
        public bool CargarVenta([FromBody] List<PostVenta> listaDeProductosVendidos)
        {
            Producto producto = new Producto();
            Usuario usuario = new Usuario();
            foreach (PostVenta item in listaDeProductosVendidos)
            {
                producto = ProductoHandler.TraerProducto_conId(item.Id);
                if (producto.Id <= 0)
                {
                    return false;
                }

                if (item.Stock <= 0)
                {
                    return false;
                }

                if (producto.Stock < item.Stock)
                {
                    return false;
                }

                usuario = UsuarioHandler.TraerUsuario_conId(item.IdUsuario);
                if (usuario.Id <= 0)
                {
                    return false;
                }
            }
            Venta venta = new Venta();
            long idVenta = VentaHandler.CargarVenta(venta);
            if (idVenta >= 0)
            {
                List<ProductoVendido> productosVendidos = new List<ProductoVendido>();
                foreach (PostVenta item in listaDeProductosVendidos)
                {
                    ProductoVendido productoVendido = new ProductoVendido();
                    productoVendido.IdProducto = item.Id;
                    productoVendido.Stock = item.Stock;
                    productoVendido.IdVenta = idVenta;
                    productosVendidos.Add(productoVendido);
                }
                if (ProductoVendidoHandler.CargarProductosVendidos(productosVendidos))
                {
                    bool resultado = false;
                    foreach (ProductoVendido item in productosVendidos)
                    {
                        producto.Id = item.IdProducto;
                        producto = ProductoHandler.ConsultarStock(producto);
                        producto.Stock = producto.Stock - item.Stock;
                        resultado = ProductoHandler.ActualizarStock(producto);
                        if (resultado == false)
                        {
                            break;
                        }
                    }
                    return resultado;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        [HttpDelete(Name = "EliminarVenta")]
        public bool EliminarVenta([FromBody] long idVenta)
        {
            bool resultado = false;
            if (idVenta <= 0)
            {
                return false;
            }
            List<ProductoVendido> productosVendidosDeLaVenta = new List<ProductoVendido>();
            productosVendidosDeLaVenta = ProductoVendidoHandler.TraerProductosVendidos_conIdVenta(idVenta);
            if (ProductoVendidoHandler.EliminarProductoVendido_conIdVenta(idVenta))
            {
                Producto producto = new Producto();
                foreach (ProductoVendido item in productosVendidosDeLaVenta)
                {
                    producto.Id = item.IdProducto;
                    producto = ProductoHandler.ConsultarStock(producto);
                    producto.Stock = producto.Stock + item.Stock;
                    resultado = ProductoHandler.ActualizarStock(producto);
                    if (resultado == false)
                    {
                        return false;
                    }
                }

                if (VentaHandler.EliminarVenta(idVenta))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

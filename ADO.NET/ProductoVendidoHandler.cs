

using System.Data.SqlClient;
using System.Data;

namespace ProyectoFinal
{

    public static class ProductoVendidoHandler
    {

        public const string connectionString = @"Server=MAIA\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True;";
        public static List<ProductoVendido> TraerProductosVendidos_conIdUsuario(long idUsuario)
        {
            List<ProductoVendido> productosVendidosPorUsuario = new List<ProductoVendido>();
            Usuario usuarioEnBD = UsuarioHandler.TraerUsuario_conId(idUsuario);
            if (usuarioEnBD.Id == 0)
            {
                return productosVendidosPorUsuario;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string query = "SELECT pv.Id, pv.Stock, pv.IdProducto, pv.IdVenta " +
                                        "FROM[SistemaGestion].[dbo].[ProductoVendido] AS pv " +
                                        "INNER JOIN Producto p ON p.Id = pv.IdProducto " +
                                        "WHERE p.IdUsuario = @idUsuario";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    var sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "idUsuario";
                    sqlParameter.SqlDbType = SqlDbType.BigInt;
                    sqlParameter.Value = idUsuario;
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.Id = Convert.ToInt64(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt64(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt64(dataReader["Idventa"]);

                                productosVendidosPorUsuario.Add(productoVendido);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productosVendidosPorUsuario;
        }
        public static List<ProductoVendido> TraerProductosVendidos_conIdVenta(long idVenta)
        {
            List<ProductoVendido> productosVendidos = new List<ProductoVendido>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string query = "SELECT * FROM[SistemaGestion].[dbo].[ProductoVendido] " +
                                        "WHERE IdVenta = @idVenta";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    var sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "idVenta";
                    sqlParameter.SqlDbType = SqlDbType.BigInt;
                    sqlParameter.Value = idVenta;
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.Id = Convert.ToInt64(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt64(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt64(dataReader["Idventa"]);

                                productosVendidos.Add(productoVendido);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productosVendidos;
        }
        public static bool CargarProductosVendidos(List<ProductoVendido> productosVendidos)
        {
            bool resultado = false;
            long idProductoVendido = 0;
            int elementosEnLaLista = 0;
            int idValidoEncontrado = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[ProductoVendido] (IdProducto, Stock, IdVenta) " +
                                        "VALUES (@idProducto, @stock, @idventa) " +
                                        "SELECT @@IDENTITY";

                var parameterIdProducto = new SqlParameter("idProducto", SqlDbType.BigInt) { Value = 0 };
                var parameterStock = new SqlParameter("stock", SqlDbType.Int) { Value = 0 };
                var parameterIdUsuario = new SqlParameter("idVenta", SqlDbType.BigInt) { Value = 0 };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterIdProducto);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);

                    foreach (ProductoVendido item in productosVendidos)
                    {
                        parameterIdProducto.Value = item.IdProducto;
                        parameterStock.Value = item.Stock;
                        parameterIdUsuario.Value = item.IdVenta;
                        elementosEnLaLista++;
                        idProductoVendido = Convert.ToInt64(sqlCommand.ExecuteScalar());
                        if (idProductoVendido > 0)
                        {
                            idValidoEncontrado++;
                        }
                    }
                }
                sqlConnection.Close();
            }
            if (idValidoEncontrado == elementosEnLaLista)
            {
                resultado = true;
            }
            return resultado;
        }
        public static bool EliminarProductoVendido(long idProducto)
        {
            bool resultado = false;
            int rowsAffected = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryUpdate = "DELETE FROM [SistemaGestion].[dbo].[ProductoVendido] " +
                                        "WHERE IdProducto = @idProducto";

                var parameterIdProducto = new SqlParameter("idProducto", SqlDbType.BigInt);
                parameterIdProducto.Value = idProducto;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterIdProducto);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            if (rowsAffected >= 1)
            {
                resultado = true;
            }
            return resultado;
        }
        public static bool EliminarProductoVendido_conIdVenta(long idVenta)
        {
            bool resultado = false;
            int rowsAffected = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[ProductoVendido] " +
                                        "WHERE IdVenta = @idventa";

                var parameterIdVenta = new SqlParameter("idVenta", SqlDbType.BigInt);
                parameterIdVenta.Value = idVenta;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterIdVenta);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            if (rowsAffected >= 1)
            {
                resultado = true;
            }
            return resultado;
        }
        public static void MostrarProductosVendidos(List<ProductoVendido> productosVendidos)
        {
            Console.WriteLine("ProductoVendido en BD:");
            foreach (ProductoVendido item in productosVendidos)
            {
                Console.WriteLine("id: " + item.Id.ToString() +
                                    "\tidProducto: " + item.IdProducto.ToString() +
                                    "\tstock: " + item.Stock.ToString() +
                                    "\tidVenta: " + item.IdVenta.ToString());
            }
            Console.WriteLine(" ");
        }
    }
}
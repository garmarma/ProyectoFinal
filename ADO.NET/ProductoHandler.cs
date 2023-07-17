

using System.Data.SqlClient;
using System.Data;

namespace ProyectoFinal
{

    public static class ProductoHandler
    {
        public const string connectionString = @"Server=MAIA\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True;";
        private static Producto InicializarProductoDesdeBD(SqlDataReader dataReader)
        {
            Producto nuevoProducto = new Producto(
                                            Convert.ToInt64(dataReader["Id"]),
                                            dataReader["Descripciones"].ToString(),
                                            Convert.ToDouble(dataReader["Costo"]),
                                            Convert.ToDouble(dataReader["PrecioVenta"]),
                                            Convert.ToInt32(dataReader["Stock"]),
                                            Convert.ToInt64(dataReader["IdUsuario"]));
            return nuevoProducto;
        }
        public static List<Producto> TraerProductos()
        {
            List<Producto> productosEnBD = new List<Producto>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string querySelect = "SELECT * FROM [SistemaGestion].[dbo].[Producto]";

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                            {
                                Producto producto = InicializarProductoDesdeBD(sqlDataReader);

                                productosEnBD.Add(producto);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productosEnBD;
        }
        public static List<Producto> TraerProductos_conIdUsuario(long idUsuario)
        {
            List<Producto> productosConIdUsuario = new List<Producto>();
            if (idUsuario <= 0)
            {
                return productosConIdUsuario;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string querySelect = "SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE IdUsuario = @idUsuario";

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection))
                {
                    var sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "idUsuario";
                    sqlParameter.SqlDbType = SqlDbType.BigInt;
                    sqlParameter.Value = idUsuario;
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlConnection.Open();

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                            {
                                Producto producto = InicializarProductoDesdeBD(sqlDataReader);

                                productosConIdUsuario.Add(producto);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productosConIdUsuario;
        }
        public static Producto ConsultarStock(Producto producto)
        {
            if (producto.Id <= 0)
            {
                return producto;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string querySelect = "SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @id";

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection))
                {
                    var sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "id";
                    sqlParameter.SqlDbType = SqlDbType.BigInt;
                    sqlParameter.Value = producto.Id;
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlConnection.Open();

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows & sqlDataReader.Read())
                        {
                            producto.Stock = Convert.ToInt32(sqlDataReader["Stock"]);
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return producto;
        }
        public static bool ActualizarStock(Producto producto)
        {
            if (producto.Id <= 0)
            {
                return false;
            }

            bool resultado = false;
            int rowsAffected = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Producto] " +
                                        "SET Stock = @stock " +
                                        "WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt);
                parameterId.Value = producto.Id;

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.Stock;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterId);
                    sqlCommand.Parameters.Add(parameterStock);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            if (rowsAffected == 1)
            {
                resultado = true;
            }
            return resultado;
        }
        public static bool CrearProducto(Producto producto)
        {
            bool resultado = false;
            long idProducto = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Producto] (Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                                        "VALUES (@descripciones, @costo, @precioVenta, @stock, @idUsuario) " +
                                        "SELECT @@IDENTITY";
                var parameterDescripciones = new SqlParameter("descripciones", SqlDbType.VarChar);
                parameterDescripciones.Value = producto.Descripciones;

                var parameterCosto = new SqlParameter("costo", SqlDbType.Money);
                parameterCosto.Value = producto.Costo;

                var parameterPrecioVenta = new SqlParameter("precioVenta", SqlDbType.Money);
                parameterPrecioVenta.Value = producto.PrecioVenta;

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.Stock;

                var parameterIdUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt);
                parameterIdUsuario.Value = producto.IdUsuario;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterDescripciones);
                    sqlCommand.Parameters.Add(parameterCosto);
                    sqlCommand.Parameters.Add(parameterPrecioVenta);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);
                    idProducto = Convert.ToInt64(sqlCommand.ExecuteScalar());
                }
                sqlConnection.Close();
            }
            if (idProducto != 0)
            {
                resultado = true;
            }
            return resultado;
        }
        public static bool ModificarProducto(Producto producto)
        {
            bool resultado = false;
            int rowsAffected = 0;
            if (producto.Id <= 0)
            {
                return false;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Producto] " +
                                        "SET Descripciones = @descripciones, " +
                                        "Costo = @costo, " +
                                        "PrecioVenta = @precioVenta, " +
                                        "stock = @Stock, " +
                                        "IdUsuario = @idUsuario " +
                                        "WHERE Id = @id";
                var parameterId = new SqlParameter("id", SqlDbType.BigInt);
                parameterId.Value = producto.Id;

                var parameterDescripciones = new SqlParameter("descripciones", SqlDbType.VarChar);
                parameterDescripciones.Value = producto.Descripciones;

                var parameterCosto = new SqlParameter("costo", SqlDbType.Money);
                parameterCosto.Value = producto.Costo;

                var parameterPrecioVenta = new SqlParameter("precioVenta", SqlDbType.Money);
                parameterPrecioVenta.Value = producto.PrecioVenta;

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.Stock;

                var parameterIdUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt);
                parameterIdUsuario.Value = producto.IdUsuario;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterId);
                    sqlCommand.Parameters.Add(parameterDescripciones);
                    sqlCommand.Parameters.Add(parameterCosto);
                    sqlCommand.Parameters.Add(parameterPrecioVenta);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            if (rowsAffected == 1)
            {
                resultado = true;
            }
            return resultado;
        }
        public static bool EliminarProducto(long id)
        {
            if (id <= 0)
            {
                return false;
            }

            bool resultado = false;
            int rowsAffected = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Producto] " +
                                        "WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt);
                parameterId.Value = id;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterId);
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            if (rowsAffected == 1)
            {
                resultado = true;
            }
            return resultado;
        }
        public static Producto TraerProducto_conId(long id)
        {
            Producto producto = new Producto();
            if (id <= 0)
            {
                return producto;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string queryGet = "SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @id";

                using (SqlCommand sqlCommand = new SqlCommand(queryGet, sqlConnection))
                {
                    var sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "id";
                    sqlParameter.SqlDbType = SqlDbType.BigInt;
                    sqlParameter.Value = id;
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlConnection.Open();

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows & sqlDataReader.Read())
                        {
                            producto = InicializarProductoDesdeBD(sqlDataReader);
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return producto;
        }
        public static void MostrarProductos(List<Producto> productosEnBd)
        {
            Console.WriteLine("\nProducto en BD: ");
            foreach (Producto item in productosEnBd)
            {
                Console.WriteLine("id: " + item.Id.ToString() +
                                    "\tDescripcion: " + item.Descripciones +
                                    "\tCosto: " + item.Costo.ToString() +
                                    "\tPrecioVenta: " + item.PrecioVenta.ToString() +
                                    "\tStock: " + item.Stock.ToString() +
                                    "\tIdUsuario: " + item.IdUsuario.ToString());
            }
            Console.WriteLine(" ");
        }
    }
}

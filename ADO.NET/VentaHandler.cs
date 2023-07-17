

using System.Data.SqlClient;
using System.Data;

namespace ProyectoFinal
{

    public static class VentaHandler// Clase encargada de proporcionar los métodos necesarios para manipular los objetos de la clase "Venta"
    {

        public const string connectionString = @"Server=MAIA\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True;";
        public static List<Venta> TraerVentas_conIdUsuario(long idUsuario)
        {
            List<Venta> ventas = new List<Venta>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string query = "SELECT v.Id, v.Comentarios " +
                                        "FROM Venta AS v " +
                                        "INNER JOIN ProductoVendido AS pv " +
                                        "ON v.Id = pv.IdVenta " +
                                        "INNER JOIN Producto AS p " +
                                        "ON pv.IdProducto = p.Id " +
                                        "WHERE p.IdUsuario = @idUsuario ";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    var parameterIdUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt);
                    parameterIdUsuario.Value = idUsuario;
                    sqlCommand.Parameters.Add(parameterIdUsuario);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();

                                venta.Id = Convert.ToInt32(dataReader["Id"]);
                                venta.Comentarios = dataReader["Comentarios"].ToString();

                                ventas.Add(venta);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return ventas;
        }
        public static long CargarVenta(Venta venta)
        {
            bool resultado = false;
            long idVenta = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Venta] (Comentarios) " +
                                        "VALUES (@comentarios) " +
                                        "SELECT @@IDENTITY";

                var parameterComentarios = new SqlParameter("comentarios", SqlDbType.VarChar);
                parameterComentarios.Value = venta.Comentarios;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterComentarios);
                    idVenta = Convert.ToInt64(sqlCommand.ExecuteScalar());
                }
                sqlConnection.Close();
            }
            return idVenta;
        }
        public static bool EliminarVenta(long id)
        {
            bool resultado = false;
            int rowsAffected = 0;

            if (id <= 0)
            {
                return false;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Venta] WHERE Id = @id";

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
        public static List<ProductoVendido> TraerVentas()
        {
            List<ProductoVendido> productosVendidos = new List<ProductoVendido>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                const string query = "SELECT pv.Id, pv.Stock, pv.IdProducto, pv.IdVenta " +
                                        "FROM Venta AS v " +
                                        "INNER JOIN ProductoVendido AS pv " +
                                        "ON v.Id = pv.IdVenta ";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
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
        public static void MostrarVentas(List<Venta> ventas)
        {
            Console.WriteLine("Venta en BD:");
            foreach (Venta item in ventas)
            {
                Console.WriteLine("id: " + item.Id.ToString() +
                                    "\tcomentarios: " + item.Comentarios.ToString());
            }
            Console.WriteLine(" ");
        }
    }
}
namespace ProyectoFinal
{
    public class ProductoVendido
    {
        private long _Id;
        private long _IdProducto;
        private int _Stock;
        private long _IdVenta;
        public ProductoVendido()
        {
            this._Id = 0;
            this._IdProducto = 0;
            this._Stock = 0;
            this._IdVenta = 0;
        }
        public ProductoVendido(long id, long idProducto, int stock, long idVenta)
        {
            this._Id = id;
            this._IdProducto = idProducto;
            this._Stock = stock;
            this._IdVenta = idVenta;
        }
        public long Id { get { return _Id; } set { _Id = value; } }
        public long IdProducto { get { return _IdProducto; } set { _IdProducto = value; } }
        public int Stock { get { return _Stock; } set { _Stock = value; } }
        public long IdVenta { get { return _IdVenta; } set { _IdVenta = value; } }
        public void verAtributos()
        {
            Console.WriteLine("\nProductoVendido:" +
                                "\n   id = " + _Id.ToString() +
                                "\n   idProducto = " + _IdProducto.ToString() +
                                "\n   stock = " + _Stock.ToString() +
                                "\n   idVenta = " + _IdVenta.ToString() +
                                "\n");
        }
    }
}

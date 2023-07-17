namespace ProyectoFinal
{
    public class Usuario
    {
        private long _Id;
        private string _Nombre;
        private string _Apellido;
        private string _NombreUsuario;
        private string _Contraseña;
        private string _Mail;
        public Usuario()
        {
            this._Id = 0;
            this._Nombre = String.Empty;
            this._Apellido = String.Empty;
            this._NombreUsuario = String.Empty;
            this._Contraseña = String.Empty;
            this._Mail = String.Empty;
        }
        public Usuario(long id, string nombre, string apellido, string nombreUsuario, string contraseña, string mail)
        {
            this._Id = id;
            this._Nombre = nombre;
            this._Apellido = apellido;
            this._NombreUsuario = nombreUsuario;
            this._Contraseña = contraseña;
            this._Mail = mail;
        }
        public long Id { get { return _Id; } set { _Id = value; } }
        public string Nombre { get { return _Nombre; } set { _Nombre = value; } }
        public string Apellido { get { return _Apellido; } set { _Apellido = value; } }
        public string NombreUsuario { get { return _NombreUsuario; } set { _NombreUsuario = value; } }
        public string Contraseña { get { return _Contraseña; } set { _Contraseña = value; } }
        public string Mail { get { return _Mail; } set { _Mail = value; } }
        public void verAtributos()
        {
            Console.WriteLine("\nUsuario" +
                                "\n   id = " + _Id.ToString() +
                                "\n   nombre = " + _Nombre +
                                "\n   apellido = " + _Apellido +
                                "\n   nombreUsuario = " + _NombreUsuario +
                                "\n   constraseña = " + _Contraseña +
                                "\n   mail = " + _Mail +
                                "\n");
        }
    }
}

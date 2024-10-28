
using AccesoDatos.DBContext;
using AccesoDatos.Interfaz;
using Entidades.Models;
using MetodosComunes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;



namespace AccesoDatos.Implementacion
{

    public class ClienteAD : IClienteAD
    {

        public Excepciones gObjExcepciones = new Excepciones();

        public SqlCommandAbirCerrar gObjSqlCommandAbrirCerrar = new SqlCommandAbirCerrar();

        private readonly IConfiguration _configuration;

        public ClienteAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Cliente> RecCliente()
        {
            List<Cliente> lObjRespuesta = new List<Cliente>();
            try
            {
                using (LoteriaContext lObjCnn = new LoteriaContext(_configuration))
                {

                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lObjCnn, "RecClientePA");
                    var lReader = lCmd.ExecuteReader();
                    while (lReader.Read())
                    {
                        Cliente lobjDatosCliente = new Cliente();
                        lobjDatosCliente.IdCliente = Convert.ToInt32(lReader["idCliente"]);
                        lobjDatosCliente.Cedula = Convert.ToInt32(lReader["cedula"]);
                        lobjDatosCliente.Nombre = lReader["nombre"]?.ToString() ?? string.Empty;
                        lobjDatosCliente.Email = lReader["email"]?.ToString() ?? string.Empty;
                        lobjDatosCliente.Telefono = lReader["telefono"]?.ToString() ?? string.Empty; 
                        lobjDatosCliente.FechaCreacion = Convert.ToDateTime(lReader["fechaCreacion"]);
                        lobjDatosCliente.FechaBorrado = Convert.ToDateTime(lReader["fechaBorrado"]);
                        lobjDatosCliente.Bloqueado = Convert.ToInt32(lReader["bloqueado"]);
                        lobjDatosCliente.NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty;
                        lobjDatosCliente.Clave = lReader["clave"]?.ToString() ?? string.Empty;

                        lObjRespuesta.Add(lobjDatosCliente);
                    }
                    gObjSqlCommandAbrirCerrar.CerrarConexion(lCmd);
                }
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
            return lObjRespuesta;
        }

        public Cliente? RecClienteXId(int pIdCliente)
        {
            Cliente? lObjRespuesta = null;  // Inicializamos como null
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "RecClienteXIdPA");
                    lCmd.Parameters.Add(new SqlParameter("@idCliente", pIdCliente));
                    var lReader = lCmd.ExecuteReader();

                    // Si hay filas en el reader, creamos un nuevo objeto Cliente
                    if (lReader.Read())
                    {
                        lObjRespuesta = new Cliente
                        {
                            IdCliente = Convert.ToInt32(lReader["idCliente"]),
                            Cedula = Convert.ToInt32(lReader["cedula"]),
                            Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                            Email = lReader["email"]?.ToString() ?? string.Empty,
                            Telefono = lReader["telefono"]?.ToString() ?? string.Empty,
                            FechaCreacion = Convert.ToDateTime(lReader["fechaCreacion"]),
                            FechaBorrado = Convert.ToDateTime(lReader["fechaBorrado"]),
                            Bloqueado = Convert.ToInt32(lReader["bloqueado"]),
                            NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty,
                            Clave = lReader["clave"]?.ToString() ?? string.Empty,
                        };
                    }
                    gObjSqlCommandAbrirCerrar.CerrarConexion(lCmd);
                }
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                // Lanza la excepción para que la maneje la capa superior
                throw;
            }

            // Si no se encontró ningún registro, lObjRespuesta seguirá siendo null
            return lObjRespuesta;
        }

        public bool InsCliente(Cliente pCliente)
        {
            return EjecutarProcedimiento("insClientePA", pCliente);
        }

        public bool ModCliente(Cliente pCliente)
        {
            return EjecutarProcedimiento("modClientePA", pCliente);
        }

        public bool DelCliente(Cliente pCliente)
        {
            return EjecutarProcedimiento("delClientePA", pCliente);
        }

        // Método auxiliar para insertar, modificar o eliminar un cliente
        private bool EjecutarProcedimiento(string procedimientoAlmacenado, Cliente pCliente)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, procedimientoAlmacenado);

                    if (procedimientoAlmacenado == "delClientePA")
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idCliente", pCliente.IdCliente));
                    }
                    else
                    {
                        lCmd.Parameters.Add(new SqlParameter("@idCliente", pCliente.IdCliente));
                        lCmd.Parameters.Add(new SqlParameter("@cedula", pCliente.Cedula));
                        lCmd.Parameters.Add(new SqlParameter("@nombre", pCliente.Nombre));
                        lCmd.Parameters.Add(new SqlParameter("@email", pCliente.Email));
                        lCmd.Parameters.Add(new SqlParameter("@telefono", pCliente.Telefono));
                        lCmd.Parameters.Add(new SqlParameter("@fechaCreacion", pCliente.FechaCreacion));
                        lCmd.Parameters.Add(new SqlParameter("@fechaBorrado", pCliente.FechaBorrado));
                        lCmd.Parameters.Add(new SqlParameter("@bloqueado", pCliente.Bloqueado));
                        lCmd.Parameters.Add(new SqlParameter("@nombreUsuario", pCliente.NombreUsuario));
                        lCmd.Parameters.Add(new SqlParameter("@clave", pCliente.Clave));

                        // Solo para inserciones de cliente, se añade la clave
                        if (procedimientoAlmacenado == "insClientePA")
                        {
                            lCmd.Parameters.Add(new SqlParameter("@clave", pCliente.Clave));
                        }
                    }

                    if (lCmd.ExecuteNonQuery() > 0)
                    {
                        lObjRespuesta = true;
                    }

                    gObjSqlCommandAbrirCerrar.CerrarConexion(lCmd);
                }
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                throw;
            }
            return lObjRespuesta;
        }


        public Cliente? ValidarLoginCliente(int pId, string pClave)
        {
            Cliente? lObjRespuesta = null;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "ValidarLoginClientePA");
                    lCmd.Parameters.Add(new SqlParameter("@idCliente", pId));
                    lCmd.Parameters.Add(new SqlParameter("@clave", pClave));

                    using (var lReader = lCmd.ExecuteReader())
                    {
                        // Si hay filas, significa que la consulta devolvió un resultado
                        if (lReader.Read())
                        {
                            lObjRespuesta = new Cliente
                            {
                                IdCliente = Convert.ToInt32(lReader["idCliente"]),
                                Cedula = Convert.ToInt32(lReader["cedula"]),
                                Nombre = lReader["nombre"]?.ToString() ?? string.Empty,
                                Email = lReader["email"]?.ToString() ?? string.Empty,
                                Telefono = lReader["telefono"]?.ToString() ?? string.Empty,
                                FechaCreacion = Convert.ToDateTime(lReader["fechaCreacion"]),
                                FechaBorrado = Convert.ToDateTime(lReader["fechaBorrado"]),
                                Bloqueado = Convert.ToInt32(lReader["bloqueado"]),
                                NombreUsuario = lReader["nombreUsuario"]?.ToString() ?? string.Empty,
                                Clave = lReader["clave"]?.ToString() ?? string.Empty,
                            };
                        }
                    }
                    gObjSqlCommandAbrirCerrar.CerrarConexion(lCmd);
                }
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
            return lObjRespuesta;
        }


        public bool ModClaveCliente(int pId, string pClave)
        {
            bool lObjRespuesta = false;
            try
            {
                using (LoteriaContext lobjCnn = new LoteriaContext(_configuration))
                {
                    var lCmd = gObjSqlCommandAbrirCerrar.CrearComando(lobjCnn, "ModClaveClientePA");
                    lCmd.Parameters.Add(new SqlParameter("@idCliente", pId));
                    lCmd.Parameters.Add(new SqlParameter("@clave", pClave));

                    if (lCmd.ExecuteNonQuery() > 0)
                    {
                        lObjRespuesta = true;
                    }
                    gObjSqlCommandAbrirCerrar.CerrarConexion(lCmd);
                }
            }
            catch (Exception lEx)
            {
                gObjExcepciones.LogError(lEx);
                // Lanza la excepción para que la maneje la capa superior
                throw;
            }
            return lObjRespuesta;
        }
    }
}

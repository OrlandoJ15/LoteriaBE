using NLog;

namespace MetodosComunes
{
    public class Excepciones
    {
        private readonly Logger gObjError = LogManager.GetCurrentClassLogger();

        public void LogError(Exception lEx)
        {
            // Obtener el nombre del método actual de forma segura
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            string methodName = methodInfo?.ToString() ?? "Método no disponible";

            // Registrar el error
            gObjError.Error("SE HA PRODUCIDO UN ERROR. Detalle: " + lEx.Message +
                            "// " + (lEx.InnerException?.Message ?? "No Inner Exception") +
                            ". Método: " + methodName);
        }


    }
}

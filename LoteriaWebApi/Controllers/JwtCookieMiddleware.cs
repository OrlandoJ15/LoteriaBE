namespace LoteriaWebApi.Controllers
{
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verificar si el token está presente en la cookie
            if (context.Request.Cookies.ContainsKey("AuthToken"))
            {
                var token = context.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    // Añadir el token al encabezado Authorization
                    context.Request.Headers["Authorization"] = "Bearer " + token;
                }
            }

            // Llamamos al siguiente middleware
            await _next(context);
        }

    }
}

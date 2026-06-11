namespace Api_DataPeople.Validacion
{
    public class CsrfMiddleware
    {
        private readonly RequestDelegate _next;

        public CsrfMiddleware(RequestDelegate requestDelegate) { 
            _next = requestDelegate;
        }


        public async Task Invoke(HttpContext context) {
            if (
                HttpMethods.IsPost(context.Request.Method) ||
                HttpMethods.IsPut(context.Request.Method) ||
                HttpMethods.IsDelete(context.Request.Method)
                ) {

                var origin = context.Request.Headers["Origin"].ToString();

                if (
                    !string.IsNullOrEmpty(origin) &&
                    origin != "https://datospr.cedesystem.com" &&
                    origin != "http://localhost:4321" &&
                    origin != "https://localhost:44300"
                    ) {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("No compa, su peticion de este origen no esta permitido");
                    return;
                }
            }
            await _next(context);
        }
    }
}

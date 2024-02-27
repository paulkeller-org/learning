using System.Net;

namespace DishesAPI.Extensions
{
    public static class EndpointWebApplicationBuilderExtensions
   {
        public static WebApplication RegisterApiExceptionHander(this WebApplication app)
        {            
            // Only add the handler when we're not working in development 
            if (app.Environment.IsDevelopment())
            {
                return app;
            }

            app.UseExceptionHandler();

            //app.UseExceptionHandler(configureApplicationBuilder => {                
            //    configureApplicationBuilder.Run(
            //        async context =>
            //        {
            //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //            context.Response.ContentType = "text/html";
            //            await context.Response.WriteAsync("An unexpected problem happend.");
            //        });
            //});
            return app;


        }
    }
}

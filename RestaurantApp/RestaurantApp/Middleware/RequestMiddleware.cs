using System.Text;

namespace RestaurantApp.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;


  public RequestMiddleware(RequestDelegate next)
        {
            this._next = next;

  }

        public async Task Invoke(HttpContext context)
        {
           
            try
            {
                await this._next.Invoke(context).ConfigureAwait(false);
            }
            catch(Exception e)
            {

            }
              
        }


    }
}

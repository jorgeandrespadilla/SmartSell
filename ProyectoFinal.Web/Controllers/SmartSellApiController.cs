using System.Web.Http;

namespace ProyectoFinal.Web.Controllers
{
    public class SmartSellApiController : ApiController
    {

        [HttpGet]
        [ActionName("Usuarios")]
        public IHttpActionResult Test()
        {
            return Ok(new
            {
                x = 4,
                y = "yolo"
            });
        }
    }
}

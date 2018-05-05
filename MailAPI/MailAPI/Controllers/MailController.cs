using System.Net;
using System.Net.Http;
using System.Web.Http;
using MailAPI.Models;
using System.Data;

/// <summary>
/// Clase que se encarga de recibir la solicitud de enviar un correo nuevo
/// </summary>
namespace MailAPI.Controllers
{
    public class MailController : ApiController
    {

        //Bandera que se activa cuando se recibe la solicitud de enviar un correo nuevo, esto con el fin de 
        //evitar consultas a la base de datos cuando no se reciben solicitudes de enviar correos. (usada en WebApiConfig.cs)
        public static bool bandera = true;

        /// <summary>
        /// Recibe la solicitud de enviar un nuevo correo
        /// </summary>
        /// <param name="email">Json con el formato {EmailOrigen: "origen", EmailDestino: "destino", Tema: "Asunto del correo", 
        /// Mensaje: "mensaje a enviar", Prioridad:int(mayor numero mayor prioridad)}</param>
        /// <returns>HttpStatusCode.OK si se guardo correctamente la solicitud, HttpStatusCode.Conflict si no se pudo guardar</returns>
        [HttpPost]
        [Route("nuevoEmail")]
        public HttpResponseMessage NuevoEmail(MENSAJENUEVO email)
        {
            using (NotificacionesEntities entities = new NotificacionesEntities())
            {
                if (ModelState.IsValid){
                    try{
                        entities.NewMessage(email.EmailOrigen, email.EmailDestino, email.Tema,
                            email.Mensaje, 0, email.Prioridad);//Almacena la solicitud
                        bandera = true;
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }catch (DataException) { return Request.CreateResponse(HttpStatusCode.Conflict); }
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
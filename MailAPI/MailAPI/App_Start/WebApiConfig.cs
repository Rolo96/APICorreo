using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Timers;
using System.Web.Http;
using MailAPI.Controllers;
using MailAPI.Models;

/// <summary>
/// Inicializa el API, pone a correr el timer que revisa en background si hay mensajes nuevos y los envía
/// </summary>
namespace MailAPI
{
    public static class WebApiConfig
    {
        static SmtpClient server = new SmtpClient("smtp.gmail.com", 587);//SmtpGmail

        /// <summary>
        /// Constructor del API, inicializa el timer, configura el smtp
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            //Configuracion del smtp con la informacion del correo a utilizar
            server.Credentials = new System.Net.NetworkCredential("notificacioneshb.ce@gmail.com", "hbCE2018");
            server.EnableSsl = true;

            //Creacion y configuracion del timer
            Timer aTimer = new Timer(60 * 1000);//Tiempo en milisegundos para volver a ejecutar la tarea
            aTimer.Elapsed += new ElapsedEventHandler(EnviarCorreos);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            //Configuracion general del API
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        /// <summary>
        /// Busca si hay correos a enviar y los envia por prioridad, si falla el envío
        /// lo intenta hasta 5 veces si no lo puede enviar lo marca como fallido
        /// </summary>
        public static void EnviarCorreos(object source, ElapsedEventArgs e)
        {
            using (NotificacionesEntities entities = new NotificacionesEntities())
            {
                while (MailController.bandera){//Mientras hayan correos a enviar
                    try
                    {
                        var mails = entities.MENSAJENUEVO.Select(p => new { p.EmailDestino, p.EmailOrigen, p.Tema, p.Mensaje, p.Prioridad, p.Intentos, p.Id }).
                            OrderByDescending((x) => x.Prioridad).ToList();//Selecciona los correos a enviar ordenados por prioridad
                        if (mails.Count < 1) {MailController.bandera = false; break;}//Si no hay correos a enviar se detiene
                        var mail = mails.First();//Obtiene el mail mas prioritario
                        try{
                            server.Send(mail.EmailOrigen, mail.EmailDestino,
                                mail.Tema, mail.Mensaje);//Envia el correo
                            //Si se envió lo mueve a la tabla de correos enviados o fallidos y le pone estado enviado
                            entities.MoveMessage(mail.Id, 1);}catch (SmtpFailedRecipientException){//Si no se puede enviar
                            //Si ya cumplió los 5 intentos lo mueve a la tabla de correos enviados o fallidos y le pone estado fallido
                            if (mail.Intentos >= 5) entities.MoveMessage(mail.Id, 0);
                            else entities.IncrementarIntentos(mail.Id);}//Si no lleva los 5 intentos le suma un intento
                    }catch (DataException){break;}
                }
            }
        }
    }
}

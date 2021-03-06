﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MailAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class NotificacionesEntities : DbContext
    {
        public NotificacionesEntities()
            : base("name=NotificacionesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ESTADO> ESTADO { get; set; }
        public virtual DbSet<MENSAJE> MENSAJE { get; set; }
        public virtual DbSet<MENSAJENUEVO> MENSAJENUEVO { get; set; }
    
        public virtual int NewMessage(string emailOrigen, string emailDestino, string tema, string mensaje, Nullable<int> intentos, Nullable<int> prioridad)
        {
            var emailOrigenParameter = emailOrigen != null ?
                new ObjectParameter("EmailOrigen", emailOrigen) :
                new ObjectParameter("EmailOrigen", typeof(string));
    
            var emailDestinoParameter = emailDestino != null ?
                new ObjectParameter("EmailDestino", emailDestino) :
                new ObjectParameter("EmailDestino", typeof(string));
    
            var temaParameter = tema != null ?
                new ObjectParameter("Tema", tema) :
                new ObjectParameter("Tema", typeof(string));
    
            var mensajeParameter = mensaje != null ?
                new ObjectParameter("Mensaje", mensaje) :
                new ObjectParameter("Mensaje", typeof(string));
    
            var intentosParameter = intentos.HasValue ?
                new ObjectParameter("Intentos", intentos) :
                new ObjectParameter("Intentos", typeof(int));
    
            var prioridadParameter = prioridad.HasValue ?
                new ObjectParameter("Prioridad", prioridad) :
                new ObjectParameter("Prioridad", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("NewMessage", emailOrigenParameter, emailDestinoParameter, temaParameter, mensajeParameter, intentosParameter, prioridadParameter);
        }
    
        public virtual int MoveMessage(Nullable<int> id, Nullable<int> estado)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var estadoParameter = estado.HasValue ?
                new ObjectParameter("Estado", estado) :
                new ObjectParameter("Estado", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MoveMessage", idParameter, estadoParameter);
        }
    
        public virtual int ObteinNewMessage()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ObteinNewMessage");
        }
    
        public virtual int IncrementarIntentos(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("IncrementarIntentos", idParameter);
        }
    }
}

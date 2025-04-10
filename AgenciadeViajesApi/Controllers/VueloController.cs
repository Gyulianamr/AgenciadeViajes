using AgenciadeViajesApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AgenciadeViajesApi.Controllers
{
    
    public class VueloController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Transporte
        /// <summary>
        /// Obtiene todos los transportes registrados
        /// </summary>
        public IHttpActionResult GetVuelos()
        {
            var vuelos = from v in db.Vuelos
                         join origen in db.Destinos on v.OrigenId equals origen.Id
                         join destino in db.Destinos on v.DestinoId equals destino.Id
                         select new
                         {
                             v.Id,
                             v.Nombre,
                             v.Tipo,
                             v.Compañia,
                             v.HoraSalida,
                             v.HoraLlegada,
                             v.Capacidad,
                             v.Precio,
                             OrigenNombre = origen.NomDestino,
                             DestinoNombre = destino.NomDestino
                         };

            return Ok(vuelos);
        }

        // GET: api/Transporte/5
        /// <summary>
        /// Busca transporte por ID
        /// </summary>
        public IHttpActionResult GetVuelo(int id)
        {
            var vuelo = from v in db.Vuelos
                        join origen in db.Destinos on v.OrigenId equals origen.Id
                        join destino in db.Destinos on v.DestinoId equals destino.Id
                        select new
                        {
                            v.Id,
                            v.Nombre,
                            v.Tipo,
                            v.Compañia,
                            v.HoraSalida,
                            v.HoraLlegada,
                            v.Capacidad,
                            v.Precio,
                            OrigenNombre = origen.NomDestino,
                            DestinoNombre = destino.NomDestino
                        };

            if (vuelo == null)
            {
                return NotFound();
            }

            return Ok(vuelo);
        }

        // POST: api/Transporte
        /// <summary>
        /// Crea nuevo transporte
        /// </summary>
        public IHttpActionResult Post(Vuelo transporte)
        {
            try
            { Destino destinosalida= db.Destinos.Find(transporte.Origen.Id);
                if (destinosalida == null) return NotFound();
                Destino destinollegada = db.Destinos.Find(transporte.Destino.Id);
                if (destinollegada == null) return NotFound();

                transporte.Origen = destinosalida;
                transporte.Destino = destinollegada;

                db.Vuelos.Add(transporte);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = transporte.Id }, transporte);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Transporte/5
        /// <summary>
        /// Actualiza transporte existente
        /// </summary>
        public IHttpActionResult Put(int id, Vuelo transporte)
        {
            try
            {
                if (id != transporte.Id)
                    return BadRequest("ID no coincide");

                var transporteExistente = db.Vuelos.Find(id);
                if (transporteExistente == null)
                    return NotFound();

             
                transporteExistente.Nombre = transporte.Nombre;
                transporteExistente.Precio = transporte.Precio;
                transporteExistente.Tipo = transporte.Tipo;
                transporteExistente.Compañia = transporte.Compañia;
                transporteExistente.HoraSalida = transporte.HoraSalida;
                transporteExistente.HoraLlegada = transporte.HoraLlegada;
                transporteExistente.Capacidad = transporte.Capacidad;

                db.SaveChanges();
                return Ok(transporteExistente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Transporte/5
        /// <summary>
        /// Elimina transporte por ID
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            Vuelo transporte = db.Vuelos.Find(id);
            if (transporte == null) return NotFound();

            db.Vuelos.Remove(transporte);
            db.SaveChanges();
            return Ok(transporte);
        }
    }
}

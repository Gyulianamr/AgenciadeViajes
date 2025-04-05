using AgenciadeViajesApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgenciadeViajesApi.Controllers
{
    public class CotizacionesController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        /// <summary>
        /// Obtiene la lista de todas las cotizaciones.
        /// </summary>
        /// <returns>Lista de cotizaciones</returns>
        public IHttpActionResult Get()
        {
            var cotizaciones = from C in db.Cotizaciones
                               select new
                               {
                                   C.Id,
                                   ClienteId = C.Cliente.Id,
                                   ClienteNombre = C.Cliente.Nombre,
                                   AgenteResponsableId = C.AgenteResponsable.Id,
                                   AgenteResponsableNombre = C.AgenteResponsable.Nombre,
                                   PaqueteId = C.Paquete.Id,
                                   PaqueteNombre = C.Paquete.Nombre,
                                   C.CantidadPersonas,
                                   C.FechaCotizacion,
                                   C.CostoTotal
                               };

            // Verificar si no hay resultados
            if (!cotizaciones.Any())
            {
                return NotFound();
            }

            return Ok(cotizaciones);
        }

        /// <summary>
        /// Obtiene una cotización por su ID.
        /// </summary>
        /// <param name="id">ID de la cotización</param>
        /// <returns>Cotización correspondiente al ID</returns>
        public IHttpActionResult Get(int id)
        {
            var cotizacion = from C in db.Cotizaciones
                             where C.Id == id
                             select new
                             {
                                 C.Id,
                                 ClienteId = C.Cliente.Id,
                                 ClienteNombre = C.Cliente.Nombre,
                                 AgenteResponsableId = C.AgenteResponsable.Id,
                                 AgenteResponsableNombre = C.AgenteResponsable.Nombre,
                                 PaqueteId = C.Paquete.Id,
                                 PaqueteNombre = C.Paquete.Nombre,
                                 C.CantidadPersonas,
                                 C.FechaCotizacion,
                                 C.CostoTotal
                             };

            // Verificar si no hay resultados
            if (!cotizacion.Any())
            {
                return NotFound();
            }

            return Ok(cotizacion);
        }

        /// <summary>
        /// Crea una nueva cotización.
        /// </summary>
        /// <param name="cotizacion">Datos de la nueva cotización</param>
        /// <returns>Resultado de la operación de creación</returns>
        /// 
        public IHttpActionResult Post([FromBody] Cotizacion cotizacion)
        {
            try
            {

                Cliente cliente = db.Clientes.Find(cotizacion.ClienteId);
                if (cliente == null)
                { return BadRequest("Cliente no válido"); }
                cotizacion.Cliente = cliente;

                AgentedeViaje agente = db.AgenteViajes.Find(cotizacion.AgenteResponsableId);
                if (agente == null)
                {
                    return BadRequest("Agente no válido");
                }
                cotizacion.AgenteResponsable = agente;

                Paquete_Turistico paquete = db.PaqueteTuristicos.Find(cotizacion.PaqueteId);
                if (paquete == null)
                {
                    return BadRequest("Paquete no válido");
                }
                cotizacion.Paquete = paquete;

                cotizacion.CostoTotal = cotizacion.Costo();


                if (cotizacion.FechaCotizacion > DateTime.Now)
                    cotizacion.FechaCotizacion = DateTime.Now;


                db.Cotizaciones.Add(cotizacion);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = cotizacion.Id }, cotizacion);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Error de BD: " + ex.InnerException?.Message);
            }
        }

        /// <summary>
        /// Actualiza una cotización existente.
        /// </summary>
        /// <param name="id">ID de la cotización a actualizar</param>
        /// <param name="cotizacion">Cotización con los datos actualizados</param>
        /// <returns>Resultado de la operación de actualización</returns>
        public IHttpActionResult Put(Cotizacion cotizacion)
        {
            try
            {
                var existente = db.Cotizaciones.Find(cotizacion.Id);
                if (existente == null) return NotFound();

                // Verifica si el cliente es válido
                Cliente cliente = db.Clientes.Find(cotizacion.ClienteId);
                if (cliente == null)
                {
                    return BadRequest("Cliente no válido");
                }
                cotizacion.Cliente = cliente;

                // Verifica si el agente de viaje es válido
                AgentedeViaje agente = db.AgenteViajes.Find(cotizacion.AgenteResponsableId);
                if (agente == null)
                {
                    return BadRequest("Agente no válido");
                }
                cotizacion.AgenteResponsable = agente;

                // Verifica si el paquete turístico es válido
                Paquete_Turistico paquete = db.PaqueteTuristicos.Find(cotizacion.PaqueteId);
                if (paquete == null)
                {
                    return BadRequest("Paquete no válido");
                }
                cotizacion.Paquete = paquete;

                // Calcula el costo total
                cotizacion.CostoTotal = cotizacion.Costo();

                // Asegura que la fecha de cotización no sea futura
                if (cotizacion.FechaCotizacion > DateTime.Now)
                    cotizacion.FechaCotizacion = DateTime.Now;

                // Actualiza los valores de la cotización existente con los nuevos datos
                db.Entry(existente).CurrentValues.SetValues(cotizacion);
                db.SaveChanges();

                return Ok(existente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Error de BD: " + ex.InnerException?.Message);
            }
        }


        /// <summary>
        /// Elimina una cotización por su ID.
        /// </summary>
        /// <param name="id">ID de la cotización a eliminar</param>
        /// <returns>Resultado de la operación de eliminación</returns>
        public IHttpActionResult Delete(int id)
        {
            Cotizacion cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null)
            {
                return NotFound();
            }

            db.Cotizaciones.Remove(cotizacion);
            db.SaveChanges();
            return Ok(cotizacion);
        }
    }

}

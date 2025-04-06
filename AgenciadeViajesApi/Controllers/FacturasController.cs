using AgenciadeViajesApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgenciadeViajesApi.Controllers
{
    public class FacturasController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        /// <summary>
        /// Obtiene la lista de todas las facturas.
        /// </summary>
        /// <returns>Lista de facturas</returns>
        public IHttpActionResult Get()
        {
            var result = from factura in db.Factura
                         join reservacion in db.Reservas on factura.ReservacionId equals reservacion.Id
                         join metodoPago in db.MetododePagos on factura.MetodoPagoId equals metodoPago.Id
                         select new
                         {
                             Id = factura.Id,
                             ReservacionId = reservacion.IdCotizacion,
                             FechaPago = factura.FechaPago,
                             MontoPagado = factura.MontoPagado,
                             MetodoPago = metodoPago.Nombre, // Asumí que hay un campo 'Nombre' en MetodoPago, cámbialo si es necesario
                             Estado = factura.Estado
                         };

            return Ok(result);
        }


        /// <summary>
        /// Obtiene una factura por su ID.
        /// </summary>
        /// <param name="id">ID de la factura</param>
        /// <returns>Factura correspondiente al ID</returns>
        public IHttpActionResult Get(int id)
        {
            var result = from factura in db.Factura
                         join reservacion in db.Reservas on factura.ReservacionId equals reservacion.Id
                         join metodoPago in db.MetododePagos on factura.MetodoPagoId equals metodoPago.Id
                         select new
                         {
                             Id = factura.Id,
                             ReservacionId = reservacion.Id,
                             FechaPago = factura.FechaPago,
                             MontoPagado = factura.MontoPagado,
                             MetodoPagoId = metodoPago.Id,
                             Estado = factura.Estado
                         };

            var facturaResult = result.FirstOrDefault(f => f.Id == id);

            if (facturaResult == null)
            {
                return NotFound();
            }

            return Ok(facturaResult);
        }

        /// <summary>
        /// Crea una nueva factura.
        /// </summary>
        /// <param name="factura">Datos de la nueva factura</param>
        /// <returns>Resultado de la operación de creación</returns>
        public IHttpActionResult Post(Factura factura)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si la reservación existe
                var reservacion = db.Reservas.Find(factura.ReservacionId);
                if (reservacion == null)
                {
                    return BadRequest("Reservación no encontrada.");
                }

                // Verificar si el método de pago existe
                var metodoPago = db.MetododePagos.Find(factura.MetodoPagoId);
                if (metodoPago == null)
                {
                    return BadRequest("Método de pago no encontrado.");
                }

                // Asignar la reservación y el método de pago
                factura.Reservacion = reservacion;
                factura.MetodoPago = metodoPago;

                db.Factura.Add(factura);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = factura.Id }, factura);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una factura existente.
        /// </summary>
        /// <param name="id">ID de la factura a actualizar</param>
        /// <param name="factura">Datos de la factura actualizada</param>
        /// <returns>Resultado de la operación de actualización</returns>
        public IHttpActionResult Put(int id, Factura factura)
        {
            try
            {
                if (id != factura.Id)
                {
                    return BadRequest("ID no coincide.");
                }

                var existente = db.Factura.Find(id);
                if (existente == null)
                {
                    return NotFound();
                }

                // Actualizar los campos de la factura
                existente.FechaPago = factura.FechaPago;
                existente.MontoPagado = factura.MontoPagado;
                existente.Estado = factura.Estado;

                db.Entry(existente).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(existente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina una factura por su ID.
        /// </summary>
        /// <param name="id">ID de la factura a eliminar</param>
        /// <returns>Resultado de la operación de eliminación</returns>
        public IHttpActionResult Delete(int id)
        {
            var factura = db.Factura.Find(id);
            if (factura == null)
            {
                return NotFound();
            }

            db.Factura.Remove(factura);
            db.SaveChanges();
            return Ok(factura);
        }
    }

}

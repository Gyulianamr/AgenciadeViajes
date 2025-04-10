﻿using AgenciadeViajesApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace AgenciadeViajesApi.Controllers
{

    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Data.Entity;
    using System.Web.Http.Description;
    using System.Data.Entity.Infrastructure;
    using Newtonsoft.Json;

    public class PaquetesTuristicosController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/PaquetesTuristicos
        /// <summary>
        /// Metodo para ver la lista de paquetes turisticos
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {
            var result = from paquete in db.PaqueteTuristicos
                         join vuelo in db.Vuelos on paquete.VueloId equals vuelo.Id into vuelos
                         from vuelo in vuelos.DefaultIfEmpty()
                         join hotel in db.Hotel on paquete.HotelId equals hotel.Id into hoteles
                         from hotel in hoteles.DefaultIfEmpty()
                         join actividad in db.Actividades on paquete.ActividadesId equals actividad.Id into actividades
                         from actividad in actividades.DefaultIfEmpty()
                         join guia in db.GuiaTuristicos on paquete.GuiaTuristicoId equals guia.Id into guias
                         from guia in guias.DefaultIfEmpty()
                         join seguro in db.Seguros on paquete.SeguroId equals seguro.Id into seguros
                         from seguro in seguros.DefaultIfEmpty()
                         select new
                         {
                             Id = paquete.Id,
                             Nombre = paquete.Nombre,
                             DestinoId = paquete.DestinoId,
                             VueloId = vuelo != null ? vuelo.Id : (int?)null,
                             HotelId = hotel != null ? hotel.Id : (int?)null,
                             ActividadesId = actividad != null ? actividad.Id : (int?)null,
                             GuiaTuristicoId = guia != null ? guia.Id : (int?)null,
                             SeguroId = seguro != null ? seguro.Id : (int?)null,
                             PrecioTotal = paquete.PrecioTotal,
                             FechaExpiracion = paquete.FechaExpiracion,
                             Estado = paquete.Estado,
                             Duracion_Dias = paquete.Duracion_Dias
                         };

            return Ok(result);
        }

        // GET: api/PaquetesTuristicos/5
        /// <summary>
        /// Metodo para buscar un paquete por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Paquete_Turistico))]
        public IHttpActionResult Get(int id)
        {
            var paquete = from p in db.PaqueteTuristicos
                          where p.Id == id
                          join vuelo in db.Vuelos on p.VueloId equals vuelo.Id into vuelos
                          from vuelo in vuelos.DefaultIfEmpty()
                          join hotel in db.Hotel on p.HotelId equals hotel.Id into hoteles
                          from hotel in hoteles.DefaultIfEmpty()
                          join actividad in db.Actividades on p.ActividadesId equals actividad.Id into actividades
                          from actividad in actividades.DefaultIfEmpty()
                          join guia in db.GuiaTuristicos on p.GuiaTuristicoId equals guia.Id into guias
                          from guia in guias.DefaultIfEmpty()
                          join seguro in db.Seguros on p.SeguroId equals seguro.Id into seguros
                          from seguro in seguros.DefaultIfEmpty()
                          select new
                          {
                              Id = p.Id,
                              Nombre_paquete = p.Nombre,
                              DestinoId = p.DestinoId,
                              VueloId = vuelo != null ? vuelo.Id : (int?)null,
                              HotelId = hotel != null ? hotel.Id : (int?)null,
                              ActividadesId = actividad != null ? actividad.Id : (int?)null,
                              GuiaTuristicoId = guia != null ? guia.Id : (int?)null,
                              SeguroId = seguro != null ? seguro.Id : (int?)null,
                              PrecioTotal = p.PrecioTotal,
                              FechaExpiracion = p.FechaExpiracion,
                              Estado = p.Estado,
                              Duracion_Dias = p.Duracion_Dias
                          };

            if (!paquete.Any())
            {
                return NotFound();
            }

            return Ok(paquete);
        }

        // POST: api/PaquetesTuristicos
        /// <summary>
        /// Metodo para agregar un paquete nuevo
        /// </summary>
        /// <param name="paquete"></param>
        /// <returns></returns>
        [ResponseType(typeof(Paquete_Turistico))]
        public IHttpActionResult Post(Paquete_Turistico paquete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (paquete.DestinoId == 0)
            {
                return BadRequest("El campo DestinoId es requerido");
            }

            var destino = db.Destinos.Find(paquete.DestinoId);
            if (destino == null)
            {
                return BadRequest("Destino no encontrado");
            }

            if (paquete.VueloId.HasValue && db.Vuelos.Find(paquete.VueloId.Value) == null)
            {
                return BadRequest("Vuelo no encontrado");
            }

            if (paquete.HotelId.HasValue && db.Hotel.Find(paquete.HotelId.Value) == null)
            {
                return BadRequest("Hotel no encontrado");
            }

            if (paquete.SeguroId.HasValue && db.Seguros.Find(paquete.SeguroId.Value) == null)
            {
                return BadRequest("Seguro no encontrado");
            }

            if (paquete.GuiaTuristicoId.HasValue && db.GuiaTuristicos.Find(paquete.GuiaTuristicoId.Value) == null)
            {
                return BadRequest("Guía turístico no encontrado");
            }

            if (paquete.ActividadesId.HasValue && db.Actividades.Find(paquete.ActividadesId.Value) == null)
            {
                return BadRequest("Actividad no encontrada");
            }

            paquete.Vuelo = paquete.VueloId.HasValue ? db.Vuelos.Find(paquete.VueloId.Value) : null;
            paquete.Hotel = paquete.HotelId.HasValue ? db.Hotel.Find(paquete.HotelId.Value) : null;
            paquete.Seguro = paquete.SeguroId.HasValue ? db.Seguros.Find(paquete.SeguroId.Value) : null;
            paquete.GuiaTuristico = paquete.GuiaTuristicoId.HasValue ? db.GuiaTuristicos.Find(paquete.GuiaTuristicoId.Value) : null;
            paquete.Actividades = paquete.ActividadesId.HasValue ? db.Actividades.Find(paquete.ActividadesId.Value) : null;
            paquete.Destino = destino;

            paquete.PrecioTotal = paquete.CalcularCostoTotal();

            db.PaqueteTuristicos.Add(paquete);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = paquete.Id }, paquete);
        }

        // PUT: api/PaquetesTuristicos/5
        /// <summary>
        /// Metodo para actualizar un paquete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paquete"></param>
        /// <returns></returns>

        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, Paquete_Turistico paquete)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != paquete.Id)
                {
                    return BadRequest("ID no coincide.");
                }

                var paqueteExistente = db.PaqueteTuristicos
                    .Include(p => p.Vuelo)
                    .Include(p => p.Hotel)
                    .Include(p => p.Seguro)
                    .Include(p => p.GuiaTuristico)
                    .Include(p => p.Actividades)
                    .FirstOrDefault(p => p.Id == id);

                if (paqueteExistente == null)
                {
                    return NotFound();
                }

               
                paqueteExistente.Nombre = paquete.Nombre;
                paqueteExistente.Duracion_Dias = paquete.Duracion_Dias;
                paqueteExistente.FechaExpiracion = paquete.FechaExpiracion;
                paqueteExistente.Estado = paquete.Estado;

                paqueteExistente.VueloId = paquete.VueloId;
                paqueteExistente.HotelId = paquete.HotelId;
                paqueteExistente.SeguroId = paquete.SeguroId;
                paqueteExistente.GuiaTuristicoId = paquete.GuiaTuristicoId;
                paqueteExistente.ActividadesId = paquete.ActividadesId;

            
                paqueteExistente.Vuelo = paqueteExistente.VueloId.HasValue
                    ? db.Vuelos.Find(paqueteExistente.VueloId.Value)
                    : null;

                paqueteExistente.Hotel = paqueteExistente.HotelId.HasValue
                    ? db.Hotel.Find(paqueteExistente.HotelId.Value)
                    : null;

                paqueteExistente.Seguro = paqueteExistente.SeguroId.HasValue
                    ? db.Seguros.Find(paqueteExistente.SeguroId.Value)
                    : null;

                paqueteExistente.GuiaTuristico = paqueteExistente.GuiaTuristicoId.HasValue
                    ? db.GuiaTuristicos.Find(paqueteExistente.GuiaTuristicoId.Value)
                    : null;

                paqueteExistente.Actividades = paqueteExistente.ActividadesId.HasValue
                    ? db.Actividades.Find(paqueteExistente.ActividadesId.Value)
                    : null;

             
                double total = 0;

                if (paqueteExistente.Vuelo != null)
                    total += paqueteExistente.Vuelo.Precio;

                if (paqueteExistente.Hotel != null && paqueteExistente.Hotel.Tipohabitacion != null)
                    total += paqueteExistente.Hotel.Tipohabitacion.PrecioPorNoche * paqueteExistente.Duracion_Dias;

                if (paqueteExistente.Seguro != null)
                    total += paqueteExistente.Seguro.Precio;

                if (paqueteExistente.GuiaTuristico != null)
                    total += paqueteExistente.GuiaTuristico.Salario;

                if (paqueteExistente.Actividades != null)
                    total += paqueteExistente.Actividades.PrecioHora;

                paqueteExistente.PrecioTotal = total;

                db.Entry(paqueteExistente).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(paqueteExistente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        // DELETE: api/PaquetesTuristicos/5
        /// <summary>
        /// Metodo para eliminar un paquete turistico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Paquete_Turistico))]
        public IHttpActionResult Delete(int id)
        {
            Paquete_Turistico paquete = db.PaqueteTuristicos.Find(id);
            if (paquete == null)
            {
                return NotFound();
            }

            db.PaqueteTuristicos.Remove(paquete);
            db.SaveChanges();

            return Ok(paquete);
        }


        /// <summary>
        /// Busca paquetes turísticos por destino y precio máximo.
        /// </summary>
        /// <param name="destinoId">ID del destino</param>
        /// <param name="precioMaximo">Precio máximo</param>
        /// <returns>Lista de paquetes turísticos que cumplen con los criterios</returns>
        [HttpGet]
        [Route("api/PaqueteTuristico/buscar-por-destino-precio")]
        public IHttpActionResult BuscarPorDestinoYPrecio(int destinoId, double precioMaximo)
        {
            if (destinoId <= 0)
            {
                return BadRequest("El ID del destino debe ser mayor que cero.");
            }

            if (precioMaximo <= 0)
            {
                return BadRequest("El precio máximo debe ser mayor que cero.");
            }

            var resultados = from p in db.PaqueteTuristicos
                             where p.Destino != null && p.Destino.Id == destinoId && p.PrecioTotal <= precioMaximo
                             orderby p.PrecioTotal
                             select new
                             {
                                 p.Id,
                                 p.Nombre,
                                 Destino = p.Destino != null ? p.Destino.NomDestino : "Destino no especificado",
                                 p.PrecioTotal,
                                 Vuelo = p.Vuelo != null ? p.Vuelo.Compañia : "Sin vuelo",
                                 Hotel = p.Hotel != null ? p.Hotel.Nombre : "Sin hotel",
                                 Seguro = p.Seguro != null ? p.Seguro.Tipo : "Sin seguro"
                             };

            if (!resultados.Any())
            {
                return NotFound();
            }

            return Ok(resultados);
        }

        /// <summary>
        /// Busca paquetes turísticos por nombre y precio mínimo.
        /// </summary>
        /// <param name="nombre">Nombre del paquete turístico</param>
        /// <param name="precioMinimo">Precio mínimo</param>
        /// <returns>Lista de paquetes turísticos que cumplen con los criterios</returns>
        [HttpGet]
        [Route("api/PaqueteTuristico/buscar-por-nombre-precio")]
        public IHttpActionResult BuscarPorNombreYPrecio(string nombre, double precioMinimo)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El nombre no puede estar vacío.");
            }

            if (precioMinimo <= 0)
            {
                return BadRequest("El precio mínimo debe ser mayor que cero.");
            }

            var resultados = from p in db.PaqueteTuristicos
                             where p.Nombre.Contains(nombre) && p.PrecioTotal >= precioMinimo
                             orderby p.PrecioTotal
                             select new
                             {
                                 p.Id,
                                 p.Nombre,
                                 p.PrecioTotal,
                                 Vuelo = p.Vuelo != null ? p.Vuelo.Compañia : "Sin vuelo",
                                 Hotel = p.Hotel != null ? p.Hotel.Nombre : "Sin hotel",
                                 Seguro = p.Seguro != null ? p.Seguro.Tipo : "Sin seguro"
                             };

            if (!resultados.Any())
            {
                return NotFound();
            }

            return Ok(resultados);
        }

      
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgenciadeViajesApi.Models
{
    public class Actividades
    {
        private int _idActividad;
        private string _nombre;
        private string _descripcion;
        private int _duracionHoras;
        private bool _requiereReserva;
        private double _precioHora;

        public Actividades() { }

        public Actividades(int id, string nombre, string descripcion, int duracionHoras,
                           bool requiereReserva, double precioHora)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.DuracionHoras = duracionHoras;
            this.RequiereReserva = requiereReserva;
            this.PrecioHora = precioHora;
        }

        public int Id
        {
            get { return _idActividad; }
            set { _idActividad = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                
                _nombre = value;
            }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public int DuracionHoras
        {
            get { return _duracionHoras; }
            set
            {
                
                _duracionHoras = value;
            }
        }

        public bool RequiereReserva
        {
            get { return _requiereReserva; }
            set { _requiereReserva = value; }
        }

        public double PrecioHora
        {
            get { return _precioHora; }
            set
            {
               
                _precioHora = value;
            }
        }
    }
}
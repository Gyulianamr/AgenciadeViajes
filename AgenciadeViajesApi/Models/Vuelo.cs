using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenciadeViajesApi.Models
{
    public class Vuelo
    {
        private int _idVuelo;
        public string Nombre { get; set; }  
        private string _tipo;
        private Destino _origen;
        private Destino _destino;
        private string _compañia;
        private TimeSpan _horaSalida;
        private TimeSpan _horaLlegada;
        private int _capacidad;
        private double _precio;


        public Vuelo() { }


        public Vuelo(int idVuelo, string tipo, string compañia, Destino origen,
                     Destino destino, TimeSpan horaSalida, TimeSpan horaLlegada,
                     int capacidad, double precio)
        {
            Id = idVuelo;
            Tipo = tipo;
            Compañia = compañia;
            Origen = origen;
            Destino = destino;
            HoraSalida = horaSalida;
            HoraLlegada = horaLlegada;
            Capacidad = capacidad;
            Precio = precio;
        }


        public int Id
        {
            get { return _idVuelo; }
            set
            {

                _idVuelo = value;
            }
        }


        public string Tipo
        {
            get { return _tipo; }
            set
            {
        
                _tipo = value;
            }
        }

        public string Compañia
        {
            get { return _compañia; }
            set
            {
               
                _compañia = value;
            }
        }


        public Destino Origen
        {
            get { return _origen; }
            set
            {
                
                _origen = value;
            }
        }


        public Destino Destino
        {
            get { return _destino; }
            set
            {
                _destino = value;
            }
        }


        public TimeSpan HoraSalida
        {
            get { return _horaSalida; }
            set
            {

                _horaSalida = value;
            }
        }


        public TimeSpan HoraLlegada
        {
            get { return _horaLlegada; }
            set
            {

                _horaLlegada = value;
            }
        }


        public int Capacidad
        {
            get { return _capacidad; }
            set
            {
                
                _capacidad = value;
            }
        }


        public double Precio
        {
            get { return _precio; }
            set
            {
                
                _precio = value;
            }
        }

       
    }
}
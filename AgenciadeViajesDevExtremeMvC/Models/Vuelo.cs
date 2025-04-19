using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenciadeViajesDevExtremeMvC.Models
{
    public class Vuelo
    {
        private int _idVuelo;
        private string _tipo;
        private Destino _origen;
        private Destino _destino;
        private string _compañia;
        private TimeSpan _horaSalida;
        private TimeSpan _horaLlegada;
        private int _capacidad;
        private double _precio;

     
        public int OrigenId { get; set; }
        public int DestinoId { get; set; }

      
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre no debe quedar vacio")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El campo Tipo no debe quedar vacio")]
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        [Required(ErrorMessage = "El campo compañia no debe quedar vacio")]
        public string Compañia
        {
            get { return _compañia; }
            set { _compañia = value; }
        }

        [Required(ErrorMessage = "El campo Hora de salida no debe quedar vacio")]
        public TimeSpan HoraSalida
        {
            get { return _horaSalida; }
            set { _horaSalida = value; }
        }

        [Required(ErrorMessage = "El campo Hora de llegada no debe quedar vacio")]
        public TimeSpan HoraLlegada
        {
            get { return _horaLlegada; }
            set { _horaLlegada = value; }
        }

        [Required(ErrorMessage = "El campo Capacidad no debe quedar vacio")]
        public int Capacidad
        {
            get { return _capacidad; }
            set { _capacidad = value; }
        }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad en Precio debe ser mayor a 0")]
        public double Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

   
        public virtual Destino Origen { get; set; }
        public virtual Destino Destino { get; set; }
    }

}
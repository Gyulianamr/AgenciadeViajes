using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace AgenciadeViajesDevExtremeMvC.Models
{
    public abstract class Empleado : Persona
    {
        private int _id;
        private DateTime _fechacontratacion;
        private TimeSpan _horallegada;
        private TimeSpan _horasalida;
        private double _salario;

        public Empleado() { }

        public Empleado( DateTime fechacontratacion, TimeSpan horallegada,
                      TimeSpan horasalida, double salario, int idpersona, string nombre,
                      string apellido, string telefono, string correo)
            : base(idpersona, nombre, apellido, telefono, correo)
        {
           
            FechaContratacion = fechacontratacion;
            HoraLlegada = horallegada;
            HoraSalida = horasalida;
            Salario = salario;
        }


        [Required(ErrorMessage = "El campo fecha de contratacion no debe quedar vacio")]
        public DateTime FechaContratacion
        {
            get { return _fechacontratacion; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("La fecha de contratación no puede ser futura");
                _fechacontratacion = value;
            }
        }

        [Required(ErrorMessage = "El campo hora de llegada no debe quedar vacio")]
        public TimeSpan HoraLlegada
        {
            get { return _horallegada; }
            set
            {
                if (HoraSalida != default && value > HoraSalida)
                    throw new ArgumentException("La hora de llegada debe ser anterior a la de salida");
                _horallegada = value;
            }
        }

        [Required(ErrorMessage = "El campo hora de salida no debe quedar vacio")]
        public TimeSpan HoraSalida
        {
            get { return _horasalida; }
            set
            {
                if (HoraLlegada != default && value <= HoraLlegada)
                    throw new ArgumentException("La hora de salida debe ser posterior a la de llegada");
                _horasalida = value;
            }
        }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public double Salario
        {
            get { return _salario; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El salario no puede ser negativo");
                _salario = value;
            }
        }
    }
}

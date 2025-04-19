using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenciadeViajesDevExtremeMvC.Models
{
    public class Reservacion
    {
        [Key]
        public int Id { get; set; }

        public int IdCotizacion { get; set; }

        [ForeignKey("IdCotizacion")]
        public virtual Cotizacion Cotizacion { get; set; }

        [Required(ErrorMessage = "El campo Fecha Reservacion no debe quedar vacio")]
        public DateTime FechaReservacion { get; set; }

        [Required(ErrorMessage = "El campo Estado no debe quedar vacio")]
        public string Estado { get; set; } // Ejemplo: Confirmada, Cancelada, En Proceso

        [Required(ErrorMessage = "El campo Fecha Viaje no debe quedar vacio")]
        public DateTime FechaViaje { get; set; }

        [Required(ErrorMessage = "El campo Fecha de regreso no debe quedar vacio")]
        public DateTime FechaRegreso { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad en monto pagado debe ser mayor a 0")]
        public double MontoPagado { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad en saldo pendiente debe ser mayor a 0")]
        public double Saldopendiente { get; set; }

        // Constructor vacío
        public Reservacion() { }

        // Método para calcular saldo pendiente
        public double CalcularSaldoPendiente()
        {
            if (Cotizacion == null)
            {
                throw new ArgumentNullException(nameof(Cotizacion), "La cotización no puede ser nula.");
            }

            if (Cotizacion.CostoTotal < 0)
            {
                throw new ArgumentException("El costo total no puede ser negativo.");
            }

            if (MontoPagado < 0)
            {
                throw new ArgumentException("El monto pagado no puede ser negativo.");
            }

            Saldopendiente = Cotizacion.CostoTotal - MontoPagado;
            return Saldopendiente;
        }

        public bool EsPagada()
        {
            return CalcularSaldoPendiente() == 0;
        }
    }
}


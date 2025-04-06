using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenciadeViajesApi.Models
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReservacionId { get; set; }

        [ForeignKey("ReservacionId")]
        public virtual Reservacion Reservacion { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor a 0")]
        public double MontoPagado { get; set; }

        [Required]
        public int MetodoPagoId { get; set; }

        [ForeignKey("MetodoPagoId")]
        public virtual Metodo_Pago MetodoPago { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        // Constructor vacío
        public Factura() { }

        // Constructor con parámetros
     
        // Método para determinar si el pago está completo
        public bool EsPagoCompleto()
        {
            return MontoPagado >= Reservacion.CalcularSaldoPendiente();
        }
    }

}
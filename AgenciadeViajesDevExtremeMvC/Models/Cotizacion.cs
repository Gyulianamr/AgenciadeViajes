using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenciadeViajesDevExtremeMvC.Models
{
   
  public class Cotizacion
    {
        [Key]
        public int Id { get; set; }

        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }  // Relación con Cliente

        public int AgenteResponsableId { get; set; }

        [ForeignKey("AgenteResponsableId")]
        public virtual AgentedeViaje AgenteResponsable { get; set; }  // Relación con Agente de Viaje

        public int PaqueteId { get; set; }

        [ForeignKey("PaqueteId")]
        public virtual Paquete_Turistico Paquete { get; set; }  // Relación con Paquete Turístico

        [Range(0.01, int.MaxValue, ErrorMessage = "La cantidad de personas debe ser mayor a 0")]
        public int CantidadPersonas { get; set; }

        [Required(ErrorMessage = "El campo Fecha no debe quedar vacio")]
        public DateTime FechaCotizacion { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public double CostoTotal { get; set; }

        // Constructor vacío
        public Cotizacion() { }

        // Constructor con parámetros


       // public double Costo { get; set; }

        public double Costo()
        {
            Console.WriteLine($"Paquete PrecioTotal: {Paquete.PrecioTotal}");
            Console.WriteLine($"Cantidad Personas: {CantidadPersonas}");

            double total = Paquete.PrecioTotal * CantidadPersonas;
            return total;
        }


        // Método para actualizar el costo total
        public void ActualizarCostoTotal()
        {
            CostoTotal =Costo();
        }
    }
}

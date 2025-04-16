using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenciadeViajesDevExtremeMvC.Models
{
    public class Seguro
    {
        private int _idSeguro;
        private string _nombre;
        private string _tipo;
        private string _cobertura;
        private int _duracionDias;
        private double _precio;

        public Seguro()
        {
        }

        public Seguro(int idSeguro, string nombre, string tipo, string cobertura,
                    int duracionDias, double precio)
        {
            Id = idSeguro;
            Nombre = nombre;
            Tipo = tipo;
            Cobertura = cobertura;
            DuracionDias = duracionDias;
            Precio = precio;
        }

        public int Id
        {
            get { return _idSeguro; }
            set
            {
    
                _idSeguro = value;
            }
        }

        [Required(ErrorMessage = "El campo Nombre no debe quedar vacio")]
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío");

                if (value.Length < 3 || value.Length > 100)
                    throw new ArgumentException("El nombre debe tener entre 3 y 100 caracteres");

                _nombre = value;
            }
        }

        [Required(ErrorMessage = "El campo Tipo no debe quedar vacio")]
        public string Tipo
        {
            get { return _tipo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El tipo no puede estar vacío");

                if (value.Length < 3 || value.Length > 50)
                    throw new ArgumentException("El tipo debe tener entre 3 y 50 caracteres");

                _tipo = value;
            }
        }

        [Required(ErrorMessage = "El campo cobertura no debe quedar vacio")]
        public string Cobertura
        {
            get { return _cobertura; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La cobertura no puede estar vacía");

                _cobertura = value;
            }
        }

        [Range(0.01, int.MaxValue, ErrorMessage = "La cantidad en duracion de dias debe ser mayor a 0")]
        public int DuracionDias
        {
            get { return _duracionDias; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La duración debe ser mayor que 0");
                _duracionDias = value;
            }
        }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad en el precio debe ser mayor a 0")]
        public double Precio
        {
            get { return _precio; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El precio no puede ser negativo");
                _precio = value;
            }
        }
    }
}
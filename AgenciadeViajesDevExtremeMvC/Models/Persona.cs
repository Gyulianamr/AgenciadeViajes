using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AgenciadeViajesDevExtremeMvC.Models
{

    public abstract class Persona
    {
        private int _id;
        private string _nombre;
        private string _apellido;
        private string _telefono;
        private string _correo;

        public Persona() { }

        public Persona(int idpersona, string nombre, string apellido, string telefono, string correo)
        {
            Id = idpersona;
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Correo = correo;
        }

        public int Id
        {
            get { return _id; }
            set
            {
             
                _id = value;
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

                if (value.Length < 2 || value.Length > 50)
                    throw new ArgumentException("El nombre debe tener entre 2 y 50 caracteres");

                _nombre = value;
            }
        }

        [Required(ErrorMessage = "El campo Apellido no debe quedar vacio")]
        public string Apellido
        {
            get { return _apellido; }
            set
            {
               

                _apellido = value;
            }
        }

        [Required(ErrorMessage = "El campo Telefono no debe quedar vacio")]
        public string Telefono
        {
            get { return _telefono; }
            set
            {
                

                _telefono = value;
            }
        }

        [Required(ErrorMessage = "El campo Correo no debe quedar vacio")]
        public string Correo
        {
            get { return _correo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El correo electrónico no puede estar vacío");

                if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("Formato de correo electrónico no válido");

                _correo = value;
            }
        }
    }
}
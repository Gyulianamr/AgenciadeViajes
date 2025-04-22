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

                _correo = value;
            }
        }
    }
}
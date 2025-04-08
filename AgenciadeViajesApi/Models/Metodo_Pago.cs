using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgenciadeViajesApi.Models
{
    public class Metodo_Pago
    {

        private int _idMetodo;
        private string _nombre;
        private string _descripcion;
        private bool _activo;

        public Metodo_Pago() { }

        public Metodo_Pago(int idMetodo, string nombre, string descripcion, bool activo)
        {
            Id = idMetodo;
            Nombre = nombre;
            Descripcion = descripcion;
            Activo = activo;
        }

        public int Id
        {
            get { return _idMetodo; }
            set
            {
              
                _idMetodo = value;
            }
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
            set
            {
                _descripcion = value;
            }
        }

        public bool Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }

        public void HabilitarMetodo()
        {
            _activo = true;
        }

        public void DeshabilitarMetodo()
        {
            _activo = false;
        }

        public bool EsMetodoActivo()
        {
            return _activo;
        }

    }
}
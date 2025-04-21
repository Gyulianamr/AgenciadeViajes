using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgenciadeViajesApi.Models
{
    public class Destino
    {
        private int _idDestino;
        private string _nomdestino;
        private string _pais;
        private string _descrip;
        private string _tipo;
        private string _moneda;
        private bool _reqvisa;

        public Destino() { }

        public Destino(int idDestino, string nomdestino, string pais, string descrip,
                     string tipo, string moneda, bool reqvisa)
        {
            Id = idDestino;
            NomDestino = nomdestino;
            Pais = pais;
            Descripcion = descrip;
            Tipo = tipo;
            Moneda = moneda;
            RequiereVisa = reqvisa;
        }

        public int Id
        {
            get { return _idDestino; }
            set
            {
                _idDestino = value;
            }
        }

        public string NomDestino
        {
            get { return _nomdestino; }
            set
            {
                _nomdestino = value;
            }
        }

        public string Pais
        {
            get { return _pais; }
            set
            {
            
                _pais = value;
            }
        }

        public string Descripcion
        {
            get { return _descrip; }
            set
            {
               
                _descrip = value;
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

        public string Moneda
        {
            get { return _moneda; }
            set
            {
                
                    

                _moneda = value;
            }
        }

        public bool RequiereVisa
        {
            get { return _reqvisa; }
            set { _reqvisa = value; }
        }
    }
}
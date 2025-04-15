using AgenciadeViajesDevExtremeMvC.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;

namespace AgenciadeViajesDevExtremeMvC.Controllers
{
    public class TipoHabitacionController : ApiController
    {
        //Creacion de habitacion
        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44321/api/TipoHabitacion";

            var respuestaJson = await GetAsync(apiUrl);
            List<TipoHabitacion> lista = JsonConvert.DeserializeObject<List<TipoHabitacion>>(respuestaJson);

            return Request.CreateResponse(DataSourceLoader.Load(lista, loadOptions));
        }

        public static async Task<string> GetAsync(string uri)
        {
            try
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(handler))
                {
                    var response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                var m = e.Message;
                return null;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var values = form.Get("values");
            var httpContent = new StringContent(values, System.Text.Encoding.UTF8, "application/json");

            var url = "https://localhost:44321/api/TipoHabitacion";
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using (var client = new HttpClient(handler))
            {
                var response = await client.PostAsync(url, httpContent);
                var result = await response.Content.ReadAsStringAsync();
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            var apiUrl = $"https://localhost:44321/api/TipoHabitacion/{key}";
            var respuesta = await GetAsync(apiUrl);

            if (respuesta == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "TipoHabitacion no encontrado");

            TipoHabitacion tipoHabitacion = JsonConvert.DeserializeObject<TipoHabitacion>(respuesta);
            JsonConvert.PopulateObject(values, tipoHabitacion);

            string jsonString = JsonConvert.SerializeObject(tipoHabitacion);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using (var client = new HttpClient(handler))
            {
                var url = $"https://localhost:44321/api/TipoHabitacion/{key}";
                var response = await client.PutAsync(url, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return Request.CreateErrorResponse(response.StatusCode, error);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var apiUrl = $"https://localhost:44321/api/TipoHabitacion/{key}";

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using (var client = new HttpClient(handler))
            {
                var respuesta = await client.DeleteAsync(apiUrl);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}

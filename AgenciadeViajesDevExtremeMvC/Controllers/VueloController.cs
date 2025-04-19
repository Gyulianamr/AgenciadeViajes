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
    public class VueloController : ApiController
    {
       // Actualice el post,put,delete
        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44321/api/Vuelo";
            var respuestaJson = await GetAsync(apiUrl);

            List<Vuelo> listaVuelo = JsonConvert.DeserializeObject<List<Vuelo>>(respuestaJson);
            return Request.CreateResponse(DataSourceLoader.Load(listaVuelo, loadOptions));
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

            var url = "https://localhost:44321/api/Vuelo";
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

            // Obtener el vuelo actual
            var apiUrl = "https://localhost:44321/api/Vuelo/" + key;
            var respuestaJson = await GetAsync(apiUrl);
            Vuelo vuelo = JsonConvert.DeserializeObject<Vuelo>(respuestaJson);

            // Actualizar los valores del vuelo con los nuevos datos
            JsonConvert.PopulateObject(values, vuelo);

            // Obtener los datos del Destino
            var urlDestino = "https://localhost:44321/api/Destino/" + vuelo.DestinoId;
            var respuestaDestino = await GetAsync(apiUrl);
            Destino destino = JsonConvert.DeserializeObject<Destino>(respuestaDestino);
            vuelo.Destino = destino;

            // Obtener los datos del Origen
            var urlOrigen = "https://localhost:44321/api/Destino/" + vuelo.OrigenId;
            var respuestaOrigen = await GetAsync(apiUrl);
            Destino origen = JsonConvert.DeserializeObject<Destino>(respuestaOrigen);
            vuelo.Origen = origen;

            // Serializar el objeto actualizado
            var jsonString = JsonConvert.SerializeObject(vuelo);
            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using (var client = new HttpClient(handler))
            {
                var url = "https://localhost:44321/api/Vuelo/" + key;
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
            var apiUrlDel = "https://localhost:44321/api/Vuelo/" + key;

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using (var client = new HttpClient(handler))
            {
                var response = await client.DeleteAsync(apiUrlDel);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


    }
}

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
    public class CotizacionController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44321/api/Cotizaciones";
            var respuestaJson = await GetAsync(apiUrl);

            List<Cotizacion> listaCotizaciones = JsonConvert.DeserializeObject<List<Cotizacion>>(respuestaJson);
            return Request.CreateResponse(DataSourceLoader.Load(listaCotizaciones, loadOptions));
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

            var url = "https://localhost:44321/api/Cotizaciones";
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using (var client = new HttpClient(handler))
            {
                var response = await client.PostAsync(url, httpContent);
                var result = await response.Content.ReadAsStringAsync();
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var apiUrl = "https://localhost:44321/api/Cotizaciones/" + key;

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using (var client = new HttpClient(handler))
            {
                var response = await client.DeleteAsync(apiUrl);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            var apiUrl = "https://localhost:44321/api/Cotizaciones/" + key;
            var respuestaJson = await GetAsync(apiUrl);
            Cotizacion cotizacion = JsonConvert.DeserializeObject<Cotizacion>(respuestaJson);

            JsonConvert.PopulateObject(values, cotizacion);
            cotizacion.ActualizarCostoTotal(); // Asegurarse de actualizar el costo total

            string jsonString = JsonConvert.SerializeObject(cotizacion);
            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using (var client = new HttpClient(handler))
            {
                var url = "https://localhost:44321/api/Cotizaciones/" + key;
                var response = await client.PutAsync(url, httpContent);
                var result = await response.Content.ReadAsStringAsync();
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }




    }
}

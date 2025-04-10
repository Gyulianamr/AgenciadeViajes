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
    public class ReservacionController : ApiController
    {
       
            [HttpGet]
            public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
            {
                var apiUrl = "https://localhost:44321/api/Reservaciones";

                var respuestaJson = await GetAsync(apiUrl);
                //System.Diagnostics.Debug.WriteLine(respuestaJson); imprimir info
                List<Reservacion> listaReservacion = JsonConvert.DeserializeObject<List<Reservacion>>(respuestaJson);
                return Request.CreateResponse(DataSourceLoader.Load(listaReservacion, loadOptions));
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

                var url = "https://localhost:44321/api/Reservaciones";
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(handler))
                {
                    var response = await client.PostAsync(url, httpContent);

                    var result = response.Content.ReadAsStringAsync().Result;
                }

                return Request.CreateResponse(HttpStatusCode.Created);
            }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            var apiUrl = "https://localhost:44321/api/Reservaciones/" + key;
            var respuestaPelic = await GetAsync(apiUrl);

            if (respuestaPelic == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Reservacion no encontrada");

            Reservacion reservacion = JsonConvert.DeserializeObject<Reservacion>(respuestaPelic);
            JsonConvert.PopulateObject(values, reservacion);

            // Corrected URL usage here
            var apiUrl1 = "https://localhost:44321/api/Cotizaciones/" + reservacion.IdCotizacion;
            var respuestaJson1 = await GetAsync(apiUrl1); // Corrected variable from apiUrl to apiUrl1
            Cotizacion cotizacion = JsonConvert.DeserializeObject<Cotizacion>(respuestaJson1);
            reservacion.Cotizacion = cotizacion;

            string jsonString = JsonConvert.SerializeObject(reservacion);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using (var client = new HttpClient(handler))
            {
                var url = "https://localhost:44321/api/Reservaciones/" + key;
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

                var apiUrlDelPeli = "https://localhost:44321/api/Reservaciones/" + key;
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(handler))
                {
                    var respuestaPelic = await client.DeleteAsync(apiUrlDelPeli);
                }
                return Request.CreateResponse(HttpStatusCode.OK);

            }
        
    }
}

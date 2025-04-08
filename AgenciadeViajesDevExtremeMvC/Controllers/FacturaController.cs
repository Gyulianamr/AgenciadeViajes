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
    public class FacturaController : ApiController
    {

            [HttpGet]
  
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44321/api/Facturas";
            var respuestaJson = await GetAsync(apiUrl);

            if (string.IsNullOrEmpty(respuestaJson))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No data received from the API");
            }

            List<Factura> listaFactura = JsonConvert.DeserializeObject<List<Factura>>(respuestaJson);
            if (listaFactura == null || !listaFactura.Any())
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No invoices found");
            }

            return Request.CreateResponse(DataSourceLoader.Load(listaFactura, loadOptions));
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

                    // Verifica el código de estado HTTP
                    if (!response.IsSuccessStatusCode)
                    {
                        // Si no es exitoso, registra el error
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        return $"Error: {response.StatusCode}, Message: {errorMessage}";
                    }

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                // En caso de una excepción
                return $"Exception: {e.Message}";
            }
        }



        [HttpPost]
            public async Task<HttpResponseMessage> Post(FormDataCollection form)
            {
                var values = form.Get("values");
                var httpContent = new StringContent(values, System.Text.Encoding.UTF8, "application/json");

                var url = "https://localhost:44321/api/Facturas";
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
                var apiUrl = "https://localhost:44321/api/Facturas/" + key;

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

            var apiUrl = "https://localhost:44321/api/Facturas/" + key;
            var respuestaJson = await GetAsync(apiUrl);
            Factura factura = JsonConvert.DeserializeObject<Factura>(respuestaJson);
            JsonConvert.PopulateObject(values, factura);


            var apiUrl1 = "https://localhost:44321/api/Reservaciones/" + factura.ReservacionId;
            var respuestaJson1 = await GetAsync(apiUrl);
            Reservacion reserva = JsonConvert.DeserializeObject<Reservacion>(respuestaJson1);

            factura.Reservacion = reserva;

            var apiUrlM = "https://localhost:44321/api/Metodo_Pago/" + factura.MetodoPagoId;
            var respuestaJsonM = await GetAsync(apiUrl);
            Metodo_Pago metodo = JsonConvert.DeserializeObject<Metodo_Pago>(respuestaJsonM);

            factura.MetodoPago = metodo;


            string jsonString = JsonConvert.SerializeObject(factura);
            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using (var client = new HttpClient(handler))
            {
                var url = "https://localhost:44321/api/Facturas/" + key;
                var response = await client.PutAsync(url, httpContent);
                var result = await response.Content.ReadAsStringAsync();
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }








    }
}


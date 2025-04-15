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
    public class SeguroController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44321/api/Seguro";

            var respuestaJson = await GetAsync(apiUrl);
            //System.Diagnostics.Debug.WriteLine(respuestaJson); imprimir info
            List<Seguro> listaseguro = JsonConvert.DeserializeObject<List<Seguro>>(respuestaJson);
            return Request.CreateResponse(DataSourceLoader.Load(listaseguro, loadOptions));
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
        [HttpPost] //Actualizacion de metodos nuevos para seguro 
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var values = form.Get("values");
            var httpContent = new StringContent(values, System.Text.Encoding.UTF8, "application/json");

            var url = "https://localhost:44321/api/Seguro";
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

            var apiUrl = $"https://localhost:44321/api/Seguro/{key}";
            var respuesta = await GetAsync(apiUrl);

            if (respuesta == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Seguro no encontrado");

            Seguro seguro = JsonConvert.DeserializeObject<Seguro>(respuesta);
            JsonConvert.PopulateObject(values, seguro);

            string jsonString = JsonConvert.SerializeObject(seguro);
            System.Diagnostics.Debug.WriteLine(jsonString);

            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using (var client = new HttpClient(handler))
            {
                var url = $"https://localhost:44321/api/Seguro/{key}";
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
            var apiUrl = $"https://localhost:44321/api/Seguro/{key}";

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using AgenciadeViajesDevExtremeMvC.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;

namespace AgenciadeViajesDevExtremeMvC.Controllers {
    public class SampleDataController : ApiController {

        private static readonly HttpClient client = new HttpClient();
        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44321/api/Cliente";
            var respuestaJson = await GetAsync(apiUrl);
            //System.Diagnostics.Debug.WriteLine(respuestaJson); imprimir info
            List<Cliente> listacliente = JsonConvert.DeserializeObject<List<Cliente>>(respuestaJson);
            return Request.CreateResponse(DataSourceLoader.Load(listacliente, loadOptions));
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

                //var response = await client.GetAsync(uri);

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

            var url = "https://localhost:44321/api/Cliente";
            var response = await client.PostAsync(url, httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            //Parámetros del form
            var key = Convert.ToInt32(form.Get("key")); //llave que estoy modificando
            var values = form.Get("values"); //Los valores que yo modifiqué en formato JSON

            var apiUrlGetPeli = "https://localhost:44321/api/Cliente/" + key;
            var respuestaPelic = await GetAsync(apiUrlGetPeli = "https://localhost:44321/api/Cliente/" + key);
            Cliente cliente = JsonConvert.DeserializeObject<Cliente>(respuestaPelic);

            JsonConvert.PopulateObject(values, cliente);

            string jsonString = JsonConvert.SerializeObject(cliente);
            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");


            var url = "https://localhost:44321/api/Cliente/" + key;
            var response = await client.PutAsync(url, httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));

            var apiUrlDelPeli = "https://localhost:44321/api/Cliente/" + key;
            var respuestaPelic = await client.DeleteAsync(apiUrlDelPeli);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
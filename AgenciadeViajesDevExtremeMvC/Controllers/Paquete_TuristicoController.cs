using AgenciadeViajesDevExtremeMvC.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AgenciadeViajesDevExtremeMvC.Controllers
{
    public class Paquete_TuristicoController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44321/api/PaquetesTuristicos";

            var respuestaJson = await GetAsync(apiUrl);
            //System.Diagnostics.Debug.WriteLine(respuestaJson); imprimir info
            List<Paquete_Turistico> listapaquete = JsonConvert.DeserializeObject<List<Paquete_Turistico>>(respuestaJson);
            return Request.CreateResponse(DataSourceLoader.Load(listapaquete, loadOptions));
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

            var url = "https://localhost:44321/api/PaquetesTuristicos";
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
            try
            {
                // Validar y obtener los datos del formulario
                if (form == null || string.IsNullOrEmpty(form.Get("key")) || string.IsNullOrEmpty(form.Get("values")))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Los datos del formulario son inválidos.");
                }

                var key = Convert.ToInt32(form.Get("key"));
                var values = form.Get("values");

                // Obtener el paquete turístico existente
                var apiUrl = $"https://localhost:44321/api/PaquetesTuristicos/{key}";
                var respuestaJson = await GetAsync(apiUrl);
                if (string.IsNullOrEmpty(respuestaJson))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Paquete turístico no encontrado.");
                }

                // Deserializar el JSON como un solo Paquete_Turistico
                Paquete_Turistico paquete;
                try
                {
                    // Verificamos si viene como lista o como objeto
                    if (respuestaJson.TrimStart().StartsWith("["))
                    {
                        var paquetes = JsonConvert.DeserializeObject<List<Paquete_Turistico>>(respuestaJson);
                        if (paquetes == null || paquetes.Count == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron paquetes turísticos en la respuesta.");
                        }
                        paquete = paquetes[0];
                    }
                    else
                    {
                        paquete = JsonConvert.DeserializeObject<Paquete_Turistico>(respuestaJson);
                        if (paquete == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "El paquete turístico no pudo ser deserializado.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error al deserializar el JSON: {ex.Message}");
                }

                // Actualizar el paquete turístico con los nuevos valores
                JsonConvert.PopulateObject(values, paquete);

                // Obtener datos relacionados (Destino, Hotel, Actividades, etc.)
                var tasks = new List<Task>
        {
            PopulateRelatedEntity<Destino>(paquete.DestinoId, "Destino", destino => paquete.Destino = destino),
            PopulateRelatedEntity<Hotel>(paquete.HotelId ?? 0, "Hotel", hotel => paquete.Hotel = hotel),
            PopulateRelatedEntity<Actividades>(paquete.ActividadesId ?? 0, "Actividades", actividades => paquete.Actividades = actividades),
            PopulateRelatedEntity<Vuelo>(paquete.VueloId ?? 0, "Vuelo", vuelo => paquete.Vuelo = vuelo),
            PopulateRelatedEntity<GuiaTuristico>(paquete.GuiaTuristicoId ?? 0, "GuiaTuristico", guia => paquete.GuiaTuristico = guia),
            PopulateRelatedEntity<Seguro>(paquete.SeguroId ?? 0, "Seguro", seguro => paquete.Seguro = seguro)
        };

                await Task.WhenAll(tasks);

                // Serializar el paquete actualizado
                var jsonString = JsonConvert.SerializeObject(paquete);
                var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using (var client = new HttpClient(handler))
                {
                    var url = $"https://localhost:44321/api/PaquetesTuristicos/{key}";
                    var response = await client.PutAsync(url, httpContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return Request.CreateErrorResponse((HttpStatusCode)response.StatusCode, errorContent);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Paquete turístico actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        private async Task PopulateRelatedEntity<T>(int id, string endpoint, Action<T> setEntity) where T : class
        {
            if (id == 0) return;

            var url = $"https://localhost:44321/api/{endpoint}/{id}";
            var json = await GetAsync(url);

            if (string.IsNullOrEmpty(json)) return;

            T entity;

            try
            {
                if (json.TrimStart().StartsWith("["))
                {
                    var list = JsonConvert.DeserializeObject<List<T>>(json);
                    entity = list?.FirstOrDefault();
                }
                else
                {
                    entity = JsonConvert.DeserializeObject<T>(json);
                }

                if (entity != null)
                {
                    setEntity(entity);
                }
            }
            catch (Exception ex)
            {
                // Puedes hacer log si quieres aquí
                Console.WriteLine($"Error al deserializar {endpoint}: {ex.Message}");
            }
        }




        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));

            var apiUrlDelPeli = "https://localhost:44321/api/PaquetesTuristicos/" + key;
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

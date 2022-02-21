using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Client.Model;

namespace Client.Logic
{
    static class Requests
    {
        private static readonly HttpClient CLIENT = new HttpClient();
        private static string URL_TO_GET_CARDS = "/api/cards";
        private static string URL_TO_GET_CARD = "/api/cards";
        private static string URL_TO_DELETE_CARD = "/api/cards";
        private static string URL_TO_UPDATE_CARD= "/api/cards/update";
        private static string URL_TO_CREATE_CARD = "/api/cards";
        private static string URL;


        async private static Task<string> getRequest(string url)
        {
            HttpResponseMessage response = await CLIENT.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else throw new Exception("Bad request!");
        }

        async private static Task<string> postRequest(string url, HttpContent data)
        {
            HttpResponseMessage response = await CLIENT.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else throw new Exception("Bad request!");
        }

        async private static Task<string> deleteRequest(string url)
        {
            HttpResponseMessage response = await CLIENT.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else throw new Exception("Bad request!");
        }


        public static bool checkUrl(string url)
        {
            try{ return Task.Run(() => getRequest(url + URL_TO_GET_CARDS)).Result.Length>0;}
            catch (Exception) { return false; }
        }

        public static void setUrl(string url) { Requests.URL = url; }

        public static ProductCard[] getCard(int count, int offset, string sort = "")
        {
            string url = URL + URL_TO_GET_CARDS + "?count=" + count + "&sort=" + sort + "&offset=" + offset;
            string data = Task.Run(() => getRequest(url)).Result;
            return JsonSerializer.Deserialize<ProductCard[]>(data);
        }

        public static ProductCard getCardById(int id)
        {
            string url = URL + URL_TO_GET_CARD + "/" + id;
            string data = Task.Run(() => getRequest(url)).Result;
            return JsonSerializer.Deserialize<ProductCard>(data);
        }

        public static void deleteCardById(int id)
        {
            string url = URL + URL_TO_DELETE_CARD + "/" + id;
            string data = Task.Run(() => deleteRequest(url)).Result;
        }

        public static void updateCard(ProductCard card)
        {
            string url = URL + URL_TO_UPDATE_CARD;
            string data = Task.Run(() => postRequest(url, new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json"))).Result;
        }

        public static void createCard(ProductCard card)
        {
            string url = URL + URL_TO_CREATE_CARD;
            string data = Task.Run(() => postRequest(url, new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json"))).Result;
        }

        public static string getRootUrl() { return URL; }
    }
}

using Data;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace JavBusDownloader
{
    internal class WebAPI
    {
        //ApiMovies mv = GetMovies("https://javbus-api-jtl1207.vercel.app/api/movies");
        ApiMovies GetMovies(string url, int page = 1, string filterType = "", string filterValue = "", string type = "normal")
        {
            try
            {
                RestClient client = new RestClient(url);
                RestResponse response = (RestResponse)client.Execute(new RestRequest());

                if (response == null | response.Content == null | response.ContentType != "application/json" | !response.IsSuccessful)
                {
                    return null;
                }
                ApiMovies movies = JsonConvert.DeserializeObject<ApiMovies>(response.Content);
                return movies;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        ApiMovies SearchMovies(string url,string keyword = "", int page = 1, string type = "normal")
        {
            try
            {
                RestClient client = new RestClient($"{url}/search");
                client.AddDefaultHeader("keyword", keyword);
                client.AddDefaultHeader("page", page.ToString());
                client.AddDefaultHeader("type", type);
                RestResponse response = client.Execute(new RestRequest());

                if (response == null | response.Content == null | response.ContentType != "application/json" | !response.IsSuccessful)
                {
                    return null;
                }
                ApiMovies movies = JsonConvert.DeserializeObject<ApiMovies>(response.Content);
                return movies;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        ApiMovieInfo GetMovieInfo(string url,string moviename )
        {
            try
            {
                RestClient client = new RestClient($"{url}/{moviename}");
                RestResponse response = (RestResponse)client.Execute(new RestRequest());

                if (response == null | response.Content == null | response.ContentType != "application/json" | !response.IsSuccessful)
                {
                    return null;
                }
                ApiMovieInfo movies = JsonConvert.DeserializeObject<ApiMovieInfo>(response.Content);
                return movies;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

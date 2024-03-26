using Data;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace JavBusDownloader
{
    internal class WebAPI
    {
        ApiMovies GetMovies(string url, int page = 1, string filterType = "", string filterValue = "", string type = "normal")
        {
            try
            {
                RestClient client = new RestClient(url);
                client.AddDefaultHeader("page", page.ToString());
                if (filterType != "")
                {
                    client.AddDefaultHeader("filterType", filterType);
                    client.AddDefaultHeader("filterValue", filterValue);
                }
                client.AddDefaultHeader("type", type);
                RestResponse response = client.Execute(new RestRequest());

                if (response == null | response.Content == null | response.ContentType != "application/json" | !response.IsSuccessful)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<ApiMovies>(response.Content);
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
                return JsonConvert.DeserializeObject<ApiMovies>(response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        ApiMovieInfo GetMovieInfo(string url,string moviename)
        {
            try
            {
                RestClient client = new RestClient($"{url}/{moviename}");
                RestResponse response = (RestResponse)client.Execute(new RestRequest());

                if (response == null | response.Content == null | response.ContentType != "application/json" | !response.IsSuccessful)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<ApiMovieInfo>(response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        ApiMagnets[] GetMovieMagnets(string url, string moviename , int gid , int uc ,string sortBy = "date" , string sortOrder = "desc")
        {
            try
            {
                RestClient client = new RestClient($"{url}/magnets/{moviename}");
                client.AddDefaultHeader("gid", gid.ToString());
                client.AddDefaultHeader("uc", uc.ToString());
                client.AddDefaultHeader("sortBy", sortBy);
                client.AddDefaultHeader("sortOrder", sortOrder);

                RestResponse response = (RestResponse)client.Execute(new RestRequest());

                if (response == null | response.Content == null | response.ContentType != "application/json" | !response.IsSuccessful)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<ApiMagnets[]>(response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

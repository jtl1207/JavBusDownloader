using Data;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Threading;

namespace JavBusDownloader
{
    internal static class OpenAPI
    {
        internal static ApiMovies GetMoviesAsync(string url, int page = 1, string filterType = "", string filterValue = "", string type = "normal")
        {
            int maxAttempts = 3;
            int currentAttempt = 0;
            IRestResponse response = null;
            Exception lastException = null;
            while (currentAttempt < maxAttempts)
            {
                currentAttempt++;

                try
                {
                    RestClient client = new RestClient($"{url}/movies");
                    client.AddDefaultParameter("page", page.ToString(), ParameterType.QueryString);
                    if (filterType != "")
                    {
                        client.AddDefaultParameter("magnet", "exist", ParameterType.QueryString);
                        client.AddDefaultParameter("filterType", filterType, ParameterType.QueryString);
                        client.AddDefaultParameter("filterValue", filterValue, ParameterType.QueryString);
                    }
                    client.AddDefaultParameter("type", type, ParameterType.QueryString);
                    response = client.Execute(new RestRequest());

                    if (response != null && response.Content != null && response.IsSuccessful)
                    {
                        return JsonConvert.DeserializeObject<ApiMovies>(response.Content);
                    }
                    else
                    {
                        lastException = null;
                    }
                }
                catch (IOException ex)
                {
                    lastException = ex;
                    Console.WriteLine($"尝试 {currentAttempt}/{maxAttempts} 失败，等待重试...");
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"发生异常：{ex.Message}");
                    throw;
                }
            }

            if (lastException != null)
            {
                Console.WriteLine($"重连 {maxAttempts} 次仍然失败，抛出异常：{lastException.Message}");
                throw lastException;
            }

            Console.WriteLine($"重连 {maxAttempts} 次后仍然无法获取响应，返回 null");
            return null;
        }
        internal static ApiMovies SearchMovies(string url,string keyword = "", int page = 1, string type = "normal")
        {
            try
            {
                RestClient client = new RestClient($"{url}/search");
                client.AddDefaultParameter("magnet", "exist", ParameterType.QueryString);
                client.AddDefaultParameter("keyword", keyword, ParameterType.QueryString);
                client.AddDefaultParameter("page", page.ToString(), ParameterType.QueryString);
                client.AddDefaultParameter("type", type, ParameterType.QueryString);
                IRestResponse response = client.Execute(new RestRequest());
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ApiMovies();
                }
                if (!response.IsSuccessful)
                {
                    Console.WriteLine($"Error: {response.ErrorMessage}");
                    return null;
                }
                if (response == null | response.Content == null | !response.IsSuccessful)
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
        internal static ApiMovieInfo GetMovieInfo(string url,string moviename)
        {
            try
            {
                RestClient client = new RestClient($"{url}/movies/{moviename}");
                RestResponse response = (RestResponse)client.Execute(new RestRequest());

                if (response == null | response.Content == null | !response.IsSuccessful)
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

        internal static ApiMagnets[] GetMovieMagnets(string url, string moviename , string gid , string uc ,string sortBy = "date" , string sortOrder = "desc")
        {
            try
            {
                RestClient client = new RestClient($"{url}/magnets/{moviename}");
                client.AddDefaultParameter("gid", gid, ParameterType.QueryString);
                client.AddDefaultParameter("uc", uc, ParameterType.QueryString);
                client.AddDefaultParameter("sortBy", sortBy, ParameterType.QueryString);
                client.AddDefaultParameter("sortOrder", sortOrder, ParameterType.QueryString);

                RestResponse response = (RestResponse)client.Execute(new RestRequest());

                if (response == null | response.Content == null | !response.IsSuccessful)
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

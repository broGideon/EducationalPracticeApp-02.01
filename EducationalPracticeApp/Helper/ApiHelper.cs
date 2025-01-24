using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace EducationalPracticeApp.Helper;

public class ApiHelper
{
     private readonly string _baseUrl = "http://localhost:5166";

    public async Task<T?> GetFiltered<T>(string model, string token = "")
    {
        var client = new HttpClient();
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"{_baseUrl}/{model}");
        if (response.StatusCode != HttpStatusCode.OK) return default;
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
    }

    public async Task<T?> Get<T>(string model, int id = 0, string token = "")
    {
        var client = new HttpClient();
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"{_baseUrl}/{model}/{(id != 0 ? id : string.Empty)}");
        if (response.StatusCode != HttpStatusCode.OK) return default;
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
    }

    public async Task<T?> Post<T>(T entity, string model, string token = "")
    {
        var client = new HttpClient();
        string json = JsonConvert.SerializeObject(entity);
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_baseUrl}/{model}", body);
        if (response.StatusCode == HttpStatusCode.OK) return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        return default;
    }

    public async Task<bool> Put<T>(T entity, string model, int id, string token = "")
    {
        var client = new HttpClient();
        string json = JsonConvert.SerializeObject(entity);
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"{_baseUrl}/{model}/{id}", body);
        if (response.StatusCode == HttpStatusCode.OK) return true;
        return false;
    }

    public async Task<bool> Delete(string model, int id, string token = "")
    {
        var client = new HttpClient();
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.DeleteAsync($"{_baseUrl}/{model}/{id}");
        if (response.StatusCode == HttpStatusCode.OK) return true;
        return false;
    }

}
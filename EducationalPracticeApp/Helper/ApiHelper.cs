using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using EducationalPracticeApp.Models;
using Newtonsoft.Json;
using EducationalPracticeApp.Properties;

namespace EducationalPracticeApp.Helper;

public class ApiHelper
{
    private readonly string _baseUrl = "http://localhost:5166";

    private async Task<string> RefreshToken()
    {
        string? refreshToken = TokenHelper.LoadRefreshToken(); 
        var token = new RefreshToken(refreshToken!);
        var client = new HttpClient();
        HttpContent content = new StringContent(JsonConvert.SerializeObject(token), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_baseUrl}/refresh", content);
        if (!response.IsSuccessStatusCode) throw new Exception();
        AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync())!;
        TokenHelper.SaveRefreshToken(authResponse.RefreshToken);
        return authResponse.Token;
    }

    public async Task<T?> Get<T>(string model, int id = 0)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await RefreshToken());
        var response = await client.GetAsync($"{_baseUrl}/{model}/{(id != 0 ? id : string.Empty)}");
        if (response.StatusCode != HttpStatusCode.OK) return default;
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
    }

    public async Task<T?> Post<T>(T entity, string model)
    {
        var client = new HttpClient();
        string json = JsonConvert.SerializeObject(entity);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await RefreshToken());
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_baseUrl}/{model}", body);
        if (response.StatusCode == HttpStatusCode.OK) return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        return default;
    }

    public async Task<bool> Put<T>(T entity, string model, int id)
    {
        var client = new HttpClient();
        string json = JsonConvert.SerializeObject(entity);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await RefreshToken());
        HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"{_baseUrl}/{model}/{id}", body);
        if (response.StatusCode == HttpStatusCode.OK) return true;
        return false;
    }

    public async Task<bool> Delete(string model, int id)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await RefreshToken());
        var response = await client.DeleteAsync($"{_baseUrl}/{model}/{id}");
        if (response.StatusCode == HttpStatusCode.OK) return true;
        return false;
    }

}
using FileCloudClient.Abstractions;
using FileCloudClient.Models;
using System.Net.Http.Json;

namespace FileCloudClient.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public AdminService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FileCloud");
        }

        public async Task<List<UserInfoModel>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/users");
            return await response.Content.ReadFromJsonAsync<List<UserInfoModel>>();
        }
    }
}

using SparePartsStoreWeb.Data.ApiClient;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories.SparePartRepository
{
    public class SparePartRepository : ISparePartRepository
    {
        private readonly IApiClient _client;

        public SparePartRepository(IApiClient client)
        {
            _client = client;
        }

        public async Task<List<SparePart>?> GetAll()
        {
            HttpResponseMessage response = await _client.Get<SparePart>("all");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<SparePart>>();
            }
            return null;
        }

        public async Task<SparePart?> GetById(int id)
        {
            HttpResponseMessage response = await _client.Get<SparePart>($"byId/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SparePart>();
            }
            return null;
        }

        public async Task<SparePart?> GetByCategory(string categoryName)
        {
            HttpResponseMessage response = await _client.Get<SparePart>($"byCategoryName/{categoryName}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SparePart>();
            }
            return null;
        }

        public async Task<bool> Create(SparePart sparePart)
        {
            HttpResponseMessage response = await _client.Post("create", sparePart);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(SparePart sparePart)
        {
            HttpResponseMessage response = await _client.Put("update", sparePart);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            HttpResponseMessage response = await _client.Delete<SparePart>($"delete/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

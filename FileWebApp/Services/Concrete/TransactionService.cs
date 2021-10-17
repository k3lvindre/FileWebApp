using FileWebApp.API.Model;
using FileWebApp.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileWebApp.Services
{
    public class TransactionService : ITransactionService
    {
        private static HttpClient _httpClient = new HttpClient();
        private readonly FileWebAppApiConfig _fileWebAppApiConfig;
        private readonly TransactionApiConfig _transactionApiConfig;

        public TransactionService(IOptions<FileWebAppApiConfig> fileWebAppApiConfig
                                 ,IOptions<TransactionApiConfig> transactionApiConfig)
        {
            _fileWebAppApiConfig = fileWebAppApiConfig.Value;
            _transactionApiConfig = transactionApiConfig.Value;

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri($"{_fileWebAppApiConfig.MainEndpoint}:{_fileWebAppApiConfig.Port}");
            }
        }

        public async Task<bool> UploadTransactionsAsync(List<Transaction> transactions)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_fileWebAppApiConfig.MediaType));
            HttpContent param = new StringContent(JsonSerializer.Serialize<List<Transaction>>(transactions), Encoding.Default, _fileWebAppApiConfig.MediaType);
            
            HttpResponseMessage result = await _httpClient.PostAsync($"{_transactionApiConfig.MainEndpoint}/{_transactionApiConfig.SaveList}", param);

            return result.IsSuccessStatusCode;
        }
    }
}

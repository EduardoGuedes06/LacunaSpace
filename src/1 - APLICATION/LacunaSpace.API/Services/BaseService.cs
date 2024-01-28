using System.Text.Json;
using System.Text;
using System.Net.Http;
using FluentValidation;
using FluentValidation.Results;
using LacunaSpace.Domain.Intefaces;
using LacunaSpace.Domain.Notificacoes;
using System.Net.Http.Headers;



namespace LacunaSpace.Service
{
    public abstract class BaseService
    {
        private readonly HttpClient _httpClient;
        private readonly INotificador _notificador;

        protected BaseService(INotificador notificador, HttpClient httpClient)
        {
            _notificador = notificador;
            _httpClient = httpClient;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        public string GerarToken()
        {
            try
            {
                const string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                const int comprimentoToken = 50;

                char[] token = new char[comprimentoToken];

                using (var gerador = new System.Security.Cryptography.RNGCryptoServiceProvider())
                {
                    byte[] bytes = new byte[comprimentoToken];

                    gerador.GetBytes(bytes);

                    for (int i = 0; i < comprimentoToken; i++)
                    {
                        token[i] = caracteresPermitidos[bytes[i] % caracteresPermitidos.Length];
                    }
                }

                return new string(token);
            }
            catch (Exception ex)
            {
                Notificar("Erro ao gerar o Token " + ex.Message);
                return null;
            }
        }

        public async Task<T> GetAsync<T>(string url)
       {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                return await HandleResponse<T>(response);
       }

        public async Task<T> PostAsync<T>(string url, object body)
            {
                string jsonBody = JsonSerializer.Serialize(body);
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                return await HandleResponse<T>(response);
            }

        public async Task<T> PutAsync<T>(string url, object body)
            {
                string jsonBody = JsonSerializer.Serialize(body);
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(url, content);

                return await HandleResponse<T>(response);
            }

        public async Task<T> DeleteAsync<T>(string url)
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                return await HandleResponse<T>(response);
            }
        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(jsonResponse);
                }
                else
                {
                    // Lidar com erros, lançar exceção ou retornar um objeto com informações de erro
                    throw new HttpRequestException($"Erro na requisição. Código de status: {response.StatusCode}");
                }
            }
        public async Task<T> PostWithTokenAsync<T>(string url, object body, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                string jsonBody = JsonSerializer.Serialize(body);
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                Notificar($"Erro na requisição POST: {ex.Message}");
                throw;
            }
        }
        public async Task<T> GetWithTokenAsync<T>(string url, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            return await HandleResponse<T>(response);
        }




    }
}
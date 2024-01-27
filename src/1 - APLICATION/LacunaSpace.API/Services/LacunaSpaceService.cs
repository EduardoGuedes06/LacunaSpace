
using GameMasterEnterprise.Domain.Intefaces;
using LacunaSpace.API.Models.Request;
using LacunaSpace.API.Models.Response;
using LacunaSpace.Domain.Intefaces;
using System.Net.Http.Headers;
using System.Net.Http;
using LacunaSpace.API.Models;

namespace LacunaSpace.Service
{
    public class LacunaSpaceService : BaseService, ILacunaSpaceService
    {

        public LacunaSpaceService(INotificador notificador, HttpClient httpClient)
            : base(notificador, httpClient)
        {

        }

        public async Task<string> IniciarTeste(StartRequestModel requestModel)
        {
            try
            {
                var response = await PostAsync<StartBaseResponseModel>("https://luma.lacuna.cc/api/start", requestModel);
                if (response.code == "Success")
                {
                    return response.accessToken;
                }
                else
                {
                    Notificar("Erro");
                    throw new Exception($"Falha ao iniciar o teste");
                }
            }
            catch (Exception ex)
            {
                Notificar($"Erro ao iniciar o teste: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ProbeModel>> ListarSondas(string accessToken)
        {
            try
            {

                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Chamar a API para listar sondas usando o BaseService
                var response = await GetAsync<ProbeListResponseModel>("https://luma.lacuna.cc/api/probe");

                // Verificar a resposta da API de listar sondas
                if (response.code == "Success")
                {
                    return response.probes;
                }
                else
                {
                    Notificar("Erro ao listar sondas");
                    throw new Exception($"Falha ao listar sondas: {response.message}");
                }
            }
            catch (Exception ex)
            {
                Notificar($"Erro ao listar sondas: {ex.Message}");
                throw;
            }
        }
    }
}

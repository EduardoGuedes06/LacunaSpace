using LacunaSpace.API.Models;
using LacunaSpace.API.Models.Request;
using LacunaSpace.API.Models.Response;

namespace GameMasterEnterprise.Domain.Intefaces
{
    public interface ILacunaSpaceService
    {
        Task IniciarTarefa(string accessToken);
        Task<string> IniciarTeste(StartRequestModel requestModel);
        Task<List<ProbeModel>> ListarSondas(string accessToken);
        List<ProbeSyncInfoModel> ObterDadosSincronizados();
        Task SincronizarRelogios(string accessToken);
        Task VerificaTarefa(string idJob, string accessToken, string job);
    }
}
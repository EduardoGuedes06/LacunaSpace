using LacunaSpace.API.Models;
using LacunaSpace.API.Models.Request;
using LacunaSpace.API.Models.Response;

namespace GameMasterEnterprise.Domain.Intefaces
{
    public interface ILacunaSpaceService
    {
        Task<string> IniciarTeste(StartRequestModel requestModel);
        Task<List<ProbeModel>> ListarSondas(string accessToken);
        Task SincronizarRelogios(ProbeListRequestModel Sondas, string accessToken);
    }
}
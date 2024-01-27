using LacunaSpace.API.Models.Request;

namespace GameMasterEnterprise.Domain.Intefaces
{
    public interface ILacunaSpaceService
    {
        Task<string> IniciarTeste(StartRequestModel requestModel);
    }
}
using AutoMapper;
using GameMasterEnterprise.Domain.Intefaces;
using LacunaSpace.API.Controllers;
using LacunaSpace.API.Models;
using LacunaSpace.API.Models.Request;
using LacunaSpace.API.Models.Response;
using LacunaSpace.Domain.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace Ipet.API.Controllers
{

    [Route("Prototipo")]
    public class PrototipoLacunaSpaceController : HomeController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILacunaSpaceService _lacunaSpaceService;

        private readonly ILogger _logger;
        public PrototipoLacunaSpaceController(
            ILacunaSpaceService lacunaSpaceService ,IMapper mapper, INotificador notificador) : base(notificador)
        {
            _lacunaSpaceService = lacunaSpaceService;
            _mapper = mapper;
        }




        [AllowAnonymous]
        [HttpPost("api/start")]
        public async Task<ActionResult> CriarSessao(StartRequestModel request)
        {

            var retorno = await _lacunaSpaceService.IniciarTeste(request);
            

            return Ok(retorno);

        }

        [AllowAnonymous]
        [HttpPost("api/listar-sondas")]
        public async Task<ActionResult> ListarSondas(string accessToken)
        {
            try
            {
                var sondas = await _lacunaSpaceService.ListarSondas(accessToken);
                return Ok(new { Sondas = sondas });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost("api/sincronizar-sondas")]
        public async Task<ActionResult<List<ProbeSyncInfoModel>>> SincronizarSondas(string accessToken)
        {
            try
            {
                await _lacunaSpaceService.SincronizarRelogios(accessToken);
                var dadosSincronizados = _lacunaSpaceService.ObterDadosSincronizados();
                return Ok(dadosSincronizados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }




    }
}


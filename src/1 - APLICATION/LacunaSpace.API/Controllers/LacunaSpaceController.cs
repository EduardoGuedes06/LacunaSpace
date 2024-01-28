using AutoMapper;
using GameMasterEnterprise.Domain.Intefaces;
using LacunaSpace.API.Controllers;
using LacunaSpace.API.Models.Request;
using LacunaSpace.Domain.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace Ipet.API.Controllers
{

    [Route("API")]
    public class LacunaSpaceController : HomeController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILacunaSpaceService _lacunaSpaceService;

        private readonly ILogger _logger;
        public LacunaSpaceController(
            ILacunaSpaceService lacunaSpaceService ,IMapper mapper, INotificador notificador) : base(notificador)
        {
            _lacunaSpaceService = lacunaSpaceService;
            _mapper = mapper;
        }




        [AllowAnonymous]
        [HttpPost("Juntando-tudo")]
        public async Task<ActionResult> JuntarTudo(StartRequestModel request)
        {

            var retorno = await _lacunaSpaceService.IniciarTeste(request);
            

            return Ok(retorno);

        }



    }
}


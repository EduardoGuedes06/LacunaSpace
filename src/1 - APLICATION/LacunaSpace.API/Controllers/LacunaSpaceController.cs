using AutoMapper;
using LacunaSpace.API.Controllers;
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

        private readonly ILogger _logger;
        public LacunaSpaceController(
            IMapper mapper, INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpPost("api/start")]
        public async Task<ActionResult> CriarCassino()
        {


            

            return Ok();

        }

    }
}


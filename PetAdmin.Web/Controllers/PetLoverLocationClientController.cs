using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Services;
using System.Threading.Tasks;

namespace PetAdmin.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PetLoverLocationClientController : BaseController
    {
        private readonly PetLoverLocationClientService _petLoverLocationClientService;

        public PetLoverLocationClientController(
             PetLoverLocationClientService petLoverLocationClientService,
            UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _petLoverLocationClientService = petLoverLocationClientService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PetLoverLocationClientDto dto)
        {
            _petLoverLocationClientService.CreateAndUpdateListByClientUid(dto.ClientUid);

            return await Response(null, _petLoverLocationClientService.Notifications);
        }
    }
}

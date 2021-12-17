using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Continental.API.WebApi.Controllers
{
    //[Authorize]. HJP Comentar para que no pida el token
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]/[action]")]
    public abstract class BaseApiController : Controller
    {
        private readonly IMapper mapper;
        protected IMapper Mapper => mapper ?? HttpContext.RequestServices.GetService<IMapper>();

        protected string _getToken()
        {
            return HttpContext.Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer", "");
        }
    }
}
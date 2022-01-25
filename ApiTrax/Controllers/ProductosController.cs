using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTrax.Controllers
{
    [Route("api/AgregarProducto")]
    [ApiController]
    [Authorize]
    public class ProductosController : ControllerBase
    {
    }
}

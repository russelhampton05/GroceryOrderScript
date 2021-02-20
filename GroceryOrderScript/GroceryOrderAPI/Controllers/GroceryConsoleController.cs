using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroceryOrderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroceryConsoleController : ControllerBase
    {

        [HttpGet]
        public async Task Get()
        {
            await GroceryOrderScript.Program.Main(null);
        }
    }
}

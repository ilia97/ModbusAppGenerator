using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModbusAppGenerator.Core.Models;

namespace ModbusAppGenerator.Controllers
{
    public abstract class BaseController : Controller
    {
        public string GetCurrentUserId()
        {
            return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
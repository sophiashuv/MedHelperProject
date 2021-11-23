using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MyApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected int GetCurrentUserId()
        {
            var userId = ControllerContext.HttpContext.User.Claims.Where(obj => 
                    obj.Type == "DoctorID")
                .Select(obj => obj.Value).SingleOrDefault();
            if (userId is null)
                throw new SecurityTokenValidationException("Invalid token");
            return int.Parse(userId);
        }
    }
}
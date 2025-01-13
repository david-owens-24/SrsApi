using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrsApi.Classes.ApiResponses;
using SrsApi.Classes.SrsItemLevelController;
using SrsApi.DbContext;
using SrsApi.Interfaces;

namespace SrsApi.Controllers
{
    
    public class BaseController : ControllerBase
    {
        //private readonly UserManager<IdentityUser> _userManager;

        //public BaseController(UserManager<IdentityUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        [NonAction]
        protected ActionResult SuccessResponse(object? body = null)
        {
            var response = new SrsApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Response = body
            };

            return Ok(response);           
        }

        [NonAction]
        protected ActionResult NotFoundResponse(string? errorMessage = null)
        {
            return NotFound(new SrsApiResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessage = errorMessage
            });
        }

        [NonAction]
        protected ActionResult ErrorResponse(string? errorMessage, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError, object? body = null, string? errorCode = null)
        {
            var response = new SrsApiResponse
            {
                StatusCode = httpStatusCode,
                Response = body,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };

            return StatusCode((int)httpStatusCode, response);
        }

        [NonAction]
        protected ActionResult ErrorResponseFromException(Exception exception, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError, object? body = null, string? errorCode = null)
        {
            SrsApiResponse response = new SrsApiResponse
            {
                StatusCode = httpStatusCode,
                Response = body,
                ErrorCode = errorCode
            };

            response.SetExceptionDetails(exception, User.IsInRole("Administrator"));

            return StatusCode((int)httpStatusCode, response);
        }
    }
}

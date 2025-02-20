using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrsApi.Classes.ApiResponses;
using SrsApi.Classes.SrsItemLevelController;
using SrsApi.DbContext;
using SrsApi.Enums;
using SrsApi.Interfaces;

namespace SrsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FuzzyMatchingController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseServiceWithIncludes<SrsAnswer> _srsAnswerService;
        private readonly IFuzzyMatchingService _fuzzyMatchingService;
        private readonly IConfiguration _appsettings;

        public FuzzyMatchingController(ApplicationDbContext context, IBaseServiceWithIncludes<SrsAnswer> srsAnswerService, IFuzzyMatchingService fuzzyMatchingService, IConfiguration appsettings)
        {
            _context = context;
            _srsAnswerService = srsAnswerService;
            _fuzzyMatchingService = fuzzyMatchingService;
            _appsettings = appsettings;
        }

        // GET: api/FuzzyMatching
        [HttpGet]
        public async Task<ActionResult<SrsApiResponse>> GetFuzzyMatchRatio(string input, string compare, FuzzySearchMethod fuzzySearchMethod)
        {
            var result = _fuzzyMatchingService.GetFuzzyMatchRatio(input, compare, fuzzySearchMethod);

            return SuccessResponse(new { Ratio = result });                     
        }        
    }
}

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
using SrsApi.Classes.SrsItemController;
using SrsApi.Classes.SrsItemLevelController;
using SrsApi.DbContext;
using SrsApi.Enums;
using SrsApi.Interfaces;

namespace SrsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class SrsAnswerSearchMethodController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseServiceWithIncludes<SrsAnswer> _srsAnswerService;
        private readonly IBaseServiceWithIncludes<SrsAnswerFuzzySearchMethod> _srsAnswerFuzzySearchMethodService;
        private readonly IFuzzySearchMethodService _fuzzySearchMethodService;
        private readonly IConfiguration _appsettings;

        public SrsAnswerSearchMethodController(ApplicationDbContext context, 
            IBaseServiceWithIncludes<SrsAnswer> srsAnswerService,
            IBaseServiceWithIncludes<SrsAnswerFuzzySearchMethod> srsAnswerFuzzySearchMethodService,
            IFuzzySearchMethodService fuzzySearchMethodService,
            IConfiguration appsettings)
        {
            _context = context;
            _srsAnswerService = srsAnswerService;
            _srsAnswerFuzzySearchMethodService = srsAnswerFuzzySearchMethodService;
            _fuzzySearchMethodService = fuzzySearchMethodService;
            _appsettings = appsettings;
        }

        // GET: api/SrsAnswerSearchMethod
        //[HttpGet]
        //public async Task<ActionResult<SrsApiResponse>> GetSrsAnswerSearchMethods(int skip = 0, int take = 0, bool includeDeleted = false)
        //{

        //}

        // GET: api/SrsAnswerSearchMethod/uid
        [HttpGet("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> GetSrsAnswerSearchMethod(Guid uid, bool includeDeleted = false)
        {
            try
            {
                var srsAnswerSearchMethod = await _srsAnswerFuzzySearchMethodService.GetByUID(uid, _srsAnswerFuzzySearchMethodService.GetIncludes("SearchMethod"), includeDeleted: includeDeleted);

                if (srsAnswerSearchMethod == null)
                {
                    return NotFoundResponse();
                }

                return SuccessResponse(srsAnswerSearchMethod);
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }
        }

        // PUT: api/SrsAnswerSearchMethod/uid
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> PutSrsAnswerSearchMethod(Guid uid, SrsAnswerSearchMethodPutModel srsAnswerSearchMethodPutModel)
        {
            SrsAnswerFuzzySearchMethod dbSrsSearchMethod = null;

            try
            {
                dbSrsSearchMethod = await _srsAnswerFuzzySearchMethodService.GetByUID(uid, _srsAnswerFuzzySearchMethodService.GetIncludes("SearchMethod"));
            } 
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            if (dbSrsSearchMethod == null)
            {
                return NotFoundResponse();
            }

            if (srsAnswerSearchMethodPutModel.FuzzySearchMethod != null && srsAnswerSearchMethodPutModel.FuzzySearchMethod != dbSrsSearchMethod.SearchMethod.FuzzySearchMethod) 
            {
                dbSrsSearchMethod.SearchMethod.FuzzySearchMethod = (FuzzySearchMethod)srsAnswerSearchMethodPutModel.FuzzySearchMethod;
            }

            if (srsAnswerSearchMethodPutModel.MinumumAcceptedValue != null && srsAnswerSearchMethodPutModel.MinumumAcceptedValue != dbSrsSearchMethod.MinumumAcceptedValue)
            {
                dbSrsSearchMethod.MinumumAcceptedValue = (int)srsAnswerSearchMethodPutModel.MinumumAcceptedValue;
            }

            try
            {
                await _srsAnswerFuzzySearchMethodService.Update(dbSrsSearchMethod);

                _srsAnswerFuzzySearchMethodService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(dbSrsSearchMethod);
        }

        // POST: api/SrsAnswerSearchMethod
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SrsApiResponse>> PostSrsAnswerSearchMethod(SrsAnswerSearchMethodPostModel srsAnswerSearchMethodPostModel)
        {
            var dbSrsAnswer = await _srsAnswerService.GetByUID(srsAnswerSearchMethodPostModel.SrsAnswerUID, _srsAnswerService.GetIncludes("SearchMethods"));

            if(dbSrsAnswer == null)
            {
                return NotFoundResponse();
            }

            var newSearchMethod = new SrsAnswerFuzzySearchMethod
            {
                MinumumAcceptedValue = (int)srsAnswerSearchMethodPostModel.MinumumAcceptedValue,
                SearchMethod = await _fuzzySearchMethodService.Get((FuzzySearchMethod)srsAnswerSearchMethodPostModel.FuzzySearchMethod)
            };

            dbSrsAnswer.SearchMethods.Add(newSearchMethod);

            try
            {
                await _srsAnswerService.Update(dbSrsAnswer);

                _srsAnswerService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(newSearchMethod);
        }

        // DELETE: api/SrsAnswer/uid
        [HttpDelete("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> DeleteSrsAnswer(Guid uid)
        {
            try
            {
                var srsAnswerSearchMethod = await _srsAnswerFuzzySearchMethodService.GetByUID(uid);

                if (srsAnswerSearchMethod == null)
                {
                    return NotFoundResponse();
                }

                _srsAnswerFuzzySearchMethodService.Delete(uid);

                _srsAnswerService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SrsApi.Classes.ApiResponses;
using SrsApi.Classes.SrsAnswerSearchMethodController;
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

            if(srsAnswerSearchMethodPutModel.SearchMethods == null || srsAnswerSearchMethodPutModel.SearchMethods.Count == 0)
            {
                return ErrorResponse("At least one SearchMethod must be provided.");
            }

            foreach (var searchMethod in srsAnswerSearchMethodPutModel.SearchMethods)
            {
                if (searchMethod.FuzzySearchMethod != null && searchMethod.FuzzySearchMethod != dbSrsSearchMethod.SearchMethod.FuzzySearchMethod)
                {
                    dbSrsSearchMethod.SearchMethod.FuzzySearchMethod = (FuzzySearchMethod)searchMethod.FuzzySearchMethod;
                }

                if (searchMethod.MinimumAcceptedValue != null && searchMethod.MinimumAcceptedValue != dbSrsSearchMethod.MinimumAcceptedValue)
                {
                    dbSrsSearchMethod.MinimumAcceptedValue = (int)searchMethod.MinimumAcceptedValue;
                }
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

            if(dbSrsAnswer.SearchMethods == null)
            {
                dbSrsAnswer.SearchMethods = new List<SrsAnswerFuzzySearchMethod>();
            }

            foreach (var searchMethod in srsAnswerSearchMethodPostModel.SearchMethods)
            {
                if(searchMethod.MinimumAcceptedValue != null && searchMethod.FuzzySearchMethod != null)
                {
                    var newSearchMethod = new SrsAnswerFuzzySearchMethod
                    {
                        MinimumAcceptedValue = (int)searchMethod.MinimumAcceptedValue,
                        SearchMethod = await _fuzzySearchMethodService.Get((FuzzySearchMethod)searchMethod.FuzzySearchMethod)
                    };

                    dbSrsAnswer.SearchMethods.Add(newSearchMethod);
                }
            }    
            
            if(dbSrsAnswer.SearchMethods.Count == 0)
            {
                return ErrorResponse("No valid SearchMethods were included.");
            }

            try
            {
                await _srsAnswerService.Update(dbSrsAnswer);

                _srsAnswerService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(dbSrsAnswer.SearchMethods);
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

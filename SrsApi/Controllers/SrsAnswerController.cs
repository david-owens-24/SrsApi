using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SrsApi.Classes.ApiResponses;
using SrsApi.Classes.SrsAnswerController;
using SrsApi.DbContext;
using SrsApi.Enums;
using SrsApi.Interfaces;

namespace SrsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class SrsAnswerController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseServiceWithIncludes<SrsAnswer> _srsAnswerService;
        private readonly IBaseServiceWithIncludes<SrsItem> _srsItemService;
        private readonly IFuzzySearchMethodService _fuzzySearchMethodService;
        private readonly IConfiguration _appsettings;

        public SrsAnswerController(ApplicationDbContext context, 
            IBaseServiceWithIncludes<SrsAnswer> srsAnswerService, 
            IBaseServiceWithIncludes<SrsItem> srsItemService,
            IFuzzySearchMethodService fuzzySearchMethodService,
            IConfiguration appsettings)
        {
            _context = context;
            _srsAnswerService = srsAnswerService;
            _srsItemService = srsItemService;
            _fuzzySearchMethodService = fuzzySearchMethodService;
            _appsettings = appsettings;
        }

        // GET: api/SrsAnswer
        [HttpGet]
        public async Task<ActionResult<SrsApiResponse>> GetSrsAnswers(int skip = 0, int take = 0, string includes = null, bool includeDeleted = false)
        {
            if(take == 0)
            {
                //set to default paging value from appsettings
                take = Convert.ToInt32(_appsettings["DefaultPagingLimit"]);
            }

            try
            {
                var results = await _srsAnswerService.GetAll(includeDeleted: includeDeleted, includes: _srsAnswerService.GetIncludes(includes), skip: skip, take: take);

                return SuccessResponse(results.ToList());
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }            
        }

        // GET: api/SrsAnswer/uid
        [HttpGet("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> GetSrsAnswer(Guid uid, string includes = null, bool includeDeleted = false)
        {
            try
            {
                var srsAnswer = await _srsAnswerService.GetByUID(uid, includes: _srsAnswerService.GetIncludes(includes), includeDeleted: includeDeleted);

                if (srsAnswer == null)
                {
                    return NotFoundResponse();
                }

                return SuccessResponse(srsAnswer);
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }
        }

        // PUT: api/SrsAnswer/uid
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> PutSrsAnswer(Guid uid, SrsAnswerPutModel srsAnswerPutModel)
        {
            SrsAnswer dbSrsAnswer = null;

            try
            {
                dbSrsAnswer = await _srsAnswerService.GetByUID(uid, _srsAnswerService.GetIncludes("SearchMethods"));
            } 
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            if (dbSrsAnswer == null)
            {
                return NotFoundResponse();
            }

            if (!string.IsNullOrWhiteSpace(srsAnswerPutModel.AnswerText) && srsAnswerPutModel.AnswerText != dbSrsAnswer.AnswerText) 
            {
                dbSrsAnswer.AnswerText = srsAnswerPutModel.AnswerText;
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

            return SuccessResponse(dbSrsAnswer);
        }

        // POST: api/SrsAnswer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SrsApiResponse>> PostSrsAnswer(SrsAnswerPostModel srsAnswerPostModel)
        {
            var dbSrsItem = await _srsItemService.GetByUID(srsAnswerPostModel.SrsItemUID, _srsItemService.GetIncludes("Answers"));

            if(dbSrsItem == null)
            {
                return NotFoundResponse();
            }

            //could check if there's already an answer with the same AnswerText here, but is not unique at database level so is not incorrect to have dupes 
            SrsAnswer srsAnswer = new SrsAnswer
            {
                AnswerText = srsAnswerPostModel.AnswerText
            };

            if (srsAnswerPostModel.HasSrsItemSearchMethod())
            {
                srsAnswer.SearchMethods = new List<SrsAnswerFuzzySearchMethod>();

                srsAnswer.SearchMethods.Add(new SrsAnswerFuzzySearchMethod
                {
                    MinimumAcceptedValue = (int)srsAnswerPostModel.MinimumAcceptedValue,
                    SearchMethod = await _fuzzySearchMethodService.Get((FuzzySearchMethod)srsAnswerPostModel.FuzzySearchMethod)
                });
            }

            dbSrsItem.Answers.Add(srsAnswer);

            try
            {
                await _srsItemService.Update(dbSrsItem);

                _srsItemService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(srsAnswer);
        }

        // DELETE: api/SrsAnswer/uid
        [HttpDelete("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> DeleteSrsAnswer(Guid uid)
        {
            try
            {
                var srsAnswer = await _srsAnswerService.GetByUID(uid, _srsAnswerService.GetIncludes("SearchMethods"));

                if (srsAnswer == null)
                {
                    return NotFoundResponse();
                }

                if(srsAnswer.SearchMethods != null)
                {
                    foreach (var searchMethod in srsAnswer.SearchMethods)
                    {
                        searchMethod.Deleted = DateTime.Now;
                    }
                }

                _srsAnswerService.Delete(srsAnswer.UID);

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

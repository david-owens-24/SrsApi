using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SrsApi.Classes.ApiResponses;
using SrsApi.Classes.SrsItemController;
using SrsApi.DbContext;
using SrsApi.Interfaces;

namespace SrsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class SrsItemController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseServiceWithIncludes<SrsItem> _srsItemService;
        private readonly IBaseService<SrsItemLevel> _srsItemLevelService;
        private readonly IConfiguration _appsettings;

        public SrsItemController(ApplicationDbContext context, IBaseServiceWithIncludes<SrsItem> srsItemService,  IConfiguration appsettings, IBaseService<SrsItemLevel> srsItemLevelService)
        {
            _context = context;
            _srsItemService = srsItemService;
            _appsettings = appsettings;
            _srsItemLevelService = srsItemLevelService;
        }

        // GET: api/SrsItem
        [HttpGet]
        public async Task<ActionResult<SrsApiResponse>> GetSrsItems(int skip = 0, int take = 0, Guid? srsItemLevelUID = null, string includes = null, bool includeDeleted = false)
        {
            if(take == 0)
            {
                //set to default paging value from appsettings
                take = Convert.ToInt32(_appsettings["DefaultPagingLimit"]);
            }

            try
            {
                IEnumerable<SrsItem> results = null;

                if (srsItemLevelUID != null)
                {
                    results = await _srsItemService.GetAll(filter: x=>x.Level.UID == srsItemLevelUID, _srsItemService.GetIncludes(includes), includeDeleted: includeDeleted, skip: skip, take: take);
                } 
                else
                {
                    results = await _srsItemService.GetAll(includes: _srsItemService.GetIncludes(includes), includeDeleted: includeDeleted, skip: skip, take: take);
                }

                return SuccessResponse(results.ToList());
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }            
        }

        // GET: api/SrsItem/uid
        [HttpGet("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> GetSrsItemByUid(Guid uid, string includes = null, bool includeDeleted = false)
        {
            try
            {
                var srsItemLevel = await _srsItemService.GetByUID(uid, includes:_srsItemService.GetIncludes(includes), includeDeleted: includeDeleted);

                if (srsItemLevel == null)
                {
                    return NotFoundResponse();
                }

                return SuccessResponse(srsItemLevel);
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }
        }

        // PUT: api/SrsItem/uid
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> PutSrsItem(Guid uid, SrsItemPutModel srsItemPutModel)
        {
            SrsItem dbSrsItem = null;

            try
            {
                dbSrsItem = await _srsItemService.GetByUID(uid, includes: [x=>x.Level]);
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            if (srsItemPutModel == null)
            {
                return NotFoundResponse();
            }

            if (dbSrsItem.Order != srsItemPutModel.Order)
            {
                dbSrsItem.Order = srsItemPutModel.Order;
            }

            if (dbSrsItem.Level.UID != srsItemPutModel.SrsItemLevelUID)
            {
                var newLevel = await _srsItemLevelService.GetByUID(srsItemPutModel.SrsItemLevelUID);

                if (newLevel == null)
                {
                    return ErrorResponse("An SrsItemLevel with the UID " + srsItemPutModel.SrsItemLevelUID + " was not found.", System.Net.HttpStatusCode.NotFound);
                }

                dbSrsItem.Level = newLevel;
            }

            try
            {
                await _srsItemService.Update(dbSrsItem);

                _srsItemService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(dbSrsItem);
        }

        // POST: api/SrsItem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SrsApiResponse>> PostSrsItem(SrsItemPostModel srsItemPostModel)
        {
            //check that the provided SrsItemLevel exists
            SrsItemLevel itemLevel = null;

            try
            {
                itemLevel = await _srsItemLevelService.GetByUID(srsItemPostModel.SrsItemLevelUID);

                if (itemLevel == null)
                {
                    return ErrorResponse("An SrsItemLevel with the UID " + srsItemPostModel.SrsItemLevelUID + " was not found.", System.Net.HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            SrsItem srsItem = new SrsItem
            {
                //TODO: add answers here maybe? should really use the SrsItemAnswerController though when that's made
                Answers = new List<SrsAnswer>(),
                Level = itemLevel,
                Order = srsItemPostModel.Order,
            };

            try
            {
                await _srsItemService.Add(srsItem);

                _srsItemService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(srsItem);
        }

        // DELETE: api/SrsItem/uid
        [HttpDelete("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> DeleteSrsItem(Guid uid)
        {
            SrsItem srsItem = null;

            try
            {
                srsItem = await _srsItemService.GetByUID(uid, includes: _srsItemService.GetIncludes(["Details", "Answers"]));

                if (srsItem == null)
                {
                    return NotFoundResponse();
                }
            } 
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }            

            try
            {
                //cascade the delete for the Details and the Answers
                DateTime now = DateTime.Now;                

                if(srsItem.Details != null)
                {
                    srsItem.Details.Deleted = now;
                }

                if(srsItem.Answers != null)
                {
                    foreach (var answer in srsItem.Answers)
                    {
                        answer.Deleted = now;
                    }
                }

                _srsItemService.Delete(uid, now);

                await _srsItemService.Update(srsItem);

                _srsItemService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse();
        }
    }
}

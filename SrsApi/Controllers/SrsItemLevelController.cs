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
using SrsApi.Interfaces;

namespace SrsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class SrsItemLevelController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseService<SrsItemLevel> _srsItemLevelService;
        private readonly IBaseServiceWithIncludes<SrsItem> _srsItemService;
        private readonly IConfiguration _appsettings;

        public SrsItemLevelController(ApplicationDbContext context, IBaseService<SrsItemLevel> srsItemLevelService, IBaseServiceWithIncludes<SrsItem> srsItemService, IConfiguration appsettings)
        {
            _context = context;
            _srsItemLevelService = srsItemLevelService;
            _srsItemService = srsItemService;
            _appsettings = appsettings;
        }

        // GET: api/SrsItemLevel
        [HttpGet]
        public async Task<ActionResult<SrsApiResponse>> GetSrsItemLevels(int skip = 0, int take = 0, bool includeDeleted = false)
        {
            if(take == 0)
            {
                //set to default paging value from appsettings
                take = Convert.ToInt32(_appsettings["DefaultPagingLimit"]);
            }

            try
            {
                var results = await _srsItemLevelService.GetAll(includeDeleted: includeDeleted, skip: skip, take: take);

                return SuccessResponse(results.ToList());
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }            
        }

        // GET: api/SrsItemLevel/uid
        [HttpGet("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> GetSrsItemLevel(Guid uid, bool includeDeleted = false)
        {
            try
            {
                var srsItemLevel = await _srsItemLevelService.GetByUID(uid, includeDeleted: includeDeleted);

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

        // PUT: api/SrsItemLevel/uid
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> PutSrsItemLevel(Guid uid, SrsItemLevelPutModel srsItemLevelPutModel)
        {
            //for this endpoint, only let the user change the Name of the SrsItemLevel
            //
            SrsItemLevel dbSrsItemLevel = null;

            try
            {
                dbSrsItemLevel = await _srsItemLevelService.GetByUID(uid);
            } 
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            if (dbSrsItemLevel == null)
            {
                return NotFoundResponse();
            }

            if (dbSrsItemLevel.Name != srsItemLevelPutModel.Name) 
            {
                dbSrsItemLevel.Name = srsItemLevelPutModel.Name;
            }

            try
            {
                await _srsItemLevelService.Update(dbSrsItemLevel);

                _srsItemLevelService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(dbSrsItemLevel);
        }

        // POST: api/SrsItemLevel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SrsApiResponse>> PostSrsItemLevel(SrsItemLevelPostModel srsItemLevelPostModel)
        {
            //check to see if there already exists an SrsItemLevel with the POSTed level value
            try
            {
                if ((await _srsItemLevelService.GetAll(x => x.Level == srsItemLevelPostModel.Level)).Any())
                {
                    return ErrorResponse("An SrsItemLevel with the level " + srsItemLevelPostModel.Level + " already exists.", System.Net.HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            SrsItemLevel srsItemLevel = new SrsItemLevel
            {
                Level = srsItemLevelPostModel.Level,
                Name = srsItemLevelPostModel.Name
            };

            try
            {
                await _srsItemLevelService.Add(srsItemLevel);

                _srsItemLevelService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(srsItemLevel);
        }

        // DELETE: api/SrsItemLevel/uid
        [HttpDelete("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> DeleteSrsItemLevel(Guid uid)
        {
            try
            {
                var srsItemLevel = await _srsItemLevelService.GetByUID(uid);

                if (srsItemLevel == null)
                {
                    return NotFoundResponse();
                }

                if((await _srsItemService.GetAll(x=>x.Level.UID == uid, take: 1)).Any())
                {
                    return ErrorResponse("SrsItemLevel has existing SrsItems, these must be deleted before the SrsItemLevel can be deleted.", System.Net.HttpStatusCode.BadRequest);
                }
            } 
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            try
            {
                _srsItemLevelService.Delete(uid);

                _srsItemLevelService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse();
        }
    }
}

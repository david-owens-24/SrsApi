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
    public class SrsItemDetailsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseService<SrsItemDetails> _srsItemDetails;
        private readonly IBaseServiceWithIncludes<SrsItem> _srsItemService;
        private readonly IFuzzySearchMethodService _fuzzySearchMethodService;
        private readonly IConfiguration _appsettings;

        public SrsItemDetailsController(ApplicationDbContext context,
            IBaseService<SrsItemDetails> srsItemDetails, 
            IBaseServiceWithIncludes<SrsItem> srsItemService,
            IFuzzySearchMethodService fuzzySearchMethodService,
            IConfiguration appsettings)
        {
            _context = context;
            _srsItemDetails = srsItemDetails;
            _srsItemService = srsItemService;
            _fuzzySearchMethodService = fuzzySearchMethodService;
            _appsettings = appsettings;
        }

        //TODO: decide if we want this endpoint, since generally SrsItemDetails will be searched for by Guid all the time
        // GET: api/SrsItemDetails
        //[HttpGet]
        //public async Task<ActionResult<SrsApiResponse>> GetSrsItemDetails(int skip = 0, int take = 0, bool includeDeleted = false)
        //{
        //    if(take == 0)
        //    {
        //        //set to default paging value from appsettings
        //        take = Convert.ToInt32(_appsettings["DefaultPagingLimit"]);
        //    }

        //    try
        //    {
        //        var results = await _srsItemDetails.GetAll(includeDeleted: includeDeleted, skip: skip, take: take);

        //        return SuccessResponse(results.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponseFromException(ex);
        //    }            
        //}

        // GET: api/SrsItemDetails/uid
        [HttpGet("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> GetSrsItemDetails(Guid uid, bool includeDeleted = false)
        {
            try
            {
                var srsItemDetails = await _srsItemDetails.GetByUID(uid, includeDeleted: includeDeleted);

                if (srsItemDetails == null)
                {
                    return NotFoundResponse();
                }

                return SuccessResponse(srsItemDetails);
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }
        }

        // PUT: api/SrsItemDetails/uid
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> PutSrsItemDetails(Guid uid, SrsItemDetailsPutModel srsItemDetailsPutModel)
        {
            SrsItemDetails dbSrsItemDetails = null;

            try
            {
                dbSrsItemDetails = await _srsItemDetails.GetByUID(uid);
            } 
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            if (dbSrsItemDetails == null)
            {
                return NotFoundResponse();
            }

            if (!string.IsNullOrWhiteSpace(srsItemDetailsPutModel.QuestionText) && srsItemDetailsPutModel.QuestionText != dbSrsItemDetails.Question) 
            {
                dbSrsItemDetails.Question = srsItemDetailsPutModel.QuestionText;
            }

            try
            {
                await _srsItemDetails.Update(dbSrsItemDetails);

                _srsItemDetails.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(dbSrsItemDetails);
        }

        // POST: api/SrsItemDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<SrsApiResponse>> PostSrsItemDetails(SrsItemDetailsPostModel srsItemDetailsPostModel)
        {
            var dbSrsItem = await _srsItemService.GetByUID(srsItemDetailsPostModel.SrsItemUID, _srsItemService.GetIncludes("Details"));

            if (dbSrsItem == null)
            {
                return NotFoundResponse();
            }

            if(dbSrsItem.Details != null)
            {
                return ErrorResponse("SrsItem already has details, please use the PUT endpoint.");
            }

            SrsItemDetails srsItemDetails = new SrsItemDetails
            {
                Question = srsItemDetailsPostModel.QuestionText ?? ""
            };

            dbSrsItem.Details = srsItemDetails;

            try
            {
                await _srsItemService.Update(dbSrsItem);

                _srsItemService.SaveChanges();
            }
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            return SuccessResponse(srsItemDetails);
        }

        // DELETE: api/SrsItemDetails/uid
        [HttpDelete("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> DeleteSrsItemDetails(Guid uid)
        {  
            //TODO: decide if we ever want to allow details to be deleted (this should only really happen when the SrsItem itself is deleted)
            throw new NotImplementedException();
        }
    }
}

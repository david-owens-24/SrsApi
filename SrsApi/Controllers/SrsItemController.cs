﻿using System;
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
using SrsApi.Services;

namespace SrsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class SrsItemController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ISrsItemService _srsItemService;
        private readonly IConfiguration _appsettings;

        public SrsItemController(ApplicationDbContext context, ISrsItemService srsItemService, IConfiguration appsettings)
        {
            _context = context;
            _srsItemService = srsItemService;
            _appsettings = appsettings;
        }

        // GET: api/SrsItem
        [HttpGet]
        public async Task<ActionResult<SrsApiResponse>> GetSrsItems(int skip = 0, int take = 0, Guid? srsItemLevel = null, string includes = null, bool includeDeleted = false)
        {
            if(take == 0)
            {
                //set to default paging value from appsettings
                take = Convert.ToInt32(_appsettings["DefaultPagingLimit"]);
            }

            try
            {
                IEnumerable<SrsItem> results = null;

                if (srsItemLevel != null)
                {
                    results = await _srsItemService.GetAll(filter: x=>x.Level.UID == srsItemLevel, _srsItemService.GetIncludes(includes), includeDeleted: includeDeleted, skip: skip, take: take);
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
        public async Task<ActionResult<SrsApiResponse>> GetSrsItemByUid(Guid uid, bool includeDeleted = false)
        {
            try
            {
                var srsItemLevel = await _srsItemService.GetByUID(uid, includeDeleted: includeDeleted);

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
        public async Task<ActionResult<SrsApiResponse>> PutSrsItem(Guid uid, SrsItemLevelPutModel srsItemLevelPutModel)
        {
            
            SrsItem dbSrsItem = null;

            try
            {
                dbSrsItem = await _srsItemService.GetByUID(uid);
            } 
            catch (Exception ex)
            {
                return ErrorResponseFromException(ex);
            }

            if (srsItemLevelPutModel == null)
            {
                return NotFoundResponse();
            }

            //TODO: add updates here
            //if (dbSrsItemLevel.Name != srsItemLevelPutModel.Name) 
            //{
            //    dbSrsItemLevel.Name = srsItemLevelPutModel.Name;
            //}

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

        //// POST: api/SrsItem
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<SrsApiResponse>> PostSrsItemLevel(SrsItemLevelPostModel srsItemLevelPostModel)
        //{
        //    //check to see if there already exists an SrsItemLevel with the POSTed level value
        //    try
        //    {
        //        if ((await _srsItemService.GetAll(x => x.Level == srsItemLevelPostModel.Level)).Any())
        //        {
        //            return ErrorResponse("An SrsItemLevel with the level " + srsItemLevelPostModel.Level + " already exists.", System.Net.HttpStatusCode.BadRequest);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponseFromException(ex);
        //    }

        //    SrsItemLevel srsItemLevel = new SrsItemLevel
        //    {
        //        Level = srsItemLevelPostModel.Level,
        //        Name = srsItemLevelPostModel.Name
        //    };

        //    try
        //    {
        //        await _srsItemLevelService.Add(srsItemLevel);

        //        _srsItemLevelService.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponseFromException(ex);
        //    }

        //    return SuccessResponse(srsItemLevel);
        //}

        // DELETE: api/SrsItem/uid
        [HttpDelete("{uid}")]
        public async Task<ActionResult<SrsApiResponse>> DeleteSrsItem(Guid uid)
        {
            try
            {
                var srsItemLevel = await _srsItemService.GetByUID(uid);

                if (srsItemLevel == null)
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
                _srsItemService.Delete(uid);

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

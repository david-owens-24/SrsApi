using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrsApi.Classes.SrsItemLevelController;
using SrsApi.DbContext;
using SrsApi.Interfaces;

namespace SrsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class SrsItemLevelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseService<SrsItemLevel> _srsItemLevelService;

        public SrsItemLevelController(ApplicationDbContext context, IBaseService<SrsItemLevel> srsItemLevelService)
        {
            _context = context;
            _srsItemLevelService = srsItemLevelService;
        }

        // GET: api/SrsItemLevel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SrsItemLevel>>> GetSrsItemLevels(bool includeDeleted = false)
        {
            return (await _srsItemLevelService.GetAll(includeDeleted: includeDeleted)).ToList();
        }

        // GET: api/SrsItemLevel/uid
        [HttpGet("{uid}")]
        public async Task<ActionResult<SrsItemLevel>> GetSrsItemLevel(Guid uid, bool includeDeleted = false)
        {
            var srsItemLevel = await _srsItemLevelService.GetByUID(uid, includeDeleted: includeDeleted);

            if (srsItemLevel == null)
            {
                return NotFound();
            }

            return srsItemLevel;
        }

        // PUT: api/SrsItemLevel/uid
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{uid}")]
        public async Task<IActionResult> PutSrsItemLevel(Guid uid, SrsItemLevelPutModel srsItemLevelPutModel)
        {
            //for this endpoint, only let the user change the Name of the SrsItemLevel            

            var dbSrsItemLevel = await _srsItemLevelService.GetByUID(uid);

            if (srsItemLevelPutModel == null)
            {
                return NotFound();
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
                //TODO: error handling here
                return StatusCode(500);
            }

            return Ok();
        }

        // POST: api/SrsItemLevel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SrsItemLevel>> PostSrsItemLevel(SrsItemLevelPostModel srsItemLevelPostModel)
        {
            //check to see if there already exists an SrsItemLevel with the POSTed level value

            if((await _srsItemLevelService.GetAll(x=>x.Level == srsItemLevelPostModel.Level)).Any())
            {
                return BadRequest("An SrsItemLevel with the level " + srsItemLevelPostModel.Level + " already exists.");
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
                //TODO: error handling/logging here
                return StatusCode(500);
            }   

            return CreatedAtAction("GetSrsItemLevel", new { uid = srsItemLevel.UID }, srsItemLevel);
        }

        // DELETE: api/SrsItemLevel/uid
        [HttpDelete("{uid}")]
        public async Task<IActionResult> DeleteSrsItemLevel(Guid uid)
        {
            var srsItemLevel = await _srsItemLevelService.GetByUID(uid);

            if (srsItemLevel == null)
            {
                return NotFound();
            }

            //TODO: if there are any SrsItems with this level, then don't allow the delete (need to create the SrsItemService first)

            try
            {
                _srsItemLevelService.Delete(uid);

                _srsItemLevelService.SaveChanges();
            }
            catch (Exception ex)
            {
                //TODO: error handling/logging here
                return StatusCode(500);
            }

            return Ok();
        }
    }
}

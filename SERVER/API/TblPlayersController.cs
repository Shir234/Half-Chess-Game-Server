﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVER.Data;
using SERVER.Models;

namespace SERVER.API
{
    [Route("api/[controller]")]                                             // The route to activate the controller -> replace the [] with TblPlayers
    [ApiController]                                                         // The controller name
    public class TblPlayersController : ControllerBase
    {
        private readonly SERVERContext _context;

        public TblPlayersController(SERVERContext context)
        {
            _context = context;
        }

        // GET: api/TblPlayers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblPlayers>>> GetTblPlayers()
        {
            return await _context.TblPlayers.ToListAsync();
        }

        // GET: api/TblPlayers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblPlayers>> GetTblPlayers(int id)
        {
            var tblPlayers = await _context.TblPlayers.FindAsync(id);

            if (tblPlayers == null)
            {
                return NotFound();
            }

            return tblPlayers;
        }

        // PUT: api/TblPlayers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblPlayers(int id, TblPlayers tblPlayers)
        {
            if (id != tblPlayers.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblPlayers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblPlayersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TblPlayers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblPlayers>> PostTblPlayers(TblPlayers tblPlayers)
        {
            _context.TblPlayers.Add(tblPlayers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblPlayers", new { id = tblPlayers.Id }, tblPlayers);
        }

        // DELETE: api/TblPlayers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblPlayers(int id)
        {
            var tblPlayers = await _context.TblPlayers.FindAsync(id);
            if (tblPlayers == null)
            {
                return NotFound();
            }

            _context.TblPlayers.Remove(tblPlayers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblPlayersExists(int id)
        {
            return _context.TblPlayers.Any(e => e.Id == id);
        }




        // GET: api/TblPlayers/Verify
        [HttpGet("Verify")]
        public async Task<bool> GetPlayerVerification(int id, string name)
        {
            var tblPlayers = await _context.TblPlayers.FindAsync(id);
 
            if (tblPlayers == null || !tblPlayers.Name.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
                //return NotFound("Player not found or name does not match.");      //404
            }
            return true;
            //return Ok(true);          //200
        }
    }
}

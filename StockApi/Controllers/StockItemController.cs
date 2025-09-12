using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi.Models;

namespace StockApi.Controllers
{
    [Route("stockapi/[controller]")]
    [ApiController]
    public class StockItemController : Controller
    {
        private readonly AppDbContext? _context;

        public StockItemController(AppDbContext? context)
        {
            _context = context;
        }

        // GET: api/StockItem/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<StockItem>>> GetAll()
        {
            if (_context != null)
            {
                return await _context.StockItems.ToListAsync();
            }
            else
            {
                return NotFound("No objects found");
            }

        }

        // POST: api/StockItem/AddStockItem
        [HttpPost]
        [Route("AddStockItem")]
        public async Task<ActionResult<bool>> AddStockItem()
        {
            StockItem itemToAdd = new StockItem
            {
                name = "metal boru",
                Id = 1,
            };

            try
            {
                _context?.StockItems.AddAsync(itemToAdd);
                return Ok(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding item: ", ex);
                return BadRequest(ex.Message);
            }


        }
    }
}

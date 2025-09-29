using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

        // GET: stockapi/StockItem/GetAll
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
                return BadRequest("No data found");
            }

        }

        // GET: stockapi/StockItem/Get/id
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<StockItem>> GetById(int id)
        {
            if (_context != null)
            {
                try
                {
                    var item = await _context.StockItems.FindAsync(id);
                    if (item == null)
                    {
                        return NotFound($"Item with ID {id} not found.");
                    }
                    else
                    {
                        return Ok(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving item: {ex}");
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return NotFound("No objects found");
            }

        }

        // POST: stockapi/StockItem/AddStockItem
        [HttpPost]
        [Route("AddStockItem")]
        public async Task<ActionResult<StockItem>> AddStockItem([FromBody] StockItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.name))
            {
                return BadRequest("Item name cannot be empty.");
            }

            try
            {
                int maxId = 0;
                if (_context == null)
                {
                    return BadRequest("Database context is not available.");
                }

                if (_context.StockItems.Any())
                {
                    maxId = _context.StockItems.Max(i => i.Id);
                }
                item.Id = maxId + 1;

                await _context.StockItems.AddAsync(item);
                await _context.SaveChangesAsync();

                return Ok($"Added new item with id: {item.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item: {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateStockItem")]
        public async Task<IActionResult> UpdateStockItem([FromBody] StockItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.name))
            {
                return BadRequest("Item name cannot be empty.");
            }
            if (_context == null)
            {
                return BadRequest("Database context is not available.");
            }
            try
            {
                var existingItem = await _context.StockItems.FindAsync(item.Id);
                if (existingItem == null)
                {
                    return NotFound($"Item with ID {item.Id} not found.");
                }

                // FILL ACCORDING TO PROPERTIES OF STOCK ITEMS
                existingItem.name = item.name;

                // END FILL

                _context.StockItems.Update(existingItem);
                await _context.SaveChangesAsync();
                return Ok($"Item with ID {item.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating item: {ex}");
                return BadRequest(ex.Message);
            }
        }

        // DELETE: stockapi/StockItem/DeleteStockItem/id
        [HttpDelete]
        [Route("DeleteStockItem")]
        public async Task<IActionResult> DeleteStockItem(int id)
        {
            if (_context == null)
            {
                return BadRequest("Database context is not available.");
            }
            try
            {
                var item = await _context.StockItems.FindAsync(id);
                if (item == null)
                {
                    return NotFound($"Item with ID {id} not found.");
                }
                _context.StockItems.Remove(item);
                await _context.SaveChangesAsync();
                return Ok($"Item with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex}");
                return BadRequest(ex.Message);
            }

        }
    }
}

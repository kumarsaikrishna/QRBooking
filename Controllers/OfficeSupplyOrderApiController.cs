using Microsoft.AspNetCore.Mvc;
using QRCodeBooking.Models.DB;
using QRCodeBooking.Models.Entitys;

namespace QRCodeBooking.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OfficeSupplyOrderApiController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public OfficeSupplyOrderApiController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("dashboard")]
		public IActionResult GetDashboardStats()
		{
			try
			{
				var allOrders = _context.OfficeSupplyOrderS.ToList();
				var unfulfilledOrders = allOrders.Where(o => !o.IsFulfilled).ToList();
				var fulfilledOrders = allOrders.Where(o => o.IsFulfilled).ToList();

				var stats = new
				{
					ActiveOrderCount = unfulfilledOrders.Count,
					CompletedOrderCount = fulfilledOrders.Count,
					AvgDeliveryTime = "18m", // Placeholder
					MostOrderedItem = GetMostOrderedItem(),
					AllOrders = allOrders
				};

				return Ok(stats);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Error loading dashboard", error = ex.Message });
			}
		}



		private string GetMostOrderedItem()
		{
			try
			{
				var mostOrdered = _context.OfficeSupplyOrderS
					.GroupBy(o => o.ItemName)
					.Select(group => new
					{
						ItemName = group.Key,
						Count = group.Count()
					})
					.OrderByDescending(g => g.Count)
					.FirstOrDefault();

				return mostOrdered?.ItemName ?? "N/A";
			}
			catch
			{
				return "N/A";
			}
		}

		[HttpPost("submit")]
		public async Task<IActionResult> SubmitOrder([FromBody] OfficeSupplyOrder order)
		{
			try
			{
				if (ModelState.IsValid)
				{
					order.RequestedOn = DateTime.Now;
					order.CreatedOn = DateTime.Now;
					order.IsFulfilled = false;

					_context.OfficeSupplyOrderS.Add(order);
					await _context.SaveChangesAsync();

					return Ok(new { message = "Order submitted successfully!" });
				}

				return BadRequest(ModelState);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Error submitting order", error = ex.Message });
			}
		}

		[HttpGet("unfulfilled-orders")]
		public IActionResult GetUnfulfilledOrders()
		{
			try
			{
				var orders = _context.OfficeSupplyOrderS
					.Where(o => !o.IsFulfilled)
					.OrderBy(o => o.CreatedOn)
					.ToList();

				return Ok(orders);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Error retrieving orders", error = ex.Message });
			}
		}

		[HttpPut("fulfill/{id}")]
		public IActionResult MarkOrderAsFulfilled(int id)
		{
			try
			{
				var order = _context.OfficeSupplyOrderS.FirstOrDefault(o => o.Id == id);
				if (order == null)
					return NotFound(new { message = "Order not found" });

				order.IsFulfilled = true;
				_context.SaveChanges();

				return Ok(new { message = "Order marked as fulfilled" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Error updating order", error = ex.Message });
			}
		}

		[HttpGet("{id}")]
		public IActionResult GetOrderDetails(int id)
		{
			try
			{
				var order = _context.OfficeSupplyOrderS.FirstOrDefault(o => o.Id == id);
				if (order == null)
					return NotFound(new { message = "Order not found" });

				return Ok(order);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Error retrieving order details", error = ex.Message });
			}
		}
	}

}

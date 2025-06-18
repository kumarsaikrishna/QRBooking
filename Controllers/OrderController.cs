using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCodeBooking.BAL_DAL;
using QRCodeBooking.Models.DB;
using QRCodeBooking.Models.DTOs;
using QRCodeBooking.Models.Entitys;

namespace QRCodeBooking.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _context;

        public OrderController(IOrderService orderService, ApplicationDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        public IActionResult CreateManualOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateManualOrder(ManualOrderDto model)
        {
            int userId = 1; // Replace with actual user ID from session/auth
            var result = await _orderService.PlaceManualOrder(userId, model);
            return RedirectToAction("VerifyOTP", new { orderId = result.OrderId });
        }

        public IActionResult VerifyOTP(int orderId)
        {
            return View(new VerifyOTPDto { OrderId = orderId });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOTP(VerifyOTPDto model)
        {
            var verified = await _orderService.VerifyOTP(model.OrderId, model.OTP);
            if (verified)
                return RedirectToAction("Success");

            ModelState.AddModelError("", "Invalid OTP");
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
         
        public IActionResult Dashboard()
        {
            try
            {
                // Get all unfulfilled and fulfilled orders
                var allOrders = _context.OfficeSupplyOrderS.ToList();
                var unfulfilledOrders = allOrders.Where(o => o.IsFulfilled == false).ToList();
                var fulfilledOrders = allOrders.Where(o => o.IsFulfilled == true).ToList();

                // ViewBag Stats
                ViewBag.ActiveOrderCount = unfulfilledOrders.Count;
                ViewBag.CompletedOrderCount = fulfilledOrders.Count;
                ViewBag.AvgDeliveryTime = "18m"; // Placeholder
                ViewBag.MostOrderedItem = GetMostOrderedItem();

                return View(allOrders);
            }
            catch (Exception ex)
            {
                // Optional: log error
                return StatusCode(500, "An error occurred while loading dashboard: " + ex.Message);
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

        public IActionResult Create(string service)
        {
            ViewBag.SelectedService = service;
            return View();
        }

        [HttpPost]
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
                // Log the exception here if needed (e.g., using ILogger)
                return StatusCode(500, new { message = "An error occurred while submitting the order.", error = ex.Message });
            }
        }



        public IActionResult OfficeBoyView()
        {
            try
            {
                var orders = _context.OfficeSupplyOrderS
                                     .Where(o => o.IsFulfilled == false)
                                     .OrderBy(o => o.CreatedOn)
                                     .ToList();
                return View(orders);
            }
            catch (Exception ex)
            {
                // Optional: log error
                return StatusCode(500, "An error occurred while retrieving orders: " + ex.Message);
            }
        }


        public IActionResult MarkOrderAsFulfilled(int id)
        {
            try
            {
                var order = _context.OfficeSupplyOrderS.FirstOrDefault(o => o.Id == id);
                if (order != null)
                {
                    order.IsFulfilled = true;
                    _context.SaveChanges();
                }
                return RedirectToAction("OfficeBoyView");
            }
            catch (Exception ex)
            {
                // Optional: log error
                return StatusCode(500, "An error occurred while updating the order: " + ex.Message);
            }
        }


        public IActionResult GetOrderDetails(int id)
        {
            try
            {
                var order = _context.OfficeSupplyOrderS.FirstOrDefault(o => o.Id == id);
                if (order == null)
                {
                    return Content("Order not found");
                }

                return PartialView("_OrderDetails", order);
            }
            catch (Exception ex)
            {
                // Optional: log error
                return StatusCode(500, "An error occurred while retrieving order details: " + ex.Message);
            }
        }




    }

}

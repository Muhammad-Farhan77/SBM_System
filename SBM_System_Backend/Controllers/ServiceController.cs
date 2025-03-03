using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SBM_System_Backend.AppDbContext;
using SBM_System_Backend.Entity;
using SBM_System_Backend.DTOs;
using System.Security.Claims;

namespace SBM_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly DataContext _datacontext;

        public ServiceController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        
       
        [HttpPost("CreateService")]
        public async Task<ActionResult<Service>> CreateService([FromBody] Service service)
        {
            if (service == null)
            {
                return BadRequest(new { message = "Invalid service data." });
            }

           
            service.IsApproved = false;

            _datacontext.Services.Add(service);
            await _datacontext.SaveChangesAsync();

            return Ok(new
            {
                message = "Service created successfully!",
                service
            });
        }
        [HttpPut("ApproveService")]
        public async Task<ActionResult> ApproveService(int id, [FromBody] bool isApproved)
        {
            var service = await _datacontext.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound(new { message = "Service not found." });
            }

            service.IsApproved = isApproved;
            await _datacontext.SaveChangesAsync();

            return Ok(new
            {
                message = isApproved ? "Service approved successfully!" : "Service rejected.",
                service
            });
        }

        [HttpPut("RejectService/{id}")]
        public async Task<ActionResult> RejectService(int id)
        {
            var service = await _datacontext.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound(new { message = "Service not found." });
            }

            service.IsApproved = false;
            await _datacontext.SaveChangesAsync();

            return Ok(new
            {
                message = "Service rejected successfully!",
                service
            });
        }


        [HttpGet("GetAllServices")]
        public async Task<ActionResult<IEnumerable<Service>>> GetAllServices()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);  // Get the user's role from the claims (JWT token)

            IEnumerable<Service> services;

            if (role == "Admin")
            {
                // Admin sees only services with Pending bookings
                services = await _datacontext.Services
                    .Where(s => _datacontext.Bookings.Any(b => b.ServiceId == s.Id && b.Status == BookingStatus.Pending))
                    .ToListAsync();
            }
            else
            {
                // Customer sees all services
                services = await _datacontext.Services.ToListAsync();
            }

            if (!services.Any())
            {
                return NotFound(new { message = "No services found." });
            }

            return Ok(services);
        }


        [HttpGet("GetServiceById")]
        public IActionResult GetServiceById(int id)
        {
            var service = _datacontext.Services.FirstOrDefault(s => s.Id == id);

            if (service == null)
            {
                return NotFound(new { message = "Service not found" });
            }

            return Ok(service);
        }

        [HttpDelete("DeleteService")]
        public async Task<IActionResult> DeleteService(int id)
        {
            // 🔹 Find the service by ID
            var service = await _datacontext.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound(new { message = "Service not found." });
            }

            // 🔹 Remove the service from the database
            _datacontext.Services.Remove(service);
            await _datacontext.SaveChangesAsync();

            return Ok(new { message = "Service deleted successfully." });
        }


        [HttpPut("UpdateService")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] Service updatedService)
        {
            var existingService = await _datacontext.Services.FindAsync(id);
            if (existingService == null)
            {
                return NotFound(new { message = "Service not found." });
            }

            // ✅ Update service properties
            existingService.Name = updatedService.Name;
            existingService.Description = updatedService.Description;
            existingService.Price = updatedService.Price;

            await _datacontext.SaveChangesAsync();

            return Ok(new { message = "Service updated successfully!" });
        }

        [HttpPost("BookService")]
        public async Task<IActionResult> BookService( int userId, int serviceId)
        {
            if (userId <= 0 || serviceId <= 0)
            {
                return BadRequest(new { message = "Invalid data." });
            }

            // Check if the service exists
            var service = await _datacontext.Services.FirstOrDefaultAsync(s => s.Id == serviceId);
            if (service == null)
            {
                return NotFound(new { message = "Service not found." });
            }

            // Check if the user exists
            var user = await _datacontext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Create a new booking
            var booking = new Booking
            {
                UserId = userId,
                ServiceId = serviceId,
                BookingDate = DateTime.UtcNow,
                Status = BookingStatus.Pending
            };

            // Add the booking to the database
             _datacontext.Bookings.Add(booking);
            await _datacontext.SaveChangesAsync();

            return Ok(new { message = "Service booked successfully!" });
        }





    }


}





    
 

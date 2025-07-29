﻿using AAUP_LabMaster.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AAUP_LabMaster.EntityManager
{
    public class SupervisourManager
    {
        private readonly ApplicationDbContext context;
        public BookingManager bookingManager { get; set; } 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupervisourManager(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,BookingManager bookingManager)

        {
            this.bookingManager = bookingManager;
            this.context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        




        public void updateBookingStatus(int id, Booking.BookStatus status)
            {
                var booking = context.Bookings
                    .Include(b => b.Client)
                    .Include(b => b.Equipment)
                        .ThenInclude(e => e.Lab)
                    .FirstOrDefault(b => b.Id == id);

                 

                booking.status = status;
                context.SaveChanges();

                string statusMessage = status switch
                {
                    Booking.BookStatus.Approved => "approved",
                    Booking.BookStatus.Rejected => "rejected",
                    _ => "updated"
                };

                var notification = new Notification
                {
                    UserId = booking.ClientId,Subject="Booking Status Update",
                    Body = $"Your booking for equipment '{booking.Equipment?.Name}' in lab '{booking.Equipment?.Lab?.Name}' on {booking.Date:MMM dd, yyyy} at {booking.Date:hh:mm tt} has been {statusMessage}.",
                    DateCreated = DateTime.Now
                };

                context.Notifications.Add(notification);
                context.SaveChanges();
            }


        public void updateBookingStatus12(int id, Booking.BookStatus status)
        {
            bookingManager.updateBookingStatus(id, status);
        }
        public List<Supervisour> GetAllSupervisours()
        {
            Console.WriteLine("Fetching all supervisors from the database.");
            foreach (var supervisour in context.Supervisours)
            {
                Console.WriteLine($"Supervisour ID: {supervisour.Id}, Name: {supervisour.FullName}");
            }
            return context.Supervisours.ToList();
        }
        public Supervisour? GetSupervisourById(int id)
        {
            return context.Supervisours.FirstOrDefault(s => s.Id == id);
        }

 
        public List<Booking> getAllBookingBySupervisourId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int supervisorId))
            {
                Console.WriteLine("Supervisor ID claim is missing or invalid.");
                return new List<Booking>();
            }

            return bookingManager.getBookingById(supervisorId);
        }
        public List<Lab> GetAllLabsbySupervisourId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int supervisorId))
            {
                Console.WriteLine("Supervisor ID claim is missing or invalid.");
                return new List<Lab>();
            }
            return context.Labs
                .Include(l => l.Equipment)
                .Where(l => l.SupervisorId == supervisorId&&l.Name!="Empty Lab")
                .ToList();
        }
        public void ApproveBooking(int bookingId)
        {
            var booking = context.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }
            booking.status = Booking.BookStatus.Approved;
            context.SaveChanges();
            var notification = new Notification
            {
                UserId = booking.ClientId,Subject = "Booking Approved",
                Body = $"Your booking for {booking.Equipment.Lab.Name} on {booking.Date.ToShortDateString()} at {booking.Date.TimeOfDay} has been approved.",
                DateCreated = DateTime.Now
            };
            context.Notifications.Add(notification);
            context.SaveChanges();
        }
        }
}

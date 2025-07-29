﻿using AAUP_LabMaster.EntityDTO;
using AAUP_LabMaster.EntityManager;
using AAUP_LabMaster.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static AAUP_LabMaster.Models.Equipment;
using Microsoft.AspNetCore.Hosting;

namespace AAUP_LabMaster.Controllers
{

    public class SupervisourController : Controller
    {
        private readonly SupervisourManager superManager;
        private readonly BookingManager bookingManager;
        private readonly EquipmentManager equipmentManager;
        private readonly LabManager labManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SupervisourController(SupervisourManager superManager, BookingManager bookingManager, EquipmentManager equipmentManager, LabManager labManager, IWebHostEnvironment webHostEnvironment)
        {
            this.superManager = superManager;
            this.bookingManager = bookingManager;
            this.equipmentManager = equipmentManager;
            this.labManager = labManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // public IActionResult getAllBookingsSupervisour()
        // {
        //     var labs = superManager.getAllBookingBySupervisourId();

        //     return View(labs);
        // }
        public IActionResult getAllBookingsSupervisour()
        {
            var bookings = superManager.getAllBookingBySupervisourId();
            return View(bookings);
        }


        public IActionResult GetAllLabsBySupervisourId()
        {
            var labs = superManager.GetAllLabsbySupervisourId();
            return View(labs);
        }
        public List<Lab> GetAllLabs()
        {
            return labManager.getAllLabs();
        }
        public IActionResult ViewEquipment(int id)
        {
            var lab = equipmentManager.GetEquipmentsByLabId(id);
            if (lab == null)
            {
                return NotFound();
            }
            return View(lab);
        }

        [HttpGet]
        public IActionResult AddNewEquipment(int? id)
        {
            ViewBag.Labs = labManager.getAllLabs();
            if (ViewBag.Labs == null) // Defensive check, though getAllLabs should return empty list, not null
            {
                ViewBag.Labs = new List<AAUP_LabMaster.Models.Lab>();
            }
            // --- END FIX ---

            var model = new EquipmentDTO();
            if (id.HasValue && id.Value > 0) // Ensure id is valid
            {
                // Verify lab exists before setting the LabId in the model
                var labExists = labManager.GetLabById(id.Value);
                if (labExists != null)
                {
                    model.LabId = id.Value; // Set the LabId for pre-selection
                }
                else
                {
                    TempData["ErrorMessage"] = $"The specified Lab (ID: {id.Value}) was not found. Please select a lab.";
                    // Do not set model.LabId if the ID is invalid, so the dropdown remains "-- Select Lab --"
                }
            }
            return View(model); // Pass the model to the view
        }

        //[HttpGet]
        //public IActionResult UpdateEquipment1(int? id)
        //{
        //    ViewBag.Labs = labManager.getAllLabs() ?? new List<Lab>();

        //    if (!id.HasValue || id <= 0)
        //    {
        //        TempData["ErrorMessage"] = "Invalid equipment ID.";
        //        return RedirectToAction("Index");
        //    }

        //    var equipment = equipmentManager.GetEquipmentById(id.Value);
        //    if (equipment == null)
        //    {
        //        TempData["ErrorMessage"] = $"Equipment with ID {id.Value} not found.";
        //        return RedirectToAction("Index");
        //    }

        //    return View(equipment);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken] 
        //public async Task<IActionResult> UpdateEquipment2(EquipmentDTO equipmentDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.Labs = labManager.getAllLabs();
        //        // Pass the DTO back to the view so form fields are repopulated with user's input
        //        return View(equipmentDto);
        //    }

        //    try
        //    {
        //        var existingEquipment = equipmentManager.GetEquipmentById(equipmentDto.Id);
        //        if (existingEquipment == null)
        //        {
        //            TempData["ErrorMessage"] = $"Equipment with ID {equipmentDto.Id} not found.";
        //            // Redirecting to list view if equipment is not found
        //            return RedirectToAction("GetEquipmentByLabId", new { id = equipmentDto.LabId });
        //        }

        //        // --- 1. Handle Image File Upload ---
        //        if (equipmentDto.ImageFile != null && equipmentDto.ImageFile.Length > 0)
        //        {
        //            // Delete old image file if it exists and a new one is uploaded
        //            if (!string.IsNullOrEmpty(existingEquipment.ImagePath))
        //            {
        //                // Construct the full physical path to the old image file
        //                // Use TrimStart('/') to remove leading slash before combining with WebRootPath
        //                var oldImageFullPath = Path.Combine(_webHostEnvironment.WebRootPath, existingEquipment.ImagePath.TrimStart('/'));
        //                if (System.IO.File.Exists(oldImageFullPath))
        //                {
        //                    System.IO.File.Delete(oldImageFullPath);
        //                }
        //            }

        //            // Define the upload folder path. Use Path.Combine correctly.
        //            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img"); // This will resolve to wwwroot/img
        //            if (!Directory.Exists(uploadsFolder))
        //            {
        //                Directory.CreateDirectory(uploadsFolder);
        //            }

        //            // Generate a unique file name to prevent overwriting issues
        //            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(equipmentDto.ImageFile.FileName);
        //            var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

        //            // Save the new file
        //            using (var fileStream = new FileStream(newFilePath, FileMode.Create))
        //            {
        //                await equipmentDto.ImageFile.CopyToAsync(fileStream);
        //            }

        //            existingEquipment.ImagePath = "/img/" + uniqueFileName; 
        //        }
              


        //        existingEquipment.Name = equipmentDto.Name;
        //        existingEquipment.Description = equipmentDto.Description;
        //        existingEquipment.Quantity = equipmentDto.Quantity;
        //        existingEquipment.Price = equipmentDto.Price;
        //        existingEquipment.status = equipmentDto.status;
        //        existingEquipment.LabId = equipmentDto.LabId;

          
        //        existingEquipment.Link = equipmentDto.linkUrl; 


        //        // --- 4. Persist Changes to the Database ---
        //        equipmentManager.UpdateEquipment(existingEquipment); // This should update the tracked entity

        //        TempData["Message"] = "Equipment updated successfully!";
        //        return RedirectToAction("GetEquipmentByLabId", new { id = existingEquipment.LabId });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the full exception for debugging, not just the message
        //        // _logger.LogError(ex, "Error updating equipment with ID {EquipmentId}", equipmentDto.Id);

        //        TempData["ErrorMessage"] = $"Error updating equipment: {ex.Message}";
        //        ViewBag.Labs = labManager.getAllLabs();
        //        // Pass the DTO back to the view in case of error
        //        return View(equipmentDto);
        //    }
        //}

        [HttpGet]
        public IActionResult UpdateEquipment(int? id)
        {
            ViewBag.Labs = labManager.getAllLabs() ?? new List<Lab>();

            if (!id.HasValue || id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid equipment ID.";
                return RedirectToAction("GetEquipmentByLabId");
            }

            var equipment = equipmentManager.GetEquipmentById(id.Value);
            if (equipment == null)
            {
                TempData["ErrorMessage"] = $"Equipment with ID {id.Value} not found.";
                return RedirectToAction("ViewAllEquipments");
            }

            // Return the Equipment model directly instead of converting to DTO
            return View(equipment);
        }


        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpdateEquipment(Equipment equipment, IFormFile ImageFile)
{
    if (!ModelState.IsValid)
    {
        ViewBag.Labs = labManager.getAllLabs();
        return View(equipment);
    }

    try
    {
        var existingEquipment = equipmentManager.GetEquipmentById(equipment.Id);
        if (existingEquipment == null)
        {
            TempData["ErrorMessage"] = $"Equipment with ID {equipment.Id} not found.";
            return RedirectToAction("ViewAllEquipments");
        }

        // ✅ Only replace image if a new one is provided
        if (ImageFile != null && ImageFile.Length > 0)
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(existingEquipment.ImagePath))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingEquipment.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Save new image
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
            Directory.CreateDirectory(uploadsFolder); // Safe even if exists

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }

            existingEquipment.ImagePath = "/img/" + uniqueFileName;
        }

        // ✅ Always update the other fields
        existingEquipment.Name = equipment.Name;
        existingEquipment.Description = equipment.Description;
        existingEquipment.Quantity = equipment.Quantity;
        existingEquipment.Price = equipment.Price;
        existingEquipment.status = equipment.status;
        existingEquipment.LabId = equipment.LabId;
        existingEquipment.Link = equipment.Link;

        // Save changes
        equipmentManager.UpdateEquipment(existingEquipment);

        TempData["Message"] = "Equipment updated successfully!";
        return RedirectToAction("ViewAllEquipments", new { id = existingEquipment.LabId });
    }
    catch (Exception ex)
    {
        TempData["ErrorMessage"] = $"Error updating equipment: {ex.Message}";
        ViewBag.Labs = labManager.getAllLabs();
        return View(equipment);
    }
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEquipment12(Equipment equipment, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Labs = labManager.getAllLabs();
                return View(equipment);
            }

            try
            {
                var existingEquipment = equipmentManager.GetEquipmentById(equipment.Id);
                if (existingEquipment == null)
                {
                    TempData["ErrorMessage"] = $"Equipment with ID {equipment.Id} not found.";
                    return RedirectToAction("ViewAllEquipments");
                }

                // Handle Image File Upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingEquipment.ImagePath))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                            existingEquipment.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save new image
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" +
                        Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    existingEquipment.ImagePath = "/img/" + uniqueFileName;
                }

                // Update other properties
                existingEquipment.Name = equipment.Name;
                existingEquipment.Description = equipment.Description;
                existingEquipment.Quantity = equipment.Quantity;
                existingEquipment.Price = equipment.Price;
                existingEquipment.status = equipment.status;
                existingEquipment.LabId = equipment.LabId;
                existingEquipment.Link = equipment.Link;

                // Save changes
                equipmentManager.UpdateEquipment(existingEquipment);

                TempData["Message"] = "Equipment updated successfully!";
                return RedirectToAction("ViewAllEquipments", new { id = existingEquipment.LabId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating equipment: {ex.Message}";
                ViewBag.Labs = labManager.getAllLabs();
                return View(equipment);
            }
        }

        // Save changes

        public List<Equipment> GetAllEquipments()
        {
            return equipmentManager.GetAllEquipments();
        }
        [HttpGet]
        public IActionResult GetEquipmentByLabId(int? id) // Make 'id' nullable to handle cases where it's not provided
        {
            List<Equipment> equipmentList;
            string labName = "All Labs"; // Default label

            if (id.HasValue)
            {
                // Check if the Lab actually exists
                var lab = labManager.GetLabById(id.Value); // Assuming GetLabById returns Lab object or null
                if (lab != null)
                {
                    equipmentList = equipmentManager.GetEquipmentsByLabId(id.Value); // Fetch equipment for specific lab
                    labName = lab.Name; // Set the actual lab name
                }
                else
                {
                    // If ID is provided but lab doesn't exist
                    TempData["ErrorMessage"] = $"Lab with ID {id.Value} not found.";
                    equipmentList = new List<Equipment>(); // Return empty list
                    labName = "Unknown Lab";
                }
            }
            else
            {
                // If no ID is provided, typically you'd show ALL equipment
                equipmentList = equipmentManager.GetAllEquipments(); // You need to implement this in EquipmentManager
                TempData["Message"] = "Displaying equipment from all labs.";
            }

            ViewBag.CurrentLabId = id; // Pass the id to the view, even if null, for the "Add Equipment" link
            ViewBag.CurrentLabName = labName; // Pass the lab name for display

            return View(equipmentList); // Pass the list of equipment as the model
        }

        [HttpPost]
        public IActionResult updateBookingStatus(int id, Booking.BookStatus status)
        {
            try
            {
                superManager.updateBookingStatus(id, status);
                TempData["Message"] = "Booking status updated and notification sent.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }

            return RedirectToAction("getAllBookingsSupervisour");
        }

 
        [HttpGet]
        public IActionResult getAllBookingsSupervisour(string name, string date, string equipment, string lab, string sortOrder)
        {
            var bookings = superManager.getAllBookingBySupervisourId();
            var filtered = bookings
                .Where(b => b.status == Booking.BookStatus.Pending)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(name))
                filtered = filtered.Where(b => b.Client.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(date) && DateTime.TryParse(date, out var parsedDate))
                filtered = filtered.Where(b => b.Date.Date == parsedDate.Date);

            if (!string.IsNullOrWhiteSpace(equipment))
                filtered = filtered.Where(b => b.Equipment.Name.Contains(equipment, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(lab))
                filtered = filtered.Where(b => b.Equipment.Lab.Name.Contains(lab, StringComparison.OrdinalIgnoreCase));

            // Default to descending if sortOrder is null/empty
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "desc" : sortOrder;

            filtered = sortOrder == "desc"
                ? filtered.OrderByDescending(b => b.Date)
                : filtered.OrderBy(b => b.Date);

            // Preserve search values in view
            ViewBag.SearchName = name;
            ViewBag.SearchDate = date;
            ViewBag.SearchEquipment = equipment;
            ViewBag.SearchLab = lab;
            ViewBag.SortOrder = sortOrder;

            return View(filtered.ToList());
}


       [HttpGet]
        public IActionResult ApprovedBookings(string name, string date, string equipment, string lab, string sortOrder)
        {
            var bookings = superManager.getAllBookingBySupervisourId();
            var approved = bookings
                .Where(b => b.status == Booking.BookStatus.Approved)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(name))
                approved = approved.Where(b => b.Client.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(date) && DateTime.TryParse(date, out DateTime parsedDate))
                approved = approved.Where(b => b.Date.Date == parsedDate.Date);

            if (!string.IsNullOrWhiteSpace(equipment))
                approved = approved.Where(b => b.Equipment.Name.Contains(equipment, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(lab))
                approved = approved.Where(b => b.Equipment.Lab.Name.Contains(lab, StringComparison.OrdinalIgnoreCase));

            // Sorting
            approved = sortOrder == "desc"
                ? approved.OrderByDescending(b => b.Date)
                : approved.OrderBy(b => b.Date);

            // Preserve values in ViewBag for form
            ViewBag.SearchName = name;
            ViewBag.SearchDate = date;
            ViewBag.SearchEquipment = equipment;
            ViewBag.SearchLab = lab;
            ViewBag.SortOrder = sortOrder;

            return View("ApprovedBookings", approved.ToList());
        }



         [HttpGet]
        public IActionResult RejectedBookings(string name, string date, string equipment, string lab, string sortOrder)
        {
            var bookings = superManager.getAllBookingBySupervisourId();
            var rejected = bookings
                .Where(b => b.status == Booking.BookStatus.Rejected)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                rejected = rejected.Where(b => b.Client.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(date) && DateTime.TryParse(date, out DateTime parsedDate))
                rejected = rejected.Where(b => b.Date.Date == parsedDate.Date);

            if (!string.IsNullOrWhiteSpace(equipment))
                rejected = rejected.Where(b => b.Equipment.Name.Contains(equipment, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(lab))
                rejected = rejected.Where(b => b.Equipment.Lab.Name.Contains(lab, StringComparison.OrdinalIgnoreCase));

          rejected = sortOrder == "desc"
            ? rejected.OrderByDescending(b => b.Date)
            : rejected.OrderBy(b => b.Date);


            ViewBag.SearchName = name;
            ViewBag.SearchDate = date;
            ViewBag.SearchEquipment = equipment;
            ViewBag.SearchLab = lab;
            ViewBag.SortOrder = sortOrder;

            return View("RejectedBookings", rejected.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewEquipment(EquipmentDTO equipmentDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Labs = labManager.getAllLabs();
                return View(equipmentDto);
            }

            try
            {
                string imagePath = equipmentDto.ImageFile != null
                    ? "/img/" + equipmentDto.ImageFile.FileName
                    : "/img/courses-6.jpg";

                var newEquipment = new Equipment
                {
                    Name = equipmentDto.Name,
                    Description = equipmentDto.Description,
                    Quantity = equipmentDto.Quantity,
                    Price = equipmentDto.Price,
                    status = equipmentDto.status,
                    LabId = equipmentDto.LabId,
                    ImagePath = imagePath,
                    Link = equipmentDto.linkUrl
                };

                var addedEquipment = equipmentManager.AddEquipment(newEquipment);

                if (addedEquipment == null || addedEquipment.Id == 0)
                {
                    throw new Exception("Failed to save equipment to database");
                }

                TempData["Message"] = "Equipment added successfully!";
                return RedirectToAction("GetEquipmentByLabId", new { id = addedEquipment.LabId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding equipment: {ex.Message}";
                ViewBag.Labs = labManager.getAllLabs();
                return View(equipmentDto);
            }
        }

        public IActionResult DeleteEquipment(int id) // Action name is DeleteEquipment, no need for ActionName attribute if matching route
        {
            int? labId = null;

            try
            {
                var equipment = equipmentManager.GetEquipmentById(id);
                if (equipment != null)
                {
                    labId = equipment.LabId;
                }

                var deleted = equipmentManager.DeleteEquipment(id);
                if (deleted)
                {
                    TempData["Message"] = "Equipment deleted successfully.";
                    if (labId.HasValue)
                    {
                        return RedirectToAction("GetEquipmentByLabId", new { id = labId.Value });
                    }
                    else
                    {
                        return RedirectToAction("GetAllEquipments");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete equipment: Equipment not found or an error occurred.";
                    // Redirect back to where they were trying to delete from
                    if (labId.HasValue)
                    {
                        return RedirectToAction("GetEquipmentByLabId", new { id = labId.Value });
                    }
                    else
                    {
                        return RedirectToAction("GetAllEquipments");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting equipment (ID: {id}): {ex.Message}");
                TempData["ErrorMessage"] = $"An error occurred while deleting equipment: {ex.Message}";
                if (labId.HasValue)
                {
                    return RedirectToAction("GetEquipmentByLabId", new { id = labId.Value });
                }
                else
                {
                    return RedirectToAction("GetAllEquipments");
                }
            }
        }

    }
}



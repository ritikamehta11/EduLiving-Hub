﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EduHubLiving.Helpers;
using EduHubLiving.Models;
using EduHubLiving.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;

namespace EduHubLiving.Controllers
{
    public class PropertyListingDataController : ApiController
    {
        private readonly PropertyListingService _propertyListingService;

        public PropertyListingDataController()
        {
            var db = new ApplicationDbContext();
            _propertyListingService = new PropertyListingService(db);
        }

        /// <summary>
        /// Retrieves all property listings.
        /// </summary>
        /// <returns>An HTTP response message containing the list of property listings.</returns>
        /// <params>
        ///     FilterPropertyDto (optional): Object containing filter criteria.
        /// </params>
        /// <example>
        ///     curl -X POST -k -H "Content-Type: application/json" -d http://localhost:port/api/property-listings
        /// </example>
        [HttpGet]
        [Route("api/property-listings")]
        public IHttpActionResult PropertyListings([FromUri] FilterPropertyDto filter)
        {
            filter = filter == null ? new FilterPropertyDto() : filter;

            // Get the filtered property listings
            var propertyListings = _propertyListingService.GetAllWithDetails(filter);

            List<PropertyListingDto> propertyListingDtos = propertyListings
                .Select(propertyListing => propertyListing.ToPropertyListingDto(Url))
                .ToList();

            return ResponseHelper.JsonResponse("Property listings retrieved successfully", HttpStatusCode.OK, true, data: propertyListingDtos);
        }

        /// <summary>
        /// Retrieves property listings for the authenticated user.
        /// </summary>
        /// <returns>An HTTP response message containing the list of property listings.</returns>
        /// <example>
        ///     curl --location 'https://localhost:44343/api/user/property-listings' \
        ///        --header 'Accept: application/json' \
        ///        --header 'Content-Type: application/json' \
        ///        --header 'Cookie: .AspNet.ApplicationCookie=XXXX'
        /// </example>
        [HttpGet]
        [Authorize]
        [Route("api/user/property-listings")]
        public IHttpActionResult PropertyListingsForAuthUser()
        {
            string userId = User.Identity.GetUserId();
            var propertyListings = _propertyListingService.GetByUserId(userId);

            List<PropertyListingDto> propertyListingDtos = propertyListings
                .Select(propertyListing => propertyListing.ToPropertyListingDto(Url))
                .ToList();


            return ResponseHelper.JsonResponse("Property listings retrieved successfully", HttpStatusCode.OK, true, data: propertyListingDtos);
        }

        /// <summary>
        /// Stores a new property listing.
        /// </summary>
        /// <returns>An HTTP response message containing the result of the operation.</returns>
        /// <example>
        ///     curl --location --request PUT 'https://localhost:44343/api/property-listings/7' \
        ///     --header 'Accept: application/json' \
        ///     --header 'Cookie: .AspNet.ApplicationCookie=XJX2zTQwc4Ws9fcoa1oDP5RAGjpg5CwWoJ_3zP-W_WIZX_hicl4hY8GNfeMt7bVcINlJiKP9VRESOyGEcCBUZ16hp7WOQ8zRTF5XpEHzyuvuYx9gLyfrwyxCANlar5egmeRKheSlduiuC7FWuoSS0lXOk4KSGDRqaIw-2gKPcULIXPf6Z3elbiN5f0ic7D-lWF29wuIDMFe0LJlCxRFTyF9vRqPzqz85qc-V3jOGNQUB07_qTlbXYq8gMFy8ke_c9aP3xcezJeaRqnRTN74bvGL1cGD78z2NcE6Ih6fF29BR7t1DCK7ajhpcDcLz5iuE_a0VG7XDZQ9oMVSQfJiWrZuqWeefnDywhrpyCcPhzOH1FjD7OavyMLBIrWDvIScR866ByHREoJTN4ZgSrr5suWXZ0wagmr0wzfLvagMB6QcEMNS6q447A-SXBHTnkOfB0JWEn5dEhZRfKDpEK-tvVgaUcsomgdpHeLcJ45cpD07gNJQb1F7DjeHpKzcC4wEv; __RequestVerificationToken=P_C40Bhkzz1plC_cIHQc9F9rH-1vsO3f6NGS7eWohYCa2ZWy2PsZOFAyJEE1bUTh6XfszVsQyPSoG-zwuzydc9RmvwkbC7BgnD47Kw8qAP01' \
        ///     --form 'Name="Sample Property-2"' \
        ///     --form 'Price="100000"' \
        ///     --form 'NoBedRooms="3"' \
        ///     --form 'NoBathRooms="2"' \
        ///     --form 'SquareFootage="1500"' \
        ///     --form 'Description="A beautiful property"' \
        ///     --form 'Status="pending"' \
        ///     --form 'Type="rent"' \
        ///     --form 'Features[]="Wifi"' \
        ///     --form 'Features[]="Hydro"' \
        ///     --form 'media=@"/C:/Users/DELL/Desktop/important items/20211026_131514.jpg"'
        /// </example>
        [HttpPost]
        [Authorize]
        [Route("api/property-listings")]
        public async Task<IHttpActionResult> StorePropertyListing()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return ResponseHelper.JsonResponse("Unsupported media type", HttpStatusCode.UnsupportedMediaType, false);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            // Log request content
            var requestContent = await Request.Content.ReadAsStringAsync();
            Debug.WriteLine($"Received request content: {requestContent}");

            // Log uploaded files
            foreach (var content in provider.Contents)
            {
                Debug.WriteLine($"Uploaded file: {content.Headers.ContentDisposition.FileName}");
            }

            PropertyListingDto propertyListingDto = new PropertyListingDto();
            string uploadDirectory = HttpContext.Current.Server.MapPath("~/Uploads");
            List<Media> mediaFiles = await _propertyListingService.HandleFileUpload(provider.Contents, uploadDirectory);

            propertyListingDto.Name = HttpContext.Current.Request.Form["Name"];
            propertyListingDto.Price = Convert.ToDecimal(HttpContext.Current.Request.Form["Price"]);
            propertyListingDto.NoBedRooms = Convert.ToInt32(HttpContext.Current.Request.Form["NoBedRooms"]);
            propertyListingDto.NoBathRooms = Convert.ToInt32(HttpContext.Current.Request.Form["NoBathRooms"]);
            propertyListingDto.SquareFootage = Convert.ToDecimal(HttpContext.Current.Request.Form["SquareFootage"]);
            propertyListingDto.Description = HttpContext.Current.Request.Form["Description"];
            propertyListingDto.Status = HttpContext.Current.Request.Form["Status"];
            propertyListingDto.Type = HttpContext.Current.Request.Form["Type"];
            propertyListingDto.Features = HttpContext.Current.Request.Form.GetValues("Features[]");

            string availabilityDate = HttpContext.Current.Request.Form["AvailabilityDate"];
            if (!string.IsNullOrEmpty(availabilityDate) && DateTime.TryParse(availabilityDate, out DateTime dateValue))
            {
                propertyListingDto.AvailabilityDate = Convert.ToDateTime(availabilityDate);
            }
            else
            {
                propertyListingDto.AvailabilityDate = null;
            }

            propertyListingDto.LeaseTerm = HttpContext.Current.Request.Form["LeaseTerm"];
            propertyListingDto.LandLordPhoneNumber = HttpContext.Current.Request.Form["LandLordPhoneNumber"];
            propertyListingDto.LandlordEmail = HttpContext.Current.Request.Form["LandlordEmail"];

            if (propertyListingDto == null)
            {
                return ResponseHelper.JsonResponse("Property listing data is null", HttpStatusCode.BadRequest, false);
            }

            if (!_propertyListingService.ValidatePropertyListing(propertyListingDto, out var validationResults))
            {
                var errors = validationResults.ToDictionary(
                    vr => vr.MemberNames.FirstOrDefault() ?? string.Empty,
                    vr => vr.ErrorMessage
                );

                return ResponseHelper.JsonResponse("Request is invalid", HttpStatusCode.BadRequest, false, errors: errors);
            }

            var existingListing = await _propertyListingService.GetByNameAsync(propertyListingDto.Name);

            if (existingListing != null)
            {
                return ResponseHelper.JsonResponse("Property name has been taken", HttpStatusCode.BadRequest, false, errors: new
                {
                    Name = "Name has been taken"
                });
            }

            var propertyListing = propertyListingDto.HydrateModel();
            propertyListing.UserId = User.Identity.GetUserId();

            await _propertyListingService.AddAsync(propertyListing);
            await _propertyListingService.SaveMediaFilesAsync(mediaFiles, propertyListing);

            _propertyListingService.LoadMediaItems(propertyListing);
            _propertyListingService.LoadUser(propertyListing);

            propertyListingDto = propertyListing.ToPropertyListingDto(Url);

            return ResponseHelper.JsonResponse("Property listing created successfully", HttpStatusCode.Created, true, propertyListingDto);
        }


        /// <summary>
        /// Retrieves a property listing by its slug.
        /// </summary>
        /// <param name="slug">The slug of the property listing.</param>
        /// <returns>An HTTP response message containing the property listing.</returns>
        /// <example>
        ///     curl -X GET "http://localhost:port/api/property-listings/{slug}"
        /// </example>
        [HttpGet]
        [Route("api/property-listings/{slug}")]
        public IHttpActionResult GetPropertyListingBySlug(string slug)
        {
            var propertyListing = _propertyListingService.GetBySlug(slug);

            if (propertyListing == null)
            {
                return ResponseHelper.JsonResponse("Property listing not found", HttpStatusCode.NotFound, false);
            }

            _propertyListingService.LoadMediaItems(propertyListing);
            _propertyListingService.LoadUser(propertyListing);

            var propertyListingDto = propertyListing.ToPropertyListingDto(Url);
            return ResponseHelper.JsonResponse("Property listing retrieved successfully", HttpStatusCode.OK, true, propertyListingDto);
        }

        /// <summary>
        /// Updates an existing property listing.
        /// </summary>
        /// <param name="id">The ID of the property listing to update.</param>
        /// <param name="updatedDto">The updated details of the property listing.</param>
        /// <returns>An HTTP response message containing the result of the operation.</returns>
        /// <example>
        ///     refer to store for payload
        /// </example>
        [HttpPut]
        [Authorize]
        [Route("api/property-listings/{id}")]
        public async Task<IHttpActionResult> UpdatePropertyListing(int id)
        {
            // Check if the user is authorized to update this property listing
            var userId = User.Identity.GetUserId();
            var propertyListing = _propertyListingService.GetById(id);

            if (propertyListing == null)
            {
                return ResponseHelper.JsonResponse("Property listing not found", HttpStatusCode.NotFound, false);
            }

            if (propertyListing.UserId != userId)
            {
                return ResponseHelper.JsonResponse("Unauthorized", HttpStatusCode.Unauthorized, false);
            }

            // Read the multipart form data from the request
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            // Extract the media files from the form data and handle the file uploads
            string uploadDirectory = HttpContext.Current.Server.MapPath("~/Uploads");
            List<Media> mediaFiles = await _propertyListingService.HandleFileUpload(provider.Contents, uploadDirectory);

            PropertyListingDto updatedDto = new PropertyListingDto();

            updatedDto.Name = HttpContext.Current.Request.Form["Name"];
            updatedDto.Price = Convert.ToDecimal(HttpContext.Current.Request.Form["Price"]);
            updatedDto.NoBedRooms = Convert.ToInt32(HttpContext.Current.Request.Form["NoBedRooms"]);
            updatedDto.NoBathRooms = Convert.ToInt32(HttpContext.Current.Request.Form["NoBathRooms"]);
            updatedDto.SquareFootage = Convert.ToDecimal(HttpContext.Current.Request.Form["SquareFootage"]);
            updatedDto.Description = HttpContext.Current.Request.Form["Description"];
            updatedDto.Status = HttpContext.Current.Request.Form["Status"];
            updatedDto.Type = HttpContext.Current.Request.Form["Type"];
            updatedDto.Features = HttpContext.Current.Request.Form.GetValues("Features[]");

            string availabilityDate = HttpContext.Current.Request.Form["AvailabilityDate"];
            if (!string.IsNullOrEmpty(availabilityDate) && DateTime.TryParse(availabilityDate, out DateTime dateValue))
            {
                updatedDto.AvailabilityDate = Convert.ToDateTime(availabilityDate);
            }
            else
            {
                updatedDto.AvailabilityDate = null;
            }

            updatedDto.LeaseTerm = HttpContext.Current.Request.Form["LeaseTerm"];
            updatedDto.LandLordPhoneNumber = HttpContext.Current.Request.Form["LandLordPhoneNumber"];
            updatedDto.LandlordEmail = HttpContext.Current.Request.Form["LandlordEmail"];

            // Validate the updated property listing
            if (!_propertyListingService.ValidatePropertyListing(updatedDto, out var validationResults))
            {
                var errors = validationResults.ToDictionary(
                    vr => vr.MemberNames.FirstOrDefault() ?? string.Empty,
                    vr => vr.ErrorMessage
                );

                return ResponseHelper.JsonResponse("Request is invalid", HttpStatusCode.BadRequest, false, errors: errors);
            }

            // Update the property listing
            propertyListing.UpdateFromDto(updatedDto);
            await _propertyListingService.UpdateAsync(propertyListing);
            await _propertyListingService.SaveMediaFilesAsync(mediaFiles, propertyListing);

            // Load related entities
            _propertyListingService.LoadMediaItems(propertyListing);
            _propertyListingService.LoadUser(propertyListing);

            var propertyListingDto = propertyListing.ToPropertyListingDto(Url);
            return ResponseHelper.JsonResponse("Property listing updated successfully", HttpStatusCode.OK, true, propertyListingDto);
        }

        /// <summary>
        /// Deletes a property listing.
        /// </summary>
        /// <param name="id">The ID of the property listing to delete.</param>
        /// <returns>An HTTP response message containing the result of the operation.</returns>
        /// <example>
        ///     curl --location --request DELETE 'https://localhost:44343/api/property-listings/5' --header 'Accept: application/json' --header 'Content-Type: application/json' --header 'Cookie: __RequestVerificationToken=P_C40Bhkzz1plC_cIHQc9F9rH-1vsO3f6NGS7eWohYCa2ZWy2PsZOFAyJEE1bUTh6XfszVsQyPSoG-zwuzydc9RmvwkbC7BgnD47Kw8qAP01'
        /// <example>
        [HttpDelete, HttpPost]
        [Authorize]
        [Route("api/property-listings/{id}")]
        public async Task<IHttpActionResult> DeletePropertyListing(int id)
        {
            // Check if the user is authorized to delete this property listing
            var userId = User.Identity.GetUserId();
            var propertyListing = _propertyListingService.GetById(id);

            if (propertyListing == null)
            {
                return ResponseHelper.JsonResponse("Property listing not found", HttpStatusCode.NotFound, false);
            }

            if (propertyListing.UserId != userId)
            {
                return ResponseHelper.JsonResponse("Unauthorized", HttpStatusCode.Unauthorized, false);
            }

            // Delete the property listing
            await _propertyListingService.DeleteAsync(propertyListing);

            return ResponseHelper.JsonResponse("Property listing deleted successfully", HttpStatusCode.OK, true);
        }
    }
}

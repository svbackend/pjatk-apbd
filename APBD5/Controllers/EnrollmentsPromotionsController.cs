using System;
using APBD3.DAL;
using APBD3.Exceptions;
using APBD3.Models;
using APBD3.Requests;
using APBD3.Responses;
using Microsoft.AspNetCore.Mvc;

namespace APBD3.Controllers
{
    [ApiController]
    [Route("api/enrollments/promotions")]
    public class EnrollmentsPromotionsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public EnrollmentsPromotionsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult CreatePromotion(CreatePromotionDto createPromotionDto)
        {
            try
            {
                Enrollment enrollment = _dbService.EnrollStudent(createPromotionDto);
                return Created($"/api/enrollments/{enrollment.IdEnrollment}", enrollment);
            }
            catch (StudiesNotFoundException e)
            {
                return BadRequest(new BadRequestDto("Provided studies not found!"));
            }
            catch (StudentAlreadyExistsException e)
            {
                return BadRequest(new BadRequestDto("Student with such Index Number already exist!"));
            }
        }
    }
}
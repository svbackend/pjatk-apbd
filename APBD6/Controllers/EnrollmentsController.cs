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
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public EnrollmentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult CreateEnrollment(CreateEnrollmentDto createEnrollmentDto)
        {
            try
            {
                Enrollment enrollment = _dbService.EnrollStudent(createEnrollmentDto);
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
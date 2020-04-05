using System;
using APBD3.DAL;
using APBD3.Models;
using APBD3.Requests;
using Microsoft.AspNetCore.Mvc;

namespace APBD3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public EnrollmentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult CreateEnrollment(CreateEnrollmentRequest request)
        {
            //todo
            return Ok(request);
        }
    }
}
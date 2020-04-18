using System;
using APBD3.DAL;
using APBD3.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD3.Controllers
{
    [ApiController]
    [Route("api/students/{id}")]
    public class StudentsEntriesController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsEntriesController(IDbService dbService)
        {
            _dbService = dbService;
        }
        
        [HttpGet("entries")]
        public IActionResult GetStudentEntries(int id)
        {
            return Ok(_dbService.GetStudentEntries(id));
        }
    }
}
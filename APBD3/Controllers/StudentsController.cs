using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD3.DAL;
using APBD3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APBD3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Test1");
            } else if (id == 2)
            {
                return Ok("Test2");
            }
            
            return NotFound("Student not found");
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(Student student)
        {
            return Ok("Update completed");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(Student student)
        {
            return Ok("Delete completed");
        }
    }
}
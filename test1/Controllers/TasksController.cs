using System;
using APBD3.DAL;
using APBD3.Exceptions;
using APBD3.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD3.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IDbService _dbService;

        public TasksController(IDbService dbService)
        {
            _dbService = dbService;
        }
        
        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            try
            {
                TeamMember teamMember = _dbService.GetTeamMember(id);
                return Ok(teamMember);
            }
            catch (TeamMemberNotFound e)
            {
                return NotFound();
            }
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                _dbService.DeleteProject(id);
                return Ok();
            }
            catch (TeamMemberNotFound e)
            {
                return NotFound();
            }
        }
    }
}
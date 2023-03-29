﻿using DSUContextDBService.Interface;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DsuController : Controller
    {
        private readonly IDsuDbService _dsuDbService;

        public DsuController(IDsuDbService dsuDbService)
        {
            _dsuDbService = dsuDbService;
        }

        [Route("GetFaculties")]
        [HttpGet]
        public async Task<IActionResult> GetFaculties()
        {
            return Ok(await _dsuDbService.GetFaculties().ToListAsync());
        }

        [Route("GetCaseSDepartmentByFacultyId")]
        [HttpGet]
        public async Task<IActionResult> GetCaseSDepartmentByFacultyId(int facultyId)
        {
            return Ok(await _dsuDbService.GetCaseSDepartmentByFacultyId(facultyId).ToListAsync());
        }

        [Route("GetCourseByDepartmentId")]
        [HttpGet]
        public async Task<IActionResult> GetCourseByDepartmentId(int departmentId)
        {
            return Ok(await _dsuDbService.GetCoursesByDepartmentId(departmentId).ToListAsync());
        }

        [Route("GetStudentsByDepartmentAndCourse")]
        [HttpGet]
        public async Task<IActionResult> GetStudentsByDepartmentAndCourse(int departmentId, int course)
        {
            var students = await _dsuDbService.GetCaseSStudents().Where(x => x.DepartmentId == departmentId && x.Course == course).ToListAsync();
            return Ok(students);
        }
    }
}
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

        [Route("GetCourseByDepartmentId")]
        [HttpGet]
        public async Task<IActionResult> GetGroupsByDepartmentId(int departmentId, int course)
        {
            return Ok(await _dsuDbService.GetGroupsByDepartmentId(departmentId, course).ToListAsync());
        }

        [Route("GetStudentsByDepartmentAndCourse")]
        [HttpGet]
        public async Task<IActionResult> GetStudentsByDepartmentAndCourse(int departmentId, int course, string ngroup)
        {
            var students = await _dsuDbService.GetCaseSStudents().Where(x => x.DepartmentId == departmentId && x.Course == course && x.Ngroup == ngroup).ToListAsync();
            return Ok(students);
        }

        [Route("GetTeachers")]
        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            return Ok(await _dsuDbService.GetCaseSTeachers().ToListAsync());
        }
    }
}

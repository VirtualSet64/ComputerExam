﻿using DSUContextDBService.DBService;
using DSUContextDBService.Interface;
using DSUContextDBService.Models;

namespace DSUContextDBService.Services
{
    public class DsuDbService : IDsuDbService
    {
        private readonly DSUContext _dSUContext;

        public DsuDbService(DSUContext dSUContext)
        {
            _dSUContext = dSUContext;
        }

        #region Faculties
        public IQueryable<CaseCFaculty> GetFaculties()
        {
            return _dSUContext.CaseCFaculties.Where(x => x.Deleted == false).OrderBy(x => x.FacName);
        }

        public CaseCFaculty GetFacultyById(int? id)
        {
            return _dSUContext.CaseCFaculties.FirstOrDefault(x => x.FacId == id);
        }
        #endregion

        #region Departments
        public CaseSDepartment GetCaseSDepartmentById(int? id)
        {
            return _dSUContext.CaseSDepartments.FirstOrDefault(x => x.DepartmentId == id);
        }

        public IQueryable<CaseSDepartment> GetCaseSDepartmentByFacultyId(int? id)
        {
            var students = GetCaseSStudents();
            return _dSUContext.CaseSDepartments.Where(x => x.Deleted == false && x.FacId == id && students.Any(c => c.DepartmentId == x.DepartmentId)).OrderBy(x => x.DeptName);
        }

        public IQueryable<CaseSDepartment> GetCaseSDepartments()
        {
            return _dSUContext.CaseSDepartments.Where(x => x.Deleted == false).OrderBy(x => x.DeptName);
        }
        #endregion

        #region Specializations
        public IQueryable<CaseSSpecialization> GetCaseSSpecializations()
        {
            return _dSUContext.CaseSSpecializations.Where(x => x.Deleted == false).OrderBy(x => x.SpecName);
        }

        public CaseSSpecialization GetCaseSSpecializationById(int? id)
        {
            return _dSUContext.CaseSSpecializations.FirstOrDefault(x => x.SpecId == id);
        }
        #endregion

        #region Students
        public IQueryable<CaseSStudent> GetCaseSStudents()
        {
            return _dSUContext.CaseSStudents.Where(x => x.Status == 0)
                                            .OrderBy(x => x.Lastname)
                                            .ThenBy(x => x.Firstname)
                                            .ThenBy(x => x.Patr);
        }

        public CaseSStudent GetCaseSStudentById(int? id)
        {
            return _dSUContext.CaseSStudents.FirstOrDefault(x => x.Id == id);
        }
        #endregion

        #region Teachers
        public IQueryable<CaseSTeacher> GetCaseSTeachers()
        {
            return _dSUContext.CaseSTeachers.Where(x => x.TeachId > 0);
        }

        public CaseSTeacher GetCaseSTeacherById(int? id)
        {
            return _dSUContext.CaseSTeachers.FirstOrDefault(x => x.TeachId == id);
        }
        #endregion

        public IQueryable<int?> GetCoursesByDepartmentId(int departmentId)
        {
            var courses = _dSUContext.CaseSStudents.Where(x => x.DepartmentId == departmentId).Select(c => c.Course);
            return courses.Distinct().OrderBy(x => x);
        }

        public IQueryable<string?> GetGroupsByDepartmentId(int departmentId, int course)
        {
            var ngroup = _dSUContext.CaseSStudents.Where(x => x.DepartmentId == departmentId && x.Course == course).Select(c => c.Ngroup);
            return ngroup.Distinct().OrderBy(x => x);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Models.SchoolContext _context;

        public IndexModel(ContosoUniversity.Models.SchoolContext context)
        {
            _context = context;
        }

		public InstructorIndexData Instructor { get; set; }
		public int InstructorID { get; set; }
		public int CourseID { get; set; }

		public async Task OnGetAsync(int? id, int? courseID)
        {
			Instructor = new InstructorIndexData();
			Instructor.Instructors = await _context.Instructors.Include(i => i.OfficeAssignment)
									.Include(i => i.CourseAssignments)
									.ThenInclude(i => i.Course)
									.ThenInclude(i => i.Department)
									//.Include(i => i.CourseAssignments)
									//.ThenInclude(i => i.Course)
									//.ThenInclude(i => i.Enrollments)
									//.ThenInclude(i => i.Student)
									//.AsNoTracking()
									.OrderBy(i => i.LastName)
									.ToListAsync();

			if (id != null)
			{
				InstructorID = id.Value;
				//Instructor instructor = Instructor.Instructors.Where(
				//	i => i.ID == id.Value).Single(); //get single value of selected instructor
				Instructor instructor = Instructor.Instructors.Single(i => i.ID == id.Value);
				Instructor.Courses = instructor.CourseAssignments.Select(c => c.Course); //Get a list of courses
			}

			/*if (courseID != null)
			{
				CourseID = courseID.Value;
				//Instructor.Enrollments = Instructor.Courses.Where(x => x.CourseID == courseID).Single().Enrollments;
				Instructor.Enrollments = Instructor.Courses.Single(x => x.CourseID == courseID).Enrollments;
				//Get enrollments for the seelected course
			}*/
			if (courseID != null)
			{
				var selectedCourse = Instructor.Courses.Where(x => x.CourseID == courseID).Single(); //Single converts from being a list to a single object
				await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync(); //Loads entities linked to main entity
				foreach (Enrollment enrollment in selectedCourse.Enrollments)
				{
					await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync(); //Loads a single field
				}
				Instructor.Enrollments = selectedCourse.Enrollments;
			}
        }
    }
}

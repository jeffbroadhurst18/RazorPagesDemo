using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.SchoolViewModels
{
	public class InstructorIndexData
	{
		[Key]
		public int InstructorIndexDataId { get; set; }

		public IEnumerable<Instructor> Instructors { get; set; }
		
		public IEnumerable<Course> Courses { get; set; }
		
		public IEnumerable<Enrollment> Enrollments { get; set; }
	}

	
}

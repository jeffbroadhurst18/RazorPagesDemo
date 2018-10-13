using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
	public class Course
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]  //Supplied by the application rather than generated.

		[Display(Name = "Number")]
		public int CourseID { get; set; }

		[StringLength(50,MinimumLength = 3)]
		public string Title { get; set; }

		[Range(0,5)]
		public int Credits { get; set; }

		public int DepartmentID { get; set; }

		public Department Department { get; set; }
		public ICollection<Enrollment> Enrollments { get; set; }
		public ICollection<CourseAssignment> CourseAssignments { get; set; }
	}
}
// When the FK property DepartmentID is included in the data model, there's no need to fetch the Department entity before an update.

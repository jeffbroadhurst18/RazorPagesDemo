using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
	public class Department
	{
		public int DepartmentID { get; set; } //As prefix is name of class EF knows to make this a primary key.

		[StringLength(50, MinimumLength = 3)]
		public string Name { get; set; }

		[DataType(DataType.Currency)]
		[Column(TypeName = "money")] //Sets the datatype on the database to be money.
		public decimal Budget { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Start Date")]
		public DateTime StartDate { get; set; }

		public int? InstructorID { get; set; } //saves having to load whole Instructor entity when saving.
		                                       //property is nullable so instructor isn't deleted when department is deleted.

		public Instructor Administrator { get; set; }  //Administrator is ALWAYS an instructor
		public ICollection<Course> Courses { get; set; }
	}
}
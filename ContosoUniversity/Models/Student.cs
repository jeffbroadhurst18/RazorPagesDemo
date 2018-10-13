using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
	public class Student
	{
		public int ID { get; set; }
		[StringLength(50, ErrorMessage = "LastName name cannot be longer than 50 characters", MinimumLength = 1)]
		[RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		[StringLength(50, ErrorMessage = "FirstMidName name cannot be longer than 50 characters", MinimumLength = 1)]
		[RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
		[Column("FirstName")] // changes name in the database when migration is run.
		[Display(Name = "First Name")]
		public string FirstMidName { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] //formats as date only while editing.
		[Display(Name = "Enrollment Date")]
		public DateTime EnrollmentDate { get; set; }

		[Display(Name = "Full Name")] //Because there is no SET option, a column is not created in the database.
		public string FullName
		{
			get
			{
				return LastName + ", " + FirstMidName;
			}
		}

		public ICollection<Enrollment> Enrollments { get; set; }
	}
}
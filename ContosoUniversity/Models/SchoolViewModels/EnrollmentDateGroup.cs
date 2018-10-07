using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models.SchoolViewModels
{
	public class EnrollmentDateGroup
	{
		[DataType(DataType.Date)]
		public DateTime? EnrollmentDate { get; set; }
		public int StudentCount { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
		public DateTime? MinDate { get; set; }
		public string FullName { get; set; }
	}
}

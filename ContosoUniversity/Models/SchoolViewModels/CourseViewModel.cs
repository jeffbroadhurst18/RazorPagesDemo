using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models.SchoolViewModels
{
	public class CourseViewModel
	{
		[Key]
		public int CourseID { get; set; }
		public string Title { get; set; }
		public int Credits { get; set; }
		public string DepartmentName { get; set;}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages
{
	public class AboutModel : PageModel
	{
		private readonly SchoolContext _context;

		public AboutModel(SchoolContext context)
		{
			_context = context;
		}

		public IList<EnrollmentDateGroup> Student { get; set; }
		public IList<EnrollmentDateGroup> Student2 { get; set; }


		public async Task OnGetAsync() //same as return void
		{
			IQueryable<EnrollmentDateGroup> data = _context.Student.GroupBy(x => x.EnrollmentDate)
				.Select(y => new EnrollmentDateGroup
				{
					EnrollmentDate = y.Key,
					StudentCount = y.Count()
				});

			Student = await data.AsNoTracking().ToListAsync();

			IQueryable<EnrollmentDateGroup> data2 = _context.Student.GroupBy(y => y.FirstMidName + " " + y.LastName)
			.Select(x => new EnrollmentDateGroup
			{
				FullName = x.Key,
				MinDate = x.Min(z => z.EnrollmentDate)
			});

			Student2 = await data2.AsNoTracking().ToListAsync();
		}
	} // You can do Group by in Linq
	  // Here we group students by date . Set Date to key of group and StudentCount to Count.
}

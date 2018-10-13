using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using System.Linq;

namespace ContosoUniversity.Pages.Students
{
	public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;

        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

		public string NameSort { get; set; }
		public string DateSort { get; set; }
		public string CurrentFilter { get; set; }
		public string CurrentSort { get; set; }

        public PaginatedList<Student> Student { get;set; }

		//just return a model - non of the IActionResult stuff
		//sortOrderx is parameter which matches asp-route-sortOrderx tag in HTML
		public async Task OnGetAsync(string sortOrderx,string currentFilter ,string searchString, int? pageIndex)
        {
			CurrentSort = sortOrderx;
			//This bit just sorts out what the link from the page is going to request next time
			NameSort = string.IsNullOrEmpty(sortOrderx) ? "name_desc" : string.Empty;
			DateSort = sortOrderx == "Date" ? "date_desc" : "Date";
			
			if (searchString != null) {
				pageIndex = 1;
			} else
			{
				searchString = currentFilter;
			}

			CurrentFilter = searchString;

			IQueryable<Student> studentIQ = _context.Students; //IQueryable generic form of list but not executed until 
																//it is converted to a List.

			if (!string.IsNullOrEmpty(searchString))
			{
				studentIQ = studentIQ.Where(s => s.LastName.Contains(searchString) || s.FirstMidName.Contains(searchString));
			}

			switch (sortOrderx)
			{
				case "name_desc":
					studentIQ = studentIQ.OrderByDescending(s => s.LastName);
					break;
				case "Date":
					studentIQ = studentIQ.OrderBy(s => s.EnrollmentDate);
					break;
				case "date_desc":
					studentIQ = studentIQ.OrderByDescending(s => s.EnrollmentDate);
					break;
				default:
					studentIQ = studentIQ.OrderBy(s => s.LastName);
					break;
			}

			int pageSize = 3;
			Student = await PaginatedList<Student>.CreateAsync(studentIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
			//Student = await studentIQ.AsNoTracking().Select(s => s).ToListAsync(); 
			//Actually sends the query to the database and populates the model.
			//pageIndex != null ? pageIndex : 1;

			var depts = _context.Departments;
			foreach(Department d in depts)
			{
				_context.Entry(d).Collection(p => p.Courses).Load();
			}
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexSelectModel : PageModel
    {
        private readonly ContosoUniversity.Models.SchoolContext _context;

        public IndexSelectModel(ContosoUniversity.Models.SchoolContext context)
        {
            _context = context;
        }

        public IList<CourseViewModel> CourseViewModel { get;set; }

        public async Task OnGetAsync()
        {
			CourseViewModel = await _context.Courses.Select(p => new CourseViewModel
			{
				CourseID = p.CourseID,
				Title = p.Title,
				Credits = p.Credits,
				DepartmentName = p.Department.Name
			}).ToListAsync();
        }
    }
}

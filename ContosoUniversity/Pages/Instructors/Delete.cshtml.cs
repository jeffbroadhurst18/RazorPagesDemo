using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using System.Linq;

namespace ContosoUniversity.Pages.Instructors
{
	public class DeleteModel : PageModel
	{
		private readonly SchoolContext _context;

		public DeleteModel(SchoolContext context)
		{
			_context = context;
		}

		[BindProperty]
		public Instructor Instructor { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.ID == id);

			if (Instructor == null)
			{
				return NotFound();
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			//Get the instructor with associated data
			//If you don't include CourseAssignments then they won't be deleted when the instructor is deleted.

			Instructor instructor = await _context.Instructors.Include(i => i.CourseAssignments).SingleAsync(i => i.ID == id);

			// Get any department records containing this instructor
			var departments = await _context.Departments.Where(d => d.InstructorID == id).ToListAsync();

			//The InstructorID represents the administrator for the department
			foreach (var department in departments)
			{
				department.InstructorID = null;
			}

			_context.Instructors.Remove(Instructor);
			await _context.SaveChangesAsync();
			return RedirectToPage("./Index");
		}
	}
}

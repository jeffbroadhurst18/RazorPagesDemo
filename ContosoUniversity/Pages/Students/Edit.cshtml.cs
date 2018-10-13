using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
	public class EditModel : PageModel
    {
        private readonly SchoolContext _context;

        public EditModel(SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			Student = await _context.Students.FindAsync(id); //Find method looks for a single instance of the key value.

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) //have parameter as URL is of form /student/10
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			var studentToUpdate = await _context.Students.FindAsync(id);

			if (await TryUpdateModelAsync(studentToUpdate, "student", s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
			{
				//above code updates the model but does not persist to the database
				
				await _context.SaveChangesAsync();//persist to database
				return RedirectToPage("./Index");
			}
           
			return Page();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
	public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;

        public CreateModel(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			var emptyStudent = new Student();

			//TryUpdateModelAsync checks if the entered value fits into the model with the student prefix
			//ensures only the specified fields are updated not others
			if (await TryUpdateModelAsync(emptyStudent,"student",s => s.FirstMidName, s=> s.LastName, s => s.EnrollmentDate))
			{
				//above code updates the model but does not persist to the database
				_context.Student.Add(emptyStudent); //add to context
				await _context.SaveChangesAsync();//persist to database
				return RedirectToPage("./Index");
			}

			//_context.Student.Add(Student);
			//await _context.SaveChangesAsync();

			return null;
        }
    }
}
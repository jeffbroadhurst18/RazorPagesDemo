using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolContext _context;

        public DeleteModel(SchoolContext context)
        {
            _context = context;
        }

        [BindProperty] //binds to the Page model that is supplied
        public Student Student { get; set; }
		public string ErrorMessage { get; set; }

		//Called when the form is opened
		public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                return NotFound();
            }

			if (saveChangesError.GetValueOrDefault())
			{
				ErrorMessage = "Delete Failed. Try again.";
			}
			
            return Page();
        }

		//Called when delete button is pressed
		public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
			//AsNoTrcking only stops tracking for updates. You can still explicitly Remove the entity.

			if (student == null)
			{
				return NotFound();
			}

			try
            {
				//we know it exists so we can delete it
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}
			catch(Exception)
			{
				return RedirectToAction("./Delete", new { id, saveChangesError = true });
				//goes to delete page but does a GET and uses passed in parameters
			}

            
        }
    }
}

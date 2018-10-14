using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Instructors
{
	public class EditModel : InstructorCoursesPageModel
    {
        private readonly SchoolContext _context;

        public EditModel(SchoolContext context)
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

			//Instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.ID == id);
			//Instructor = await _context.Instructors.Include(i => i.OfficeAssignment).AsNoTracking()
			//			.FirstOrDefaultAsync(m => m.ID == id);

			Instructor = await _context.Instructors.Include(i => i.OfficeAssignment)
							.Include(i => i.CourseAssignments).ThenInclude(i => i.Course)
							.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);


            if (Instructor == null)
            {
                return NotFound();
            }

			PopulateAssignedCourseData(_context, Instructor);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCourses)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			//_context.Attach(Instructor).State = EntityState.Modified;
			var instructorToUpdate = await _context.Instructors
				.Include(i => i.OfficeAssignment)
				.Include(i => i.CourseAssignments)
				.ThenInclude(i => i.Course)
				.FirstOrDefaultAsync(s => s.ID == id);

			// update retrieved record with values from model binding
			if (await TryUpdateModelAsync<Instructor>(instructorToUpdate,"instructor", 
				i => i.FirstMidName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))
			{
				if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
				{
					instructorToUpdate.OfficeAssignment = null; // has effect of deleting record from database.
				}
				UpdateInstructorCourses(_context, selectedCourses, instructorToUpdate);
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}
			//This bit ensures any changes to the selected assignments are maintained
			UpdateInstructorCourses(_context, selectedCourses, instructorToUpdate);
			PopulateAssignedCourseData(_context, instructorToUpdate);
			return Page();
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Models;
using System.Collections.Generic;

namespace ContosoUniversity.Pages.Instructors
{
	public class CreateModel : InstructorCoursesPageModel
	{
		private readonly SchoolContext _context;

		public CreateModel(SchoolContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
			var instructor = new Instructor();
			instructor.CourseAssignments = new List<CourseAssignment>();

			//Return a list of all courses and whether the specified instructor is
			//assigned to that course or not.  Populates AssignedCourseDatalist.  Will be empty new record initially
			PopulateAssignedCourseData(_context, instructor);
			return Page();
		}

		[BindProperty]
		public Instructor Instructor { get; set; }

		public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var newInstructor = new Instructor();
			if (selectedCourses != null)
			{
				newInstructor.CourseAssignments = new List<CourseAssignment>();
				foreach (var course in selectedCourses)
				{
					var courseToAdd = new CourseAssignment
					{
						CourseID = int.Parse(course)
					};
					newInstructor.CourseAssignments.Add(courseToAdd);
				}
			}

			if (await TryUpdateModelAsync<Instructor>(newInstructor, "instructor",
				i => i.FirstMidName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment
				))
			{
				_context.Instructors.Add(newInstructor);
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}

			PopulateAssignedCourseData(_context, newInstructor);
			return Page();
		}
	}
}
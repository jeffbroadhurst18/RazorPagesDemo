using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Departments
{
	public class EditModel : PageModel
	{
		private readonly ContosoUniversity.Models.SchoolContext _context;

		public EditModel(ContosoUniversity.Models.SchoolContext context)
		{
			_context = context;
		}

		[BindProperty]
		public Department Department { get; set; }
		public SelectList InstructorNameSL { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			Department = await _context.Departments
				.Include(d => d.Administrator).AsNoTracking().FirstOrDefaultAsync(m => m.DepartmentID == id);

			if (Department == null)
			{
				return NotFound();
			}
			//ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName");
			InstructorNameSL = new SelectList(_context.Instructors, "ID", "FirstMidName");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var departmentToUpdate = await _context.Departments.Include(i => i.Administrator).FirstOrDefaultAsync(m => m.DepartmentID == id);

			if (departmentToUpdate == null)
			{
				return await HandleDeletedDepartment();
			}

			_context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = Department.RowVersion;
			//The Department.RowVersion is set when the record is read from the database on the initial GET.
			//if 2RowVersion2 has been updated so it doesn't match Department.RowVersion then Concurrency exception is thrown.
			//OriginalValue is the database value just before we tried to save

			if (await TryUpdateModelAsync<Department>(departmentToUpdate, "Department",
				s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID))
			{
				try
				{
					await _context.SaveChangesAsync();
					return RedirectToPage("./Index");
				}



				catch (DbUpdateConcurrencyException ex)
				{
					var exceptionEntry = ex.Entries.Single();
					var clientValues = (Department)exceptionEntry.Entity;
					var databaseEntry = exceptionEntry.GetDatabaseValues();
					if (databaseEntry == null)
					{
						ModelState.AddModelError(string.Empty, "Unable to save. The department was deleted by another user");
						return Page();
					}

					var dbValues = (Department)databaseEntry.ToObject();
					await setDbErrorMessage(dbValues, clientValues, _context);

					Department.RowVersion = (byte[])dbValues.RowVersion;//sets the rowversion to be the correct one from the database
					ModelState.Remove("Department.RowVersion");
				}
			}
			InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName", departmentToUpdate.InstructorID);
			return Page();
		}

		private async Task<IActionResult> HandleDeletedDepartment()
		{
			Department deletedDepartment = new Department();
			ModelState.AddModelError(string.Empty, "Unable to save. The department was deleted by another user");
			InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName",Department.InstructorID);
			return Page();
		}

		private async Task setDbErrorMessage(Department dbValues, Department clientValues, SchoolContext context)
		{
			if (dbValues.Name != clientValues.Name)
			{
				ModelState.AddModelError("Department.Name", $"Current value: {dbValues.Name}");
			}

			if (dbValues.Budget != clientValues.Budget)
			{
				ModelState.AddModelError("Department.Budget", $"Current value: {dbValues.Budget:c}");
			}

			if (dbValues.StartDate != clientValues.StartDate)
			{
				ModelState.AddModelError("Department.StartDate", $"Current value: {dbValues.StartDate:d}");
			}
			if (dbValues.InstructorID != clientValues.InstructorID)
			{
				Instructor dbInstructor = await _context.Instructors.FindAsync(dbValues.InstructorID);
				ModelState.AddModelError("Department.InstructorID", $"Current value: {dbInstructor?.FullName}");
			}
			ModelState.AddModelError(string.Empty,"The record you attempted to edit was modified by another user after you. the edit operation was cancelled and the current values in the database have been displayed. If you still want to edit click the Save button again.");
		}

		private bool DepartmentExists(int id)
		{
			return _context.Departments.Any(e => e.DepartmentID == id);
		}
	}
}

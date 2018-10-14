using System.Linq;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Courses
{
	public class DepartmentNamePageModel : PageModel
    {
        public SelectList DepartmentNameSL { get; set; }

		public void PopulateDepartmentsDropDownList(SchoolContext _context, object selectedDepartment = null)
		{
			var departmentsQuery = _context.Departments.OrderBy(d => d.Name).Select(d => d);

			DepartmentNameSL = new SelectList(departmentsQuery.AsNoTracking(), "DepartmentID", "Name", selectedDepartment);
		}
    }
}
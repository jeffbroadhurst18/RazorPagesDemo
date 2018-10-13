namespace ContosoUniversity.Models
{
	public class CourseAssignment
	{
		public int InstructorID { get; set; }
		public int CourseID { get; set; }
		public Instructor Instructor { get; set; }
		public Course Course {get;set;}
	}
}
//This is a Pure Join Table as there are no fields other than those directly involved in the relationship.
//Included the int fields so that a record can be updated without having to load all of the data from the
//related entity. This requires less database processing.
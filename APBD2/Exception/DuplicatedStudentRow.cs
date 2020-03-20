namespace APBD2.Exception
{
    public class DuplicatedStudentRow : System.Exception
    {
        public DuplicatedStudentRow()
            : base("Student with such First Name, Last Name and Student Number was already added")
        {

        }
    }
}
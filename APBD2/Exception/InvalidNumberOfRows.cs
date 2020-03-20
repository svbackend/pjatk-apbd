namespace APBD2.Exception
{
    public class InvalidNumberOfRows : System.Exception
    {
        public InvalidNumberOfRows(int expected, int given)
            : base($"Invalid number of rows provided, {expected} expected, but {given} given")
        {

        }
    }
}
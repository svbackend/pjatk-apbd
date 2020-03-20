namespace APBD2.Exception
{
    public class EmptyValueProvided : System.Exception
    {
        public EmptyValueProvided()
            : base("All columns are required but empty value provided")
        {

        }
    }
}
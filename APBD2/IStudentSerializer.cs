using System.Collections.Specialized;

namespace APBD2
{
    public interface IStudentSerializer
    {
        public string Serialize(ListDictionary students);
    }
}
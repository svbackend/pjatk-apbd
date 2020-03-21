using System.Collections.Specialized;
using APBD2.Entity;

namespace APBD2.Serializer
{
    public interface IUniversitySerializer
    {
        public string Serialize(University university);
    }
}
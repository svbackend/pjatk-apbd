using System.Text.Json;
using APBD2.Entity;

namespace APBD2.Serializer
{
    public class JsonUniversitySerializer : IUniversitySerializer
    {
        public string Serialize(University university)
        {
            return JsonSerializer.Serialize(new { university = university });
        }
    }
}
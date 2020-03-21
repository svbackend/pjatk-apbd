using System;
using System.IO;
using System.Text;
using APBD2.Entity;

namespace APBD2.Serializer
{
    public class XmlUniversitySerializer : IUniversitySerializer
    {
        public string Serialize(University university)
        {
            using (MemoryStream ms = new MemoryStream())
            using (System.IO.TextWriter textWriter = new System.IO.StreamWriter(ms))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(University));
                serializer.Serialize(textWriter, university);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
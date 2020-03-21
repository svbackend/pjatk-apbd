using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;

namespace APBD2.Converter
{
    public class DateFormatConverter : DateTimeConverter
    {
        DateFormatConverter()
        {
            DateFormat = "";
        }
    }
}
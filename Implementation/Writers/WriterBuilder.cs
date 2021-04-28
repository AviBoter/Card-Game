using Database.API.Card;
using System.Collections.Generic;
using System.IO;

namespace writers
{
    public class WriterBuilder
    {
        public static IFileWriter GetFileWriter(string fullPathToFile)
        {
            var fileExtension = Path.GetExtension(fullPathToFile).ToLower();

            if (fileExtension == ".json")
            {
                return new JsonWriter();
            }
            else if (fileExtension == ".xml")
            {
                return new XmlWriter();
            }
            //else if (fileExtension == AnyOtherType)
            //  return new AnyOtherTypeWriter();
            return null;
        }
    }
}

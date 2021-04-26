using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using readers;

namespace Builders
{
    public class FileReaderBuilder
    {
        public static IFileReader GetFileReader(string fullPathToFile)
        {
            var fileExtension = Path.GetExtension(fullPathToFile).ToLower();
            if (fileExtension == ".json")
            {
                return new JsonReader();
            }
            else if (fileExtension == ".xml")
            {
                return new readers.MyXmlReader();
            }
            return null;
        }
    }
}

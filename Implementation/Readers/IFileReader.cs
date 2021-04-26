using Database.API.Card;
using System.Collections.Generic;

namespace readers
{
    public interface IFileReader
    {
        public Dictionary<string,CardData> ReadFromFile(string path);
    }
}
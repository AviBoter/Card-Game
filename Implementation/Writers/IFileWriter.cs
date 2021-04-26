using Database.API.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFileWriter
{
    public void WriteToFile(string path, CardData[] cards);
}

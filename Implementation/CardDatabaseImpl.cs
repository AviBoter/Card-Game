using Builders;
using Database.API;
using Database.API.Card;
using readers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using writers;

namespace Database.Implementation
{
    public class CardDatabaseImpl : CardDatabase
    {
        public Dictionary<string,CardData> Cards;

        public CardDatabaseImpl()
        {
            Cards = new Dictionary<string, CardData>();
        }

        public override IEnumerable<CardData> AllCards
        {
            get => Cards.Values;
        }

        public override void Init()
        {
            CardData[] CardArray = CardData.LoadAllCards();

            foreach (CardData item in CardArray)
            {
                string path = AssetDatabase.GetAssetPath(item.Art);
                Cards.Add(item.CardName,CardData.BuildNewAsset(item.CardName, item.Cost, path, item.ExtraData));
            }

        }

        public override void ImportCardsFromSource(string source)
        {
            CardData.LoadAllCards();
            IFileReader reader = FileReaderBuilder.GetFileReader(source);
            if (reader == null)
            {
                throw new UnidentifiedCardSourceException();
            }

            Dictionary<string ,CardData> ImportCards = reader.ReadFromFile(source);

            foreach (KeyValuePair<string, CardData> card in ImportCards)
            {
                Cards.Add(card.Key,card.Value);
            }
        }

        public override void ExportCardsToSource(string source)
        {
            var cards = CardData.LoadAllCards();
            IFileWriter fileWriter = WriterBuilder.GetFileWriter(source);

            if(fileWriter == null)
                throw new UnidentifiedCardSourceException();
            else
            fileWriter.WriteToFile(source, cards);

        }

        public override List<CardData> GetCardsByCost(int cost)
        {
            List<CardData> myList = Cards.Values.ToList();

            myList.Sort(
                delegate (CardData pair1,CardData pair2)
                {
                    return pair1.Cost.CompareTo(pair2.Cost);
                }
            );
            return myList;
        }

        public override CardData GetByName(string name)
        {

            foreach (KeyValuePair<string, CardData> card in Cards)
            {
                if (card.Value.CardName.Equals(name))
                    return card.Value;
            }
            return null;
        }

    }
}

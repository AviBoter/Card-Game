using Database.API.Card;
using Database.API.Card.Extra;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using GenericCardFields;

namespace readers
{
    public class JsonReader : IFileReader
    {
        public Dictionary<string, CardData> ReadFromFile(string path)
        {
            var Cards = new Dictionary<string, CardData>();
            var jsonText = File.ReadAllText(path);
            var array = JArray.Parse(jsonText);
            BuildNewCards(array, Cards);

            return Cards;
        }

        private void BuildNewCards(JArray array, Dictionary<string, CardData> Cards)
        {
            foreach (var item in array)
            {
                try
                {
                    var name = (string)item[MyConsts.name];
                    var cost = (int)item[MyConsts.cost];
                    var artPath = (string)item[MyConsts.art_path];
                    var extraDataTypeName = (string)item[MyConsts.extra_data_type_name];
                    var extraData = assign_extradata(extraDataTypeName, item);
                    var card = CardData.BuildNewAsset(name, cost, artPath, extraData);
                    Cards.Add(name, card);
                }
                catch (Exception)
                {
                }
            }
        }

        private ExtraData assign_extradata(string extraDataTypeName, JToken item)
        {
            ExtraData extraData = null;
            try
            {
                switch (extraDataTypeName)
                {
                    case MyConsts.spell_extra_data:
                        var effect = (SpellEffect)(int)item[MyConsts.extra_data][MyConsts.effect];
                        var effectAmount = (int)item[MyConsts.extra_data][MyConsts.effect_amount];
                        extraData = new SpellExtraData(effect, effectAmount);
                        break;
                    case MyConsts.minion_extra_data:
                        var health = (int)item[MyConsts.extra_data][MyConsts.health];
                        var attackDamage = (int)item[MyConsts.extra_data][MyConsts.attack_damage];
                        extraData = new MinionExtraData(health, attackDamage);
                        break;
                }
            }
            catch (Exception) 
            {
            }

            return extraData;
        }
    }
}
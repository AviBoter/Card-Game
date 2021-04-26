using Database.API.Card;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using GenericCardFields;
using Database.API.Card.Extra;
using UnityEditor;

public class JsonWriter : IFileWriter
{
    public void WriteToFile(string path , CardData[] Cards)
    {

        try
        {
            TextWriter myStreamWriter = new StreamWriter(path);
            string json = "[";
            JObject jsonObj = new JObject();
            foreach (CardData card in Cards)
            {

                jsonObj[MyConsts.name] = card.CardName;
                jsonObj[MyConsts.cost] = card.Cost;
                jsonObj[MyConsts.art_path] = AssetDatabase.GetAssetPath(card.Art);
                jsonObj[MyConsts.extra_data_type_name] = card.ExtraDataTypeName;
                jsonObj[MyConsts.extra_data] = jsonExtraData(jsonObj, card);
                json = json + ", \n" + jsonObj.ToString();
               
            }
            json = json + "\n ]";
            myStreamWriter.Write(json);
        }
        catch (Exception) { }
    }

    private JArray jsonExtraData(JObject jsonObj, CardData card)
    {
        JObject obj1 = new JObject();
        JObject obj2 = new JObject();

        switch (card.ExtraDataTypeName.ToString())
        {
            case MyConsts.spell_extra_data:
                SpellExtraData spell = (SpellExtraData)card.ExtraData;
                obj1[MyConsts.effect] = spell.Effect.ToString();
                obj2[MyConsts.effect_amount] = spell.EffectAmount.ToString();
                break;

            case MyConsts.minion_extra_data:
                MinionExtraData minion = (MinionExtraData)card.ExtraData;
                obj1[MyConsts.effect] = minion.Health.ToString();
                obj2[MyConsts.effect_amount] = minion.AttackDamage;
                break;
        }
        JArray array = new JArray(obj1, obj2);
        return array;
    }
}

using Database.API.Card;
using System;
using System.Collections.Generic;
using GenericCardFields;
using Database.API.Card.Extra;
using UnityEditor;
using System.Runtime.Serialization;

[DataContract]
public class XmlWriter : IFileWriter
{
    public void WriteToFile(string path, CardData[] Cards)
    {

        var ser = new DataContractSerializer(typeof(CardData));
        var obj = new XmlWriter(Guid.NewGuid());
        using (System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(path))
        {
            ser.WriteObject(xw, obj);
            foreach (CardData card in Cards)
        {
            writeToXml(card, path , writer);
        }
            writer.WriteEndElement();
        }

    }
    private void writeToXml(CardData card,string Path, System.Xml.XmlWriter writer)
    {
            writer.WriteStartElement("element");
            writer.WriteElementString(MyConsts.art_path, AssetDatabase.GetAssetPath(card.Art));
            writer.WriteElementString(MyConsts.cost, card.Cost.ToString());
            writerExtraData(writer, card);
            writer.WriteElementString(MyConsts.extra_data_type_name, card.ExtraDataTypeName.ToString());
            writer.WriteElementString(MyConsts.name, card.CardName.ToString());
            writer.WriteEndElement();
            writer.Flush();
    }
    
    private void writerExtraData(System.Xml.XmlWriter writer, CardData card)
    {
        writer.WriteStartElement(MyConsts.extra_data);

        switch (card.ExtraDataTypeName.ToString())
        {
            case MyConsts.spell_extra_data:
                SpellExtraData spell = (SpellExtraData)card.ExtraData;
                writer.WriteElementString(MyConsts.effect.ToString(), spell.Effect.ToString());
                writer.WriteElementString(MyConsts.effect_amount.ToString(), spell.EffectAmount.ToString());
                break;

            case MyConsts.minion_extra_data:
                MinionExtraData minion = (MinionExtraData)card.ExtraData;
                writer.WriteElementString(MyConsts.effect.ToString(), minion.Health.ToString());
                writer.WriteElementString(MyConsts.effect_amount.ToString(), minion.AttackDamage.ToString());
                break;
        }
        writer.WriteEndElement();
    }
}

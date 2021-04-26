using Database.API.Card;
using Database.API.Card.Extra;
using GenericCardFields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;


namespace readers
{
    public class MyXmlReader : IFileReader
    {
        private readonly XmlDocument xmlDoc = new XmlDocument();

        public Dictionary<string, CardData> ReadFromFile(string path)
        {
            var strXMLdoc = File.ReadAllText(path);
            var Cards = new Dictionary<string, CardData>();
            using (var reader = new StringReader(strXMLdoc))
            {
                xmlDoc.Load(reader);
            }

            BuildNewCards(xmlDoc.DocumentElement.ChildNodes, Cards);

            return Cards;
        }

        private void BuildNewCards(XmlNodeList xmlList, Dictionary<string, CardData> Cards)
        {
            foreach (XmlNode childNode in xmlList)
            {
                var cost = int.Parse(childNode[MyConsts.cost].InnerText);
                var name = childNode[MyConsts.name].InnerText;
                var artPath = childNode[MyConsts.art_path].InnerText;
                var extraDataTypeName = childNode[MyConsts.extra_data_type_name].InnerText;
                var extraData = assign_extradata(extraDataTypeName, childNode[MyConsts.extra_data]);
                var card = CardData.BuildNewAsset(name, cost, artPath, extraData);
                Cards.Add(name, card);

            }
        }

        private ExtraData assign_extradata(string extraDataTypeName, XmlNode childNode)
        {
            ExtraData extraData = null;
            try
            {
                switch (extraDataTypeName)
                {
                    case MyConsts.spell_extra_data:
                        var effect = (SpellEffect)int.Parse(childNode[MyConsts.effect].InnerText);
                        var effectAmount = int.Parse(childNode[MyConsts.effect_amount].InnerText);
                        extraData = new SpellExtraData(effect, effectAmount);
                        break;

                    case MyConsts.minion_extra_data:
                        var health = int.Parse(childNode[MyConsts.health].InnerText);
                        var attackDamage = int.Parse(childNode[MyConsts.attack_damage].InnerText);
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Assets.Scripts.Data
{
    public class PlayerDataConverter : JsonConverter<PlayerData>
    {
        public override void WriteJson(JsonWriter writer, PlayerData value, JsonSerializer serializer)
        {
            // Start writing the object
            writer.WriteStartObject();

            // Serialize all other fields normally
            writer.WritePropertyName("Id");
            writer.WriteValue(value.Id);

            writer.WritePropertyName("PlayerName");
            writer.WriteValue(value.PlayerName);

            writer.WritePropertyName("Level");
            writer.WriteValue(value.Level);

            writer.WritePropertyName("Experience");
            writer.WriteValue(value.Experience);

            writer.WritePropertyName("Mana");
            writer.WriteValue(value.Mana);

            writer.WritePropertyName("Skills");
            serializer.Serialize(writer, value.Skills);

            // Custom serialization for the Inventory dictionary
            writer.WritePropertyName("Inventory");
            writer.WriteStartArray();
            foreach (var item in value.Inventory)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Key");
                writer.WriteValue($"{item.Key.Id}_{item.Key.Index}");
                writer.WritePropertyName("Value");
                writer.WriteValue(item.Value);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            // End writing the object
            writer.WriteEndObject();
        }

        public override PlayerData ReadJson(JsonReader reader, Type objectType, PlayerData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            PlayerData playerData = new PlayerData("Default");

            JObject obj = JObject.Load(reader);

            playerData.Id = obj["Id"].ToObject<Guid>();
            playerData.PlayerName = obj["PlayerName"].ToString();
            playerData.Level = obj["Level"].ToObject<int>();
            playerData.Experience = obj["Experience"].ToObject<float>();
            playerData.Mana = obj["Mana"].ToObject<float>();
            playerData.Skills = obj["Skills"].ToObject<PlayerSkills>();

            // Deserialize the Inventory dictionary
            playerData.Inventory.Clear();
            JArray inventoryArray = (JArray)obj["Inventory"];
            foreach (JObject item in inventoryArray)
            {
                string[] keyData = item["Key"].ToString().Split('_');
                InventoryItemData key = new InventoryItemData(keyData[0], int.Parse(keyData[1]));
                int value = item["Value"].ToObject<int>();
                playerData.Inventory.Add(key, value);
            }

            return playerData;
        }
    }


}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AttnKare.Data
{
    [CreateAssetMenu(fileName = "New Game Data Set", menuName = "ScriptableObjects/Game Data")]
    public class GameData : ScriptableObject
    {
        public List<GameDataField> gameDataList;

        public int FindDataIndex(string dataName)
        {
            foreach (GameDataField dataField in gameDataList)
            {
                if (dataName.Equals(dataField.dataName))
                    return gameDataList.IndexOf(dataField);
            }

            return -1;
        }
        
        public void SerializeToJson()
        {
            string json = "";

            foreach (GameDataField data in gameDataList)
            {
                json += $"\t\"{data.dataName}\": ";
                switch (data.dataType)
                {
                    case DataType.INT:
                        json += data.intValue + ",";
                        break;
                    case DataType.FLOAT:
                        json += data.floatValue + ",";
                        break;
                    case DataType.BOOL:
                        json += (data.boolValue ? "true" : "false") + ",";
                        break;
                    case DataType.STRING:
                        json += $"\"{data.stringValue}\",";
                        break;
                }
                json += "\n";
            }

            if (json.Length >= 2)
                json = json.Substring(0, json.Length - 2) + "\n";

            json = "{\n" + json + "}";
            string filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
            File.WriteAllText(filePath, json);
            
            Debug.Log("JSON file created at: " + filePath);
        }
    }
}

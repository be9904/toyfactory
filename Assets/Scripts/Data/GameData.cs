using System;
using System.Collections.Generic;
using UnityEngine;

namespace AttnKare.Data
{
    [CreateAssetMenu(fileName = "New Game Data Set", menuName = "ScriptableObjects/Game Data")]
    public class GameData : ScriptableObject
    {
        public List<GameDataField> gameDataList;
        
        public string SerializeToJson()
        {
            Dictionary<string, object> gameData = new Dictionary<string, object>();

            foreach (GameDataField data in gameDataList)
            {
                switch (data.dataType)
                {
                    case DataType.INT:
                        gameData[data.dataName] = data.intValue;
                        break;
                    case DataType.FLOAT:
                        gameData[data.dataName] = data.floatValue;
                        break;
                    case DataType.BOOL:
                        gameData[data.dataName] = data.boolValue;
                        break;
                    case DataType.STRING:
                        gameData[data.dataName] = data.stringValue;
                        break;
                }
            }

            string json = JsonUtility.ToJson(gameData);
            
            Debug.Log(json);

            return json;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using AttnKare.Data;
using AttnKare.Interactables;

namespace AttnKare
{
    [RequireComponent(typeof(BehaviorData))]
    public class DataManager : MonoBehaviour
    {
        public static DataManager main { get; private set; }
        
        private void Awake()
        {
            // singleton initialization
            if (main != null && main != this)
            {
                Debug.Log(
                    "Singleton instance of " + GetType().Name + " already exists. " + 
                    "Destroying instance " + gameObject.GetInstanceID() + "."
                );
                Destroy(this);
            }
            else
            {
                main = this;
                behaviorData = GetComponent<BehaviorData>();
            }
        }
        
        public GameData gameData;
        public Dictionary<string, object> dataList = new Dictionary<string, object>();

        private BehaviorData behaviorData;
        
        // Start is called before the first frame update
        void Start()
        {
            foreach (GameDataField dataField in gameData.gameDataList)
            {
                dataField.ResetAll();
                dataList.Add(dataField.dataName, dataField.value);
            }
        }

        public void IncrementButtonCount(Button.ButtonType buttonType)
        {
            if(buttonType == Button.ButtonType.Paint)
                dataList["paintButtonCount"] = (int)dataList["paintButtonCount"] + 1;
            else if(buttonType == Button.ButtonType.Leave)
                dataList["leaveButtonCount"] = (int)dataList["leaveButtonCount"] + 1;
        }

        public void SaveData()
        {
            int idx;
            foreach (KeyValuePair<string, object> kvp in dataList)
            {
                idx = gameData.FindDataIndex(kvp.Key);
                gameData.gameDataList[idx].SetValue(kvp.Value);
            }
        }

        private void OnApplicationQuit()
        {
            SaveData();
            behaviorData.SaveBehaviorData();
            gameData.SerializeToJson();
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using AttnKare.Data;
using AttnKare.Interactables;

namespace AttnKare
{
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
                main = this;
        }
        
        public GameData gameData;
        public Dictionary<string, object> dataList = new Dictionary<string, object>();
        
        // Debug
        public int field0;
        public int field1;
        public int field2;
        public int field3;
        public int field4;
        public int field5;
        public int field6;
        public bool field7;
        
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

        // Update is called once per frame
        void Update()
        {
            ReadData();
        }

        void ReadData()
        {
            field0 = (int)dataList["leverUpCount"];
            field1 = (int)dataList["leverDownCount"];
            field2 = (int)dataList["paintButtonCount"];
            field3 = (int)dataList["leaveButtonCount"];
            field4 = (int)dataList["colorWheelGrabCount"];
            field5 = (int)dataList["correctColorRobotCount"];
            field6 = (int)dataList["wrongColorRobotCount"];
            field7 = (bool)dataList["gameQuit"];
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
            gameData.SerializeToJson();
        }
    }
}

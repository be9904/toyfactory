using System;
using UnityEngine;

namespace AttnKare.Data
{
    [Serializable]
    public class GameDataField
    {
        // data properties
        public DataType dataType;
        public string dataName;

        // data values
        public int intValue;
        public float floatValue;
        public bool boolValue;
        public string stringValue;

        public object value
        {
            get
            {
                switch (dataType)
                {
                    case DataType.INT:      return intValue;
                    case DataType.FLOAT:    return floatValue;
                    case DataType.BOOL:     return boolValue;
                    case DataType.STRING:   return stringValue;
                    default:                return intValue;
                }
            }
        }

        public void ResetAll()
        {
            intValue = 0;
            floatValue = 0f;
            boolValue = false;
            stringValue = "";
        }
    }
    
    public enum DataType{ INT, FLOAT, BOOL, STRING}
}
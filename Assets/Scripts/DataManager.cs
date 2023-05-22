using UnityEngine;
using AttnKare.Data;

namespace AttnKare
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField] private GameData gameData;
        
        // Start is called before the first frame update
        void Start()
        {
            foreach (GameDataField dataField in gameData.gameDataList)
            {
                dataField.ResetAll();
            }

            gameData.SerializeToJson();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

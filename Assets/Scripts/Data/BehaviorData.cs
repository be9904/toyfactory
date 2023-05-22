using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BNG;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

/// <summary>
/// 시작과 끝부분마다 GetComponent<BNG.CollectData>().AddTimeStamp("delimiter name");를 호출하면 됩니다
/// </summary>

namespace AttnKare.Data
{
    public class BehaviorData : MonoBehaviour
    {
        // Add Objects in Inspector
        [Header("Objects in Game")]
        [SerializeField] [Tooltip("XR Rig or XR Rig Advanced goes here")] Transform XRRig;
        [SerializeField] [Tooltip("CenterEyeAnchor goes here")] Transform Camera;
        [SerializeField] [Tooltip("LeftControllerAnchor goes here")] Transform LHand;
        [SerializeField] [Tooltip("RightControllerAnchor goes here")] Transform RHand;
        private Stats database; // All data is stored in this object
        private InputBridge _inputBridge; // XR Rig Input Bridge (C# Script)

        // Fields needed for Trigger Input (Do Not Remove)
        float RTriggerState = 0;
        float LTriggerState = 0;

        // For Checking in Inspector 
        [Header("Controller Button Debug Panel")]
        public int _LTriggerClicks;
        public float _LTrigger;
        public int _RTriggerClicks;
        public float _RTrigger;
        public int _LGripClicks;
        public float _LGrip;
        public int _RGripClicks;
        public float _RGrip;
        public int _AClicks;
        public int _BClicks;
        public int _XClicks;
        public int _YClicks;

        public Vector3 currentVelocity;

        // File Writing
        FileStream InputDataInfo;
        StreamWriter InputDataWriter;

        string FileName;
        string FolderName;
        string FilePath_Root;
        string FilePath_Folder;

        // Data per Frame
        List<string> dataPerFrame = new List<string>();

        // Velocity
        Vector3 prevPosHead;
        Vector3 curPosHead;
        Vector3 prevPosRHand;
        Vector3 curPosRHand;
        Vector3 prevPosLHand;
        Vector3 curPosLHand;

        float velocityXHead;
        float velocityYHead;
        float velocityZHead;
        float velocityXRHand;
        float velocityYRHand;
        float velocityZRHand;
        float velocityXLHand;
        float velocityYLHand;
        float velocityZLHand;

        // Path
        private string csvPath;
        private string txtPath;

        float Timer = 0;
        FileStream DeviceDataInfo;
        StreamWriter DeviceDataWriter;
        int frame = 0;
        public float dt; // Time Interval

        // Object Class for storing data
        class Stats
        {
            // Head Transform
            public Vector3 HeadPosition;    // 3D Coordinates (Contains x, y, z)
            public Vector3 HeadAngle;   // Euler Angles (Contains x, y, z

            // Controller Transform
            public Vector3 LHandPosition;
            public Vector3 RHandPosition;
            public string controllerTransform;

            // Controller Input
            public int LTriggerClicks = 0;
            public int RTriggerClicks = 0;
            public int LGripClicks = 0;
            public int RGripClicks = 0;
            public int AClicks = 0;
            public int BClicks = 0;
            public int XClicks = 0;
            public int YClicks = 0;
            public string controllerInput;

            // Final Output
            public string output;
        }

        // Start is called before the first frame update
        void Start()
        {
            database = new Stats();
            
            FileName = SceneManager.GetActiveScene().buildIndex.ToString();
#if UNITY_EDITOR
            FileName = EditorSceneManager.GetActiveScene().buildIndex.ToString();
#endif

            // 아래 코드를 막은 이유는 DataManager의 함수와 중복되는 내용
            // FolderName = "NAME" + DateTime.Now.ToString("yyyyMMddHHdd");                                          // UserData.DataManager.GetInstance().userInfo.Name + "_" + UserData.DataManager.GetInstance().userInfo.Gender;
            // FilePath_Root = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";       //기본 날짜 묶음 C:\Users\uk308\AppData\LocalLow\HippoTnC\Strengthen_Concentration_VR
            // FilePath_Folder = FilePath_Root + FolderName + "/";

            //if (!Directory.Exists(FilePath_Root))
            //{
            //    Directory.CreateDirectory(FilePath_Root);
            //}

            //if (!Directory.Exists(FilePath_Folder))
            //{
            //    Directory.CreateDirectory(FilePath_Folder);
            //}
            
            csvPath = Path.Combine(Application.persistentDataPath, "behaviordata.csv");
            txtPath = Path.Combine(Application.persistentDataPath, "controllerinput.txt");

            ShowDataOnInspector();
            /*dataPerFrame.Add("Time, A, B, X, Y, LTr, RTr, LGr, RGr, HPosX, HPosY, HPosZ, HAngX, HAngY, HAngZ, LHPosX, LHPosY, LHPosZ, RHPosX, RHPosY, RHPosZ");*/
            prevPosHead = database.HeadPosition;
            prevPosLHand = database.LHandPosition;
            prevPosRHand = database.RHandPosition;
        }

        void FixedUpdate()
        {
            Timer += Time.deltaTime;
            dt += Time.deltaTime;
            frame++;

            if(frame > 3)
            {
                SaveDeviceData();
                frame = 0;
                dt = 0;
            }

            ShowDataOnInspector();
        }

        // 30
        // Time, A, B, X, Y, LTr, RTr, LGr, RGr, HPosX, HPosY, HPosZ, HAngX, HAngY, HAngZ, LHPosX, LHPosY, LHPosZ, RHPosX, RHPosY, RHPosZ, HeadVelX, HeadVelY, HeadVelZ, LHandVelX, LHandVelY, LHandVelZ, RHandVelX, RHandVelY, RHandVelZ
        public void SaveBehaviorData()     //<<< ------------------------------- 각자 종료할때 호출해서 저장
        {
            database.controllerInput = "Left Trigger Clicks: " + database.LTriggerClicks.ToString() + "\nRight Trigger Clicks: " + database.RTriggerClicks.ToString()
                + "\nLeft Grip Clicks: " + database.LGripClicks.ToString() + "\nRight Grip Clicks: " + database.RGripClicks.ToString()
                + "\nA Button Pressed: " + database.AClicks.ToString() + "\nB Button Pressed: " + database.BClicks.ToString()
                + "\nX Button Pressed: " + database.XClicks.ToString() + "\nY Button Pressed: " + database.YClicks.ToString();

            // Add Number of Clicks to Data List
            File.WriteAllText(txtPath, database.controllerInput);

            DeviceDataInfo = new FileStream(csvPath, FileMode.Append, FileAccess.Write);
            DeviceDataWriter = new StreamWriter(DeviceDataInfo, System.Text.Encoding.Unicode);
            foreach (string data in dataPerFrame)
            {
                DeviceDataWriter.WriteLine(data);
            }
            DeviceDataWriter.Close();

            // Delete if unnecessary
            // SaveInputData(database.controllerInput);
        }

        void ShowDataOnInspector()
        {
            _inputBridge = XRRig.GetComponent<InputBridge>();

            // Saves Camera Rig Position & EulerAngles, Left Controller Position and Right Controller Position
            database.HeadPosition = Camera.localPosition;
            database.HeadAngle = Camera.eulerAngles;
            database.LHandPosition = LHand.localPosition;
            database.RHandPosition = RHand.localPosition;

            // Trigger Button
            if (RTriggerState < _inputBridge.RightTrigger && _inputBridge.RightTrigger == 1)
            {
                database.RTriggerClicks++;
            }
            if (LTriggerState < _inputBridge.LeftTrigger && _inputBridge.LeftTrigger == 1)
            {
                database.LTriggerClicks++;
            }
            RTriggerState = _inputBridge.RightTrigger; // Save Right Trigger State in current frame
            LTriggerState = _inputBridge.LeftTrigger; // Save Left Trigger State in current frame
            
            // Grip Button
            if (_inputBridge.RightGripDown)
            {
                database.RGripClicks++;
            }
            if (_inputBridge.LeftGripDown)
            {
                database.LGripClicks++;
            }
            // Right Controller
            if (_inputBridge.AButtonDown)
            {
                database.AClicks++;
            }
            if (_inputBridge.BButtonDown)
            {
                database.BClicks++;
            }
            // Left Controller
            if (_inputBridge.XButtonDown)
            {
                database.XClicks++;
            }
            if (_inputBridge.YButtonDown)
            {
                database.YClicks++;
            }

            // Update Debug Panel per frame
            _LTriggerClicks = database.LTriggerClicks;
            _LTrigger = _inputBridge.LeftTrigger;
            _RTriggerClicks = database.RTriggerClicks;
            _RTrigger = _inputBridge.RightTrigger;
            _LGripClicks = database.LGripClicks;
            _LGrip = _inputBridge.LeftGrip;
            _RGripClicks = database.RGripClicks;
            _RGrip = _inputBridge.RightGrip;
            _AClicks = database.AClicks;
            _BClicks = database.BClicks;
            _XClicks = database.XClicks;
            _YClicks = database.YClicks;
        }

        void SaveDeviceData()
        {
            //                    Time            Controller Buttons    Device Position      Device Velocity
            dataPerFrame.Add(Timer.ToString() + ", " + Buttons() + ", " + Positions() + ", " + Velocity());
        }

        public string Velocity()
        {
            // Return Value
            string result = "";

            // Update Current Position
            curPosHead = database.HeadPosition;
            curPosLHand = database.LHandPosition;
            curPosRHand = database.RHandPosition;

            // Get xyz components of velocity vector
            // Head
            velocityXHead = (curPosHead.x - prevPosHead.x) / dt;
            velocityYHead = (curPosHead.y - prevPosHead.y) / dt;
            velocityZHead = (curPosHead.z - prevPosHead.z) / dt;
            // Left Hand
            velocityXLHand = (curPosLHand.x - prevPosLHand.x) / dt;
            velocityYLHand = (curPosLHand.y - prevPosLHand.y) / dt;
            velocityZLHand = (curPosLHand.z - prevPosLHand.z) / dt;
            // Right Hand
            velocityXRHand = (curPosRHand.x - prevPosRHand.x) / dt;
            velocityYRHand = (curPosRHand.y - prevPosRHand.y) / dt;
            velocityZRHand = (curPosRHand.z - prevPosRHand.z) / dt;
            
            // Save Current State
            prevPosHead = curPosHead;
            prevPosLHand = curPosLHand;
            prevPosRHand = curPosRHand;

            // Save to Return Value
            result += velocityXHead.ToString() + ", " + velocityYHead.ToString() + ", " + velocityZHead.ToString() + ", ";
            result += velocityXLHand.ToString() + ", " + velocityYLHand.ToString() + ", " + velocityZLHand.ToString() + ", ";
            result += velocityXRHand.ToString() + ", " + velocityYRHand.ToString() + ", " + velocityZRHand.ToString();

            // Return Velocity
            return result;
        }

        public string Positions()
        {
            return database.HeadPosition.x.ToString() + ", " + database.HeadPosition.y.ToString() + ", " + database.HeadPosition.z.ToString() +
                ", " + database.HeadAngle.x.ToString() + ", " + database.HeadAngle.y.ToString() + ", " + database.HeadAngle.z.ToString() +
                ", " + database.LHandPosition.x.ToString() + ", " + database.LHandPosition.y.ToString() + ", " + database.LHandPosition.z.ToString() +
                ", " + database.RHandPosition.x.ToString() + ", " + database.RHandPosition.y.ToString() + ", " + database.RHandPosition.z.ToString();
        }

        public string Buttons()
        {
            return (_inputBridge.AButtonDown || _inputBridge.AButton || _inputBridge.AButtonUp ? 1 : 0).ToString() +
                ", " + (_inputBridge.BButtonDown || _inputBridge.BButton || _inputBridge.BButtonUp ? 1 : 0).ToString() +
                ", " + (_inputBridge.XButtonDown || _inputBridge.XButton || _inputBridge.XButtonUp ? 1 : 0).ToString() +
                ", " + (_inputBridge.YButtonDown || _inputBridge.YButton || _inputBridge.YButtonUp ? 1 : 0).ToString() +
                ", " + _LTrigger.ToString() + ", " + _RTrigger.ToString() + ", " + _LGrip.ToString() + ", " + _RGrip.ToString();
        }
    }   
}
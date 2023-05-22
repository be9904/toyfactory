using UnityEngine;

namespace AttnKare
{
    public class Robot : Spawnable
    {
        private GameManager.RobotColor robotColor = GameManager.RobotColor.NONE;

        public GameManager.RobotColor Color => robotColor;
        [SerializeField] private Material defaultMaterial;

        private float paintProgress;

        public float PaintProgress
        {
            get => paintProgress;
            set => paintProgress = value;
        } 

        private bool _isPainted;
        public bool IsPainted
        {
            get => _isPainted;
            set => _isPainted = value;
        }

        private void OnEnable()
        {
            GameManager.RobotDestroyEvent += ResetRobot;
        }
        
        private void OnDisable()
        {
            GameManager.RobotDestroyEvent -= ResetRobot;
        }

        public void PaintRobot(float paintSpeed)
        {
            if(paintProgress < 100)
                paintProgress += Time.deltaTime * paintSpeed;
        }

        public void SetColor(GameManager.RobotColor color)
        {
            robotColor = color;
        }

        void ResetRobot(GameObject obj)
        {
            _isPainted = false;
            paintProgress = 0f;
            gameObject.GetComponent<Renderer>().material = defaultMaterial;
        }
    }
}

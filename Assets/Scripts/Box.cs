using UnityEngine;
using Random = UnityEngine.Random;

namespace AttnKare
{
    public class Box : Spawnable
    {
        private bool _isProperBox = true;

        public bool IsProperBox
        {
            get => _isProperBox;
            set => _isProperBox = value;
        }

        // Start is called before the first frame update
        void Start()
        {
            GenerateRandomState();
        }

        void GenerateRandomState()
        {
            int rnd = Random.Range(0, 10);
            if (rnd % 4 == 0)
            {
                IsProperBox = false;
            }
        }
    }
}

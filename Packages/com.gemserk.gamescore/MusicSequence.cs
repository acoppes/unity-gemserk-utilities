using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MusicSequence : MonoBehaviour
    {
        public List<AudioSource> musicList;

        private int current;

        private void Start()
        {
            if (current < musicList.Count)
            {
                musicList[current].Play();
            }
        }

        private void Update()
        {
            if (musicList.Count == 0)
                return;

            if (!musicList[current].isPlaying)
            {
                current++;
                if (current >= musicList.Count)
                {
                    current = 0;
                }
                
                musicList[current].Play();
            }
            
        }
    }
}
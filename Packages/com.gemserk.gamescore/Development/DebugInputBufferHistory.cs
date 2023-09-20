using Game.Components;
using Game.DebugTools;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using UnityEngine;

namespace Game.Development
{
    public class DebugInputBufferHistory : MonoBehaviour
    {
        public Query playerQuery;

        [SerializeField]
        private int maxHistory = 10;
        
        [SerializeField]
        private GameObject debugBufferPrefab;

        private DebugBuffer _debugBuffer;

        private World _world;

        public void Start()
        {
            _world = World.Default;
        }
        private void Update()
        {
            // var entity = _world.GetEntityByName(playerName);

            var playerEntity = _world.GetFirstOrDefault(playerQuery.GetEntityQuery());
            
            if (playerEntity == Entity.NullEntity)
            {
                return;
            }

            var bufferedInput = _world.GetComponent<BufferedInputComponent>(playerEntity);

            if (bufferedInput.buffer.Count == 0)
            {
                if (_debugBuffer != null)
                {
                    _debugBuffer.ConvertToHistory();
                }
                _debugBuffer = null;
                return;
            }

            if (_debugBuffer == null)
            {
                var debugBufferInstance = GameObject.Instantiate(debugBufferPrefab, transform);
                debugBufferInstance.SetActive(true);
                _debugBuffer = debugBufferInstance.GetComponent<DebugBuffer>();
            }
            
            _debugBuffer.UpdateBuffer(bufferedInput);
            
            if (transform.childCount > maxHistory)
            {
                var firstChild = transform.GetChild(0);
                GameObject.DestroyImmediate(firstChild.gameObject);
            }
        }
    }
}

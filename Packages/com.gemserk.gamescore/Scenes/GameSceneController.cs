using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Game.Scenes
{
    public class GameSceneController : MonoBehaviour
    {
        public static int players = 1;

        public float spawnDistanceToCenter = 4f;
        public GameObject playerCharacterDefinition;
        
        // Start is called before the first frame update
        void Start()
        {
            var world = World.Default;

            var spawnAngle = UnityEngine.Random.Range(0, 360);
            var divAngle = 360 / players;
            
            for (var i = 0; i < players; i++)
            {
                var playerCharacterEntity = world.CreateEntity(playerCharacterDefinition);
                
                world.AddComponent(playerCharacterEntity, new PlayerInputComponent()
                {
                    playerInput = i,
                });
                
                world.AddComponent(playerCharacterEntity, new NameComponent
                {
                    name = $"Character_Player_{i}",
                    singleton = true
                });

                var position = Vector2.right.Rotate(spawnAngle * Mathf.Deg2Rad) * spawnDistanceToCenter;
                spawnAngle += divAngle;

                ref var positionComponent = ref world.GetComponent<PositionComponent>(playerCharacterEntity);
                positionComponent.value = new Vector3(position.x, 0, position.y);

                // set in position, configure controllable, etc
            }
        }
    }
}

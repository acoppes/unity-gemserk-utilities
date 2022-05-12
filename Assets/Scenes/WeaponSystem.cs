using Leopotam.EcsLite;
using UnityEngine;

public class WeaponSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IFixedUpdateSystem
{
    public void Init(EcsSystems systems)
    {
        
    }

    public void Run(EcsSystems systems)
    {
        Debug.Log("Fixed update!");
    }
}
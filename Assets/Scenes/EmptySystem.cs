using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

public class EmptySystem : BaseSystem, IEcsRunSystem, IUpdateSystem
{
    public void Run(EcsSystems systems)
    {
    }
}
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

public class EmptySystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
{
    public void Run(EcsSystems systems)
    {
    }
}
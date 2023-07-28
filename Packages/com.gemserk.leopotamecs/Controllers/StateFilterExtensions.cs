using System.Linq;
using Gemserk.Leopotam.Ecs.Components;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public static class StateFilterExtensions
    {

        public static bool Match(this StateFilter filter, StatesComponent stateComponent)
        {
            return filter.Match(stateComponent.states.Keys.ToList());
        }
    }
}
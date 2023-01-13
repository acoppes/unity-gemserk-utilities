using System.Linq;

namespace Gemserk.Leopotam.Gameplay.Controllers
{
    public static class StateFilterExtensions
    {

        public static bool Match(this StateFilter filter, StatesComponent stateComponent)
        {
            return filter.Match(stateComponent.states.Keys.ToList());
        }
    }
}
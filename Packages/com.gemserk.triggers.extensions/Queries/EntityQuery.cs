using System.Collections.Generic;
using System.Linq;

namespace Gemserk.Triggers.Queries
{
    public readonly struct EntityQuery
    {
        public readonly List<IEntityMatcher> parameters;

        public EntityQuery(List<IEntityMatcher> parameters)
        {
            this.parameters = parameters;
        }
        
        public EntityQuery(params IEntityMatcher[] parameters)
        {
            this.parameters = parameters.ToList();
        }
        
        public override string ToString()
        {
            if (parameters == null)
            {
                return string.Empty;
            }
            
            var list = new List<string>();

            foreach (var parameter in parameters)
            {
                if (parameter != null)
                {
                    list.Add(parameter.ToString());
                }
            }

            return string.Join(",", list);
        }
        
        public static EntityQuery Create(IEntityMatcher[] parameters)
        {
            return new EntityQuery(parameters);
        }
    }
}
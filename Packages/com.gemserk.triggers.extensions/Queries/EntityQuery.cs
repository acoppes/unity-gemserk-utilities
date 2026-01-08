using System.Collections.Generic;
using System.Linq;

namespace Gemserk.Triggers.Queries
{
    public struct EntityQuery
    {
        public List<IQueryParameter> parameters;

        public EntityQuery(List<IQueryParameter> parameters)
        {
            this.parameters = parameters;
        }
        
        public EntityQuery(params IQueryParameter[] parameters)
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
        
        public static EntityQuery Create(IQueryParameter[] parameters)
        {
            return new EntityQuery(parameters);
        }
    }
}
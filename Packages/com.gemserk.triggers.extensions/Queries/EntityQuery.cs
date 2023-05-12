using System.Collections.Generic;

namespace Gemserk.Triggers.Queries
{
    public struct EntityQuery
    {
        public IEnumerable<IQueryParameter> parameters;

        public EntityQuery(IEnumerable<IQueryParameter> parameters)
        {
            this.parameters = parameters;
        }
        
        public EntityQuery(params IQueryParameter[] parameters)
        {
            this.parameters = parameters;
        }
        
        public override string ToString()
        {
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

        public static EntityQuery Create(ICollection<IQueryParameter> parameters)
        {
            return new EntityQuery(parameters);
        }
    }
}
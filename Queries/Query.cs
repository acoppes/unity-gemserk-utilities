using System.Collections.Generic;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers.Queries
{
    public class Query : AutoNamedObject
    {
        [SerializeReference]
        [SubclassSelector]
        public List<IQueryParameter> parameters = new();

        public EntityQuery GetEntityQuery() => EntityQuery.Create(parameters);

        public override string GetObjectName()
        {
            return $"Q({GetEntityQuery()})";
        }
    }
}
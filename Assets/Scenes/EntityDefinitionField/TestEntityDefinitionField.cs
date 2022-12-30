using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;


public class TestEntityDefinitionField : MonoBehaviour
{
    [EntityDefinition]
    public Object objectDefinition;

    [EntityDefinition]
    public GameObject gameObjectDefinition;
    
    public Object definitionNoCheck1;

    public int test;

    [TypeValidation(typeof(IEntityDefinition))]
    public Object objectDefinition2;

    [TypeValidation(typeof(IEntityDefinition))]
    public GameObject gameObjectDefinition2;
    
    public Object definitionNoCheck2;
}
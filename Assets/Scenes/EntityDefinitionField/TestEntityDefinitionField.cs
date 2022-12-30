using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class TestEntityDefinitionField : MonoBehaviour
{
    [EntityDefinition]
    public Object objectDefinition;

    [EntityDefinition]
    public GameObject gameObjectDefinition;
    
    public Object definitionNoCheck1;
    
    
}
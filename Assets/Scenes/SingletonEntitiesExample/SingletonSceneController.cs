using System.Collections;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

public class SingletonSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var emptyEntity = World.Instance.CreateEntity();
        World.Instance.AddComponent<NameComponent>(emptyEntity, new NameComponent()
        {
            singleton = true,
            name = "UniqueName1"
        });

        StartCoroutine(CheckForUniqueEntity());
    }

    IEnumerator CheckForUniqueEntity()
    {
        yield return null;
        var entity = World.Instance.GetEntityByName("UniqueName1");
        Assert.IsFalse(entity == Entity.NullEntity);
    }
}

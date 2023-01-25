using System;
using Gemserk.Utilities;
using UnityEngine;

[Serializable]
public class TestOptionalFields
{
    public bool checkAliveType;
    public int aliveType;
}

public class OptionalFieldsExample : MonoBehaviour
{
    [OptionalFields]
    public TestOptionalFields testOptionalFields = new TestOptionalFields();
}

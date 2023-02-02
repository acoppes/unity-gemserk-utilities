using System.Collections;
using System.Collections.Generic;
using Gemserk.BuildTools;
using UnityEngine;

public class ObjectWithSelectFolderProp : MonoBehaviour
{
    [SelectFile(true)]
    public string folder;
    
    [SelectFile(true)]
    public string file;
}

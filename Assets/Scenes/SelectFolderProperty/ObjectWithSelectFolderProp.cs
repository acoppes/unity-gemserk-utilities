using Gemserk.Utilities;
using UnityEngine;

public class ObjectWithSelectFolderProp : MonoBehaviour
{
    [FolderPath()]
    public string folder;
    
    [FilePath()]
    public string file;
}

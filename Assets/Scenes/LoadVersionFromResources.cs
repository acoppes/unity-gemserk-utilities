using UnityEngine;
using UnityEngine.UI;

public class LoadVersionFromResources : MonoBehaviour
{
    public Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        var versionTextAsset = Resources.Load<TextAsset>("version");
        if (versionTextAsset != null)
        {
            text.text = versionTextAsset.text;
        }
    }
}

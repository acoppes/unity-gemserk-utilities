using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class TestCompareSelectedFiles
{
    [MenuItem("Gemserk/Compare Selected Files")]
    public static void CompareSelectedFiles()
    {
        var paths = Selection.objects.Where(obj => AssetDatabase.GetAssetPath(obj) != null)
            .Select(AssetDatabase.GetAssetPath).ToList();

        var md5 = MD5.Create();

        foreach (var path in paths)
        {
            // var filePath = Path.Combine(Application.dataPath, path);
            var stream = File.OpenRead(path);
            var hash = md5.ComputeHash(stream);
            Debug.Log(Convert.ToBase64String(hash));
            // Debug.Log(Encoding.Default.GetString(hash));
            stream.Close();
        }
    }    
}
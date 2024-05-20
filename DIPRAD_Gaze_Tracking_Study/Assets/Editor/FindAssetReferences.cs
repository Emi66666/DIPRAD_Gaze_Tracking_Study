using UnityEditor;
using UnityEngine;

public class FindAssetReferences : MonoBehaviour
{
    [MenuItem("Tools/Find References to sv_icon_dot10_pix16_gizmo")]
    static void FindReferences()
    {
        string[] guids = AssetDatabase.FindAssets("sv_icon_dot10_pix16_gizmo");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log("Asset found at: " + path);
        }
    }
}

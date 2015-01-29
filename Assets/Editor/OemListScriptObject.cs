using UnityEngine;
using System.Collections;
using UnityEditor;
//Unity 3D : 腳本化物件 ScriptableObject 設置資料成為 Asset
public class OemListScriptObject : ScriptableObject
{
	public string[] OemListArray;
	public string[] OemCodeArray;
	public int[] OemVersionCodeArray;

	[MenuItem("Editor/BuildAsset/create OemListAsset")]
	public static void BuildArrayAsset()
	{
		OemListScriptObject asset = ScriptableObject.CreateInstance<OemListScriptObject>();
		AssetDatabase.CreateAsset(asset, "Assets/Settings/OemList.asset");
		AssetDatabase.SaveAssets();
		EditorUtility.DisplayDialog("create OemList Asset", "OemList.asset", "OK", "");	
	}
}
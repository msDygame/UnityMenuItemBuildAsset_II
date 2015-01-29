using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;//for Directory

//Unity 3D : 腳本化物件 ScriptableObject 設置資料成為 AssetBundle 
public class MyScriptableObject : ScriptableObject  
{
	public string[] strings;
	public int[] integers;
	public float[] floats;
	public bool[] booleans;
	public byte[] bytes;
	
	[MenuItem("Editor/Create Data Asset")]
	static void CreateDataAsset()
	{		
		//資料 Asset 路徑
		string holderAssetPath = "Assets/Resources/";
	    	
		if(!Directory.Exists(holderAssetPath)) Directory.CreateDirectory(holderAssetPath);
		
		//建立實體
		MyScriptableObject holder = ScriptableObject.CreateInstance<MyScriptableObject> ();
		
		//使用 holder 建立名為 dataHolder.asset 的資源
		AssetDatabase.CreateAsset(holder, holderAssetPath + "dataHolder.asset");
		//
		EditorUtility.DisplayDialog("create Asset", "dataHolder.asset", "OK", "");
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	//
	void OnGUI()
	{
		//非Editor模式下使用..
		if(GUILayout.Button("Loading.."))
		{ 	
			MyScriptableObject holder = (MyScriptableObject)Resources.Load("dataHolder" , typeof(MyScriptableObject));
		}
	}
}

//Unity 3D : 製作與載入 AssetBundle / 設定值做成外部資源來讀入
public class MyBuildAssetBundle : MonoBehaviour  
{
	//直接將選擇的資源打包成 AssetBundle，
	[MenuItem("Editor/Build Selected AssetBunldes")]
	static void ExecCreateAssetBunldes()
	{
		// AssetBundle 的資料夾名稱及副檔名
		// 建議最後將Assetbundle放在StreamingAssets文件夾下，如果沒有就創建一個，因為移動平台下只能讀取這個路徑 string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".assetbundle";
		string targetDir = "Assets/StreamingAssets";
		string extensionName = ".assetBunldes";
		//取得在 Project 視窗中選擇的資源(包含資料夾的子目錄中的資源)
		Object[] SelectedAsset = Selection.GetFiltered(typeof (Object), SelectionMode.DeepAssets);		
		//建立存放 AssetBundle 的資料夾
		if(!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);
		foreach(Object obj in SelectedAsset)
		{			
			//資源檔案路徑
			string sourcePath = AssetDatabase.GetAssetPath(obj);
			
			// AssetBundle 儲存檔案路徑
			string targetPath = targetDir + Path.DirectorySeparatorChar + obj.name + extensionName;
			
			if(File.Exists(targetPath)) File.Delete(targetPath);

			//打包的目標為 GameObject、Texture2D、Material 三種型別的資源
		  //if(!(obj is GameObject) && !(obj is Texture2D) && !(obj is Material)) continue;
			
			//建立 AssetBundle
			//默認情況下打的包只能在電腦上用，如果要在手機上用就要添加一個參數。
			//if(BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies,BuildTarget.Android))
			//電腦上和手機上打出來的Assetbundle不能混用，不同平台只能用自己的。
			if(BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies))
			{				
				Debug.Log(obj.name + " 建立完成");				
			}
			else
			{				
				Debug.Log(obj.name + " 建立失敗");
			}
		}
	}
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	//非Editor模式下使用..
	void OnGUI()
	{		
		if(GUI.Button(new Rect(5,35,100,25) , "Load GameObject"))
		{			
			StartCoroutine(LoadGameObject());//need base from MonoBehaviour
		}
	}
	//製作好的 AssetBundle 最終目的還是要在遊戲中載入，
	private IEnumerator LoadGameObject()
	{		
		// AssetBundle 檔案路徑
		string path = string.Format("file://{0}/Assets/StreamingAssets/{1}.assetBunldes" , Application.dataPath , "TestGameObject");
		//  載入 AssetBundle
		WWW bundle = new WWW(path);		
		//等待載入完成
		yield return bundle;		
		//實例化 GameObject 並等待實作完成
		yield return Instantiate(bundle.assetBundle.mainAsset);		
		//卸載 AssetBundle
	  //bundle.assetBundle.Unload(false);
	}
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Ability
{
	public class AbilityGraphEditor : Editor
	{
		//[MenuItem( "Ability/Graph/Create" )]
		[MenuItem("Assets/Create/Archtype/AbilityGraph")]
		public static void CreateGraph()
		{
			var asset = ScriptableObject.CreateInstance( "AbilityGraph" );
			var endNameEditorAction = ScriptableObject.CreateInstance<AbilityGraphEndNameAction>();
			var icon = AssetPreview.GetMiniThumbnail( asset );
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists( asset.GetInstanceID(), endNameEditorAction, "AbilityGraph.asset", icon, null );
		}
	}
	public class AbilityGraphEndNameAction : EndNameEditAction
	{
		public override void Action( int instanceId, string pathName, string resourceFile )
		{
			var asset = EditorUtility.InstanceIDToObject( instanceId );
			AssetDatabase.CreateAsset( asset, pathName );
			AssetDatabase.SaveAssets();
		}
	}
}



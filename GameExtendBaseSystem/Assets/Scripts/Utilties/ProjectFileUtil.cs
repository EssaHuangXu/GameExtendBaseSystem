using System;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Ability
{

	public class ProjectFileUtil
	{
		public static string GetSelectProjectPath()
		{
			string path = "Assets";
			UnityEngine.Object[] assets = Selection.GetFiltered( typeof( UnityEngine.Object ), SelectionMode.Assets );
			for( int index = 0; index < assets.Length; index++ )
			{
				var asset = assets[index];
				string assetPath = AssetDatabase.GetAssetPath( asset );
				if( !string.IsNullOrEmpty(assetPath) && File.Exists(assetPath) )
				{
					path = Path.GetDirectoryName( assetPath );
					break;
				}
				else if( !string.IsNullOrEmpty( assetPath ) && Directory.Exists( assetPath ) )
				{
					path = assetPath;
					break;
				}
			}

			return path;
		}
	}
}

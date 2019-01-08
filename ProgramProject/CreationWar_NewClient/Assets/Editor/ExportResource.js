@MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")
static function ExportResource () {
	// Bring up save panel
	var path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
	if (path.Length != 0)
	{
		// Build the resource file from the active selection.
		var selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		
		BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);	
		Selection.objects = selection;
	}
}

@MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")
static function ExportResourceNoTrack () {
	// Bring up save panel
	var path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
	if (path.Length != 0)
	{
		// Build the resource file from the active selection.
		BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
	}
}
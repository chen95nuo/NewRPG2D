#if UNITY_EDITOR
using System.Collections.Generic;
﻿using UnityEngine;
using UnityEditor;

public static class UITemplateFormatter
{
	private static GameObject GetTemplateContainer()
	{
		return GameObject.Find("TemplateContainer");
	}

	private static GameObject GetUIContainer()
	{
		return GameObject.Find("UIContainer");
	}

	public static void Fromat()
	{
		GameObject templateContainer = GetTemplateContainer();
		if (templateContainer == null)
		{
			Debug.LogError("templateContainer is null");
			return;
		}

		// Fromat UI in template
		GameObject uiContainer = GetUIContainer();
		if (uiContainer == null)
		{
			Debug.LogError("uiContainer is null");
			return;
		}

		// 应用模板组下面的控件
		foreach (var uiTemplate in templateContainer.GetComponentsInChildren<UITemplate>(true))
			if (uiTemplate.transform.parent == templateContainer.transform)
				foreach (var component in templateContainer.GetComponentsInChildren(uiTemplate.GetTargetType(), true))
					uiTemplate.Apply(component);

		// 应用UIContrainer下面的控件
		foreach (var uiTemplate in templateContainer.GetComponentsInChildren<UITemplate>(true))
			if (uiTemplate.transform.parent == templateContainer.transform)
				foreach (var component in uiContainer.GetComponentsInChildren(uiTemplate.GetTargetType(), true))
					uiTemplate.Apply(component);
	}

	public static void RemoveUnusedTemplate()
	{
		GameObject templateContainer = GetTemplateContainer();
		if (templateContainer == null)
		{
			Debug.LogError("templateContainer is null");
			return;
		}

		// Fromat UI in template
		GameObject uiContainer = GetUIContainer();
		if (uiContainer == null)
		{
			Debug.LogError("uiContainer is null");
			return;
		}

		List<UITemplate> templateListToRemove = new List<UITemplate>();
		for (int index = 0; index < templateContainer.transform.childCount; index++)
		{
			UITemplate uiTemplate = templateContainer.transform.GetChild(index).GetComponent<UITemplate>();
			if (uiTemplate == null)
				continue;

			bool beUsing = false;
			foreach (var component in uiContainer.GetComponentsInChildren(uiTemplate.GetTargetType(), true))
			{
				SpriteRoot btnCtrl = component as SpriteRoot;
				SpriteText textCtrl = component as SpriteText;

				if (btnCtrl != null)
				{
					if (btnCtrl.templateName.Equals(uiTemplate.name))
					{
						beUsing = true;
						break;
					}
				}

				if (textCtrl != null)
				{
					if (textCtrl.templateName.Equals(uiTemplate.name))
					{
						beUsing = true;
						break;
					}
				}
			}



			if (beUsing)
				continue;
			templateListToRemove.Add(uiTemplate);
		}

		for (int index = 0; index < templateListToRemove.Count; )
		{
			Debug.Log("Remove template " + templateListToRemove[index].name);
			templateListToRemove.RemoveAt(index);
			GameObject.DestroyImmediate(templateListToRemove[index].gameObject);
		}
	}

	public static void SelectUIWithTemplate()
	{
		GameObject[] selectedObjs = Selection.gameObjects;
		if (selectedObjs == null || selectedObjs.Length == 0)
			return;

		UITemplate uiTemplate = selectedObjs[0].GetComponent<UITemplate>();

		// Fromat UI in template
		GameObject uiContainer = GetUIContainer();
		if (uiContainer == null)
		{
			Debug.LogError("uiContainer is null");
			return;
		}

		List<GameObject> objsWithTemplate = new List<GameObject>();
		foreach (var component in uiContainer.GetComponentsInChildren(uiTemplate.GetTargetType(), true))
		{
			SpriteRoot btnCtrl = component as SpriteRoot;
			SpriteText textCtrl = component as SpriteText;

			if (btnCtrl != null)
			{
				if (btnCtrl.templateName.Equals(uiTemplate.name))
				{
					objsWithTemplate.Add(component.gameObject);
				}
			}

			if (textCtrl != null)
			{
				if (textCtrl.templateName.Equals(uiTemplate.name))
				{
					objsWithTemplate.Add(component.gameObject);
				}
			}
		}

		Selection.objects = objsWithTemplate.ToArray();
	}

	public static List<string> GetTemplateNameList(System.Type baseType, bool insertEmpty)
	{
		GameObject templateContainer = GetTemplateContainer();
		if (templateContainer == null)
			return new List<string>();

		GameObject uiContainer = GetUIContainer();
		if (uiContainer == null)
		{
			Debug.LogError("uiContainer is null");
			return new List<string>();
		}

		List<string> nameList = new List<string>();
		foreach (var uiTemplate in templateContainer.GetComponentsInChildren<UITemplate>(true))
			if (uiTemplate.transform.parent == templateContainer.transform && (uiTemplate.GetTargetType() == baseType || uiTemplate.GetTargetType().IsSubclassOf(baseType)))
				if (nameList.Contains(uiTemplate.name) == false)
					nameList.Add(uiTemplate.name);

		nameList.Sort();
		if (insertEmpty)
			nameList.Insert(0, "Empty");

		return nameList;
	}

	public static UITemplate GetTemplateByName(System.Type baseType, string name)
	{
		GameObject templateContainer = GetTemplateContainer();
		if (templateContainer == null)
			return null;

		GameObject uiContainer = GetUIContainer();
		if (uiContainer == null)
		{
			Debug.LogError("uiContainer is null");
			return null;
		}

		foreach (var uiTemplate in templateContainer.GetComponentsInChildren<UITemplate>(true))
			if (uiTemplate.transform.parent == templateContainer.transform && (uiTemplate.GetTargetType() == baseType || uiTemplate.GetTargetType().IsSubclassOf(baseType)))
				if (uiTemplate.name.Equals(name))
					return uiTemplate;

		return null;
	}
}
#endif

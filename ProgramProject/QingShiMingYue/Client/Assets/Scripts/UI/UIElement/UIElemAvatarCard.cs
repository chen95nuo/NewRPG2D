using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemAvatarCard : MonoBehaviour
{
	public AutoSpriteControlBase cardUI;
	public AutoSpriteControlBase cardFrame;
	public AutoSpriteControlBase cityUI;

	private int avatarId = IDSeg.InvalidId;
	public int AvatarId
	{
		get { return avatarId; }
	}

	public Material CardMaterial
	{
		get { return avatarCardMat; }
	}

	private int soundIndex = -1;
	private bool smallCard;
	private bool playVoice;

	private Material avatarCardMat;
	private bool isTextureCreator = false;

	void OnDestroy()
	{
		Clear();
	}

	public void SetData(int avatarId, bool smallCard, bool playVoice, Material cardMat)
	{
		Clear();

		this.avatarId = avatarId;
		this.smallCard = smallCard;
		this.playVoice = playVoice;

		if (cardMat != null)
		{
			isTextureCreator = false;
			ShowCardTexture(cardMat);
		}
		else
		{
			isTextureCreator = true;
			HideCardUI(true);
			StartCoroutine("LoadAvatarCard");
		}
	}

	public void Clear()
	{
		StopCoroutine("LoadAvatarCard");

		this.avatarId = IDSeg.InvalidId;

		if (isTextureCreator && avatarCardMat != null)
		{
			Object.Destroy(avatarCardMat.mainTexture);
			Object.Destroy(avatarCardMat);
			avatarCardMat = null;

			//Debug.Log("删除角色卡纹理");
		}

		isTextureCreator = false;
	}

	private void ShowCardTexture(Material cardMat)
	{
		// Material
		avatarCardMat = cardMat;

		// Set card to UI
		if (smallCard == false)
			UIUtility.SetIconInUVUnit(cardUI, avatarCardMat, new Rect(0, 0, 1, 1));
		else
		{
			var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(this.avatarId);
			UIUtility.SetIconInUVUnit(cardUI, avatarCardMat, ClientServerCommon.Converter.ToRect(avatarCfg.smardCardRect));
		}

		// 卡牌控件之前是隐藏的.
		HideCardUI(false);

		if (playVoice)
			PlayVoice();
	}

	private void HideCardUI(bool hide)
	{
		cardUI.Hide(hide);
		if (cardFrame != null)
			cardFrame.Hide(hide);
		if (cityUI != null)
			cityUI.Hide(hide);
	}

	private void PlayVoice()
	{
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId);
		if (avatarCfg == null || avatarCfg.voices == null || avatarCfg.voices.Count <= 0)
			return;

		int tempSoundIndex = Random.Range(0, avatarCfg.voices.Count);

		if (tempSoundIndex == soundIndex && tempSoundIndex < avatarCfg.voices.Count - 1)
			tempSoundIndex++;
		else if (tempSoundIndex == soundIndex && tempSoundIndex > 0)
			tempSoundIndex--;

		soundIndex = tempSoundIndex;

		if (soundIndex < avatarCfg.voices.Count && !string.IsNullOrEmpty(avatarCfg.voices[soundIndex]))
			AudioManager.Instance.PlayStreamSound(avatarCfg.voices[soundIndex], 0f);
	}

	public void StopVoice()
	{
		if (soundIndex < 0)
			return;

		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId);
		if (avatarCfg == null || avatarCfg.voices == null || avatarCfg.voices.Count <= 0)
			return;

		if (soundIndex < avatarCfg.voices.Count && !string.IsNullOrEmpty(avatarCfg.voices[soundIndex]))
			AudioManager.Instance.StopSound(avatarCfg.voices[soundIndex]);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator LoadAvatarCard()
	{
		yield return null;

		// Create avatar card texture
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId);

		var www = ResourceManager.Instance.LoadStreamingTexture(avatarCfg.cardPicture);
		yield return www;

		var cardMat = new Material(Shader.Find("Kod/UI/Transparent Color"));
		cardMat.mainTexture = www.textureNonReadable;

		//Debug.Log("创建角色卡纹理");

		ShowCardTexture(cardMat);

		www = null;
	}
}

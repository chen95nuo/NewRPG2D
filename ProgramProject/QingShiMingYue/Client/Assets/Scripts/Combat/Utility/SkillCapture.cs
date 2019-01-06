using UnityEngine;

public class SkillCapture : MonoBehaviour
{
	public UIBtnCamera btnCamera;

	private BattleRole role;
	private int oldRoleLayer;
	private Transform oldCameraParent;
	private float oldSize = 0.8f;

	public void SetTarget(BattleRole newRole)
	{
		if (role != null)
		{
			// Detach from oldRole
			ObjectUtility.AttachToParentAndKeepLocalTrans(oldCameraParent.gameObject, btnCamera.captureCamera.gameObject);

			// Reset orthographicSize of captureCamera
			btnCamera.captureCamera.orthographicSize = oldSize;

			// Reset newRole layer
			if (role.gameObject.layer == GameDefines.AvatarCaptureLayer)
				role.Avatar.SetGameObjectLayer(oldRoleLayer);
		}

		if (newRole != null)
		{
			// Attach to newRole
			oldCameraParent = btnCamera.gameObject.transform.parent;
			oldSize = btnCamera.captureCamera.orthographicSize;
			btnCamera.captureCamera.orthographicSize *= newRole.CachedTransform.localScale.x;
			ObjectUtility.AttachToParentAndKeepLocalTrans(newRole.gameObject, btnCamera.captureCamera.gameObject);

			// Set newRole layer
			oldRoleLayer = newRole.gameObject.layer;
			newRole.Avatar.SetGameObjectLayer(GameDefines.AvatarCaptureLayer);

			// Shadow will keep old layer
			newRole.Avatar.Shadow.layer = oldRoleLayer;
			foreach (var subObject in newRole.Avatar.Shadow.transform.GetComponentsInChildren<Transform>())
			{
				// except clickRoleButton
				UIButton3D clickRoleButton = subObject.gameObject.GetComponent<UIButton3D>();
				if (clickRoleButton != null)
				{
					continue;
				}

				subObject.gameObject.layer = oldRoleLayer;
			}

			// except FXController
			foreach (var fx in newRole.transform.GetComponentsInChildren<FXController>())
			{
				ObjectUtility.SetObjectLayer(fx.gameObject, oldRoleLayer);
			}
		}

		this.role = newRole;
	}

	void OnDestroy()
	{
		SetTarget(null);
	}
}

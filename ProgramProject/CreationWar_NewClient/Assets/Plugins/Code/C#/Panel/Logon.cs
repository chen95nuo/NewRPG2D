using UnityEngine;
using System.Collections;

public class Logon : MonoBehaviour {


    public UILabel lblLicense;
    public UILabel lblLicenseWarning;
    public UIInput txtLicense;
    public UILabel lblPlayerInvite;
    public UIInput txtPlayerInvite;

    void OnEnable()
    {
        //if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo!=null&&
        //    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo.ContainsKey((byte)yuan.YuanPhoton.BenefitsType.LogonStatus) &&
        //    (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.LogonStatus] == 2)
        //{
        //    lblLicense.gameObject.active = true;
        //    lblLicenseWarning.gameObject.active = true;
        //    txtLicense.gameObject.SetActiveRecursively(true);
        //}
        //else
        //{
        //    lblLicense.gameObject.active = false;
        //    lblLicenseWarning.gameObject.active = false;
        //    txtLicense.gameObject.SetActiveRecursively(false);
        //}

        //if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo!=null&&
        //    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo.ContainsKey((byte)yuan.YuanPhoton.BenefitsType.PlayerInvite) &&
        //    (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.PlayerInvite] == 1)
        //{
        //    lblPlayerInvite.gameObject.active = true;
        //    txtPlayerInvite.gameObject.SetActiveRecursively(true);
        //}
        //else
        //{
        //    lblPlayerInvite.gameObject.active = false;
        //    txtPlayerInvite.gameObject.SetActiveRecursively(false);
        //}
        if (YuanUnityPhoton.dicBenefitsInfo != null &&
    YuanUnityPhoton.dicBenefitsInfo.ContainsKey((byte)yuan.YuanPhoton.BenefitsType.LogonStatus) &&
    (int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.LogonStatus] == 2)
        {
            lblLicense.gameObject.active = true;
            lblLicenseWarning.gameObject.active = true;
            txtLicense.gameObject.SetActiveRecursively(true);
        }
        else
        {
            lblLicense.gameObject.active = false;
            lblLicenseWarning.gameObject.active = false;
            txtLicense.gameObject.SetActiveRecursively(false);
        }

        if (YuanUnityPhoton.dicBenefitsInfo != null &&
            YuanUnityPhoton.dicBenefitsInfo.ContainsKey((byte)yuan.YuanPhoton.BenefitsType.PlayerInvite) &&
            (int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.PlayerInvite] == 1)
        {
            lblPlayerInvite.gameObject.active = true;
            txtPlayerInvite.gameObject.SetActiveRecursively(true);
        }
        else
        {
            lblPlayerInvite.gameObject.active = false;
            txtPlayerInvite.gameObject.SetActiveRecursively(false);
        }
    }
}

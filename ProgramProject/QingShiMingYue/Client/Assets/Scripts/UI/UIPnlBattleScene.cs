using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPnlBattleScene : UIModule
{
    public override bool OnShow(_UILayer layer, params object[] userDatas)
    {
        if (!base.OnShow(layer, userDatas))
            return false;
        if (BattleScene.GetBattleScene() != null)
            BattleScene.GetBattleScene().BattleCameraCtrl.AddInputDelegate();
        return true;
    }

    private IEnumerator BattleData()
    {
        yield return new WaitForSeconds(0.5f);
        BattleScene.GetBattleScene().BattleCameraCtrl.AddInputDelegate();
    }

    public override void OnHide()
    {
        BattleScene.GetBattleScene().BattleCameraCtrl.RemoveInputDelegate();
        base.OnHide();
    }

    public override void Overlay()
    {
        base.Overlay();

        //奇遇战斗弹板不关场景相机
        if (SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>() != null)
            if (SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>().BattleType != ClientServerCommon._CombatType.Adventure)
                KodGames.Camera.main.enabled = false;

        BattleScene.GetBattleScene().BattleCameraCtrl.RemoveInputDelegate();
    }

    public override void RemoveOverlay()
    {
        base.RemoveOverlay();
        KodGames.Camera.main.enabled = true;
        BattleScene.GetBattleScene().BattleCameraCtrl.AddInputDelegate();
    }
}

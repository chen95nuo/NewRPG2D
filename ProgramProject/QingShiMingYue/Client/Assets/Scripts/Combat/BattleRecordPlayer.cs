
using UnityEngine;
using System.Collections.Generic;
using KodGames.ClientClass;
using ClientServerCommon;
using KodGames;

public class BattleRecordPlayer
{
    //private static bool enableLog = false;
    //public static bool EnableLog
    //{
    //    get { return enableLog; }
    //    set { enableLog = value; }
    //}

    public delegate void LoadingFinishDelegate();

    public delegate void PlayerStateChangingDelegate(BattleRecordPlayer player, _PlayerState oldState, _PlayerState newState);

    public delegate void PlayerStateChangedDelegate(BattleRecordPlayer player, _PlayerState oldState, _PlayerState newState);

    public enum _PlayerState
    {
        Idle,
        LoadingResource,
        WaitingStart,
        Playering,
        Pause,
        PlayFinished,
    }

    private BattleScene battleScene;
    public BattleScene BattleScene
    {
        get { return battleScene; }
    }

    private _PlayerState playerState = _PlayerState.Idle;
    public _PlayerState PlayerState
    {
        get { return playerState; }
    }

    private KodGames.ClientClass.BattleRecord battleRecord = new BattleRecord();
    public KodGames.ClientClass.BattleRecord BattleRecord
    {
        get { return battleRecord; }
    }

    private int battleIndex;
    public int BattleIndex
    {
        get { return battleIndex; }
        set { battleIndex = value; }
    }

    private int sponsorTeamIndex;
    public int SponsorTeamIndex
    {
        get { return sponsorTeamIndex; }
    }

    public int OpponentTeamIndex
    {
        get { return sponsorTeamIndex == 0 ? 1 : 0; }
    }

    public bool FirstBattle
    {
        get { return battleIndex == 0; }
    }

    private bool lastBattle;
    public bool LastBattle
    {
        get { return lastBattle; }
    }

    private bool isTutorial = false;
    public bool IsTutorial
    {
        get { return isTutorial; }
        set { isTutorial = value; }
    }

    private bool isSkip = false;
    public bool IsSkip
    {
        get { return isSkip; }
        set { isSkip = value; }
    }

    private bool canSkip = false;
    public bool CanSkip
    {
        get { return canSkip; }
        set { canSkip = value; }
    }

    private bool hasShowSkip = false;
    private bool canShowSkip = false;
    public bool CanShowSkip
    {
        get { return canShowSkip; }
        set { canShowSkip = value; }
    }

    private float skipTimer = 0;
    private float skipDelayTime = 0;
    public float SkipDelayTime
    {
        get { return skipDelayTime; }
        set { skipDelayTime = value; }
    }

    private bool hasSkip = false;
    public bool HasSkip
    {
        get { return hasSkip; }
        set { hasSkip = value; }
    }

    private bool isFinished = false;
    public bool IsFinished
    {
        get { return isFinished; }
        set { isFinished = value; }
    }

    private bool isSkipCameraAnimation = false;
    public bool IsSkipCameraAnimation
    {
        get { return isSkipCameraAnimation; }
        set { isSkipCameraAnimation = value; }
    }

    private bool canSkipCameraAnimation = false;
    public bool CanSkipCameraAnimation
    {
        get { return canSkipCameraAnimation; }
        set { canSkipCameraAnimation = value; }
    }

    private List<BattleRole> battleRoles = new List<BattleRole>();
    public List<BattleRole> BattleRoles
    {
        get { return battleRoles; }
    }

    //	private LoadingFinishDelegate loadingFinishDelegate;
    //	private PlayerStateChangingDelegate playerStateChangingDelegate;
    private PlayerStateChangedDelegate playerStateChangedDelegate;
    private List<BattleRound> battleRounds = new List<BattleRound>();

    public void Initialize()
    {
        // Set initialize state
        playerState = _PlayerState.Idle;

        battleScene = BattleScene.GetBattleScene();
        if (battleScene)
            battleScene.battleRecordPlayer = this;

        hasSkip = false;
        isSkip = false;
        isTutorial = false;
        isSkipCameraAnimation = false;
        canSkipCameraAnimation = false;
        hasShowSkip = false;
        canShowSkip = false;
    }

    public void Release()
    {

    }

    public void DestroyAllBattleRole()
    {
        foreach (BattleRole role in battleRoles)
        {
            if (role != null)
            {
                role.BattleBar.DestroySelf();
                role.BattleBar = null;
                Object.Destroy(role.gameObject);
            }
        }
    }

    void LogAvatarAttrs(CombatAvatarData avatarData, int battleIndex, bool isAvatarResult)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (isAvatarResult)
            sb.Append("AvatarResult ");

        sb.Append(string.Format("Battle {0} Avatar {1} HP:{2}/MaxHP{3}\n",
        battleIndex, avatarData.DisplayName, avatarData.GetAttributeByType(_AvatarAttributeType.HP).Value, avatarData.GetAttributeByType(_AvatarAttributeType.MaxHP).Value));

        Debug.Log(sb.ToString());
    }
    //battleRecord 一场战斗数据  sponsorTeamIndex 我方战斗Index  battleIndex 场次下班  lastBattle 是否是最后一场战斗
    public void InitializeBattle(KodGames.ClientClass.BattleRecord battleRecord, int sponsorTeamIndex, int battleIndex, bool lastBattle)
    {
        this.battleRecord = KodGames.ClientClass.BattleRecord.CopyTo(battleRecord); //一场战斗数据
        this.sponsorTeamIndex = sponsorTeamIndex;
        this.battleIndex = battleIndex;
        this.lastBattle = lastBattle;
        // Save previous opponent avatars 保存以前的对手头像
        List<BattleRole> oldBattleRoles = new List<BattleRole>(battleRoles);
        battleRoles.Clear();

        // Add avatar
        for (int teamIndex = 0; teamIndex < battleRecord.TeamRecords.Count; ++teamIndex)
        {
            KodGames.ClientClass.TeamRecord teamRecord = battleRecord.TeamRecords[teamIndex];

            foreach (var avatarResult in teamRecord.AvatarResults)
            {
                BattleRole battleRole = null;

                //Avatars for sponsor has been loaded in the first battle, skip them
                if (teamIndex == sponsorTeamIndex && oldBattleRoles.Count != 0)
                {
                    for (int i = 0; i < oldBattleRoles.Count; ++i)
                    {
                        BattleRole role = oldBattleRoles[i];

                        if (role != null && role.TeamIndex == sponsorTeamIndex && role.AvatarIndex == avatarResult.AvatarIndex)
                        {
                            // Reset attribute
                            //Bug记录：此处的赋值会覆盖上个Round的"AvatarData"
                            //role.AvatarData.Attributes = avatarResult.CombatAvatarData.Attributes;

                            //清空Buff，确保Buff不会保留到下一场战斗而影响下一场战斗的AddBuff事件。
                            role.GetBuffs().Clear();
                            role.AvatarData.Copy(avatarResult.CombatAvatarData);

                            // Use this role
                            battleRole = role;
                            oldBattleRoles[i] = null;
                            battleRole.AvatarHP = role.AvatarData.GetAttributeByType(_AvatarAttributeType.HP).Value;
                            break;
                        }
                    }
                }

                // Not found, create a new one
                if (battleRole == null)
                {
                    battleRole = BattleRole.Create(avatarResult.CombatAvatarData, teamIndex, avatarResult.AvatarIndex);
                }
                else
                {
                    // Stop dead action which can not be interrupt by other action
                    battleRole.StopAction();
                    battleRole.InitializeSkillPower();
                }
                battleRole.Avatar.StopAllPfx();
                battleRole.CheckAndInitBattleFuncOpenStatus();

                // Set scale
                float scale = avatarResult.CombatAvatarData.Scale;
                battleRole.transform.localScale = new Vector3(scale, scale, scale);

                // Add battle
                battleRoles.Add(battleRole);
            }
        }

        // Release unused roles
        foreach (var role in oldBattleRoles)
        {
            if (role != null)
            {
                role.BattleBar.DestroySelf();
                role.BattleBar = null;
                Object.Destroy(role.gameObject);
            }
        }

        //防止AvatarIndex与数组序号不匹配，这种情况会出现在手动配置的剧情战斗中
        battleRoles.Sort((m, n) =>
        {
            return m.AvatarIndex - n.AvatarIndex;
        });
    }

    public void BuildPlayer()
    {
        if (this.BattleScene == null)
            Initialize();
        battleRounds.Clear();
        /*
         * 构造回放数据
         */
        if (FirstBattle)
        {
            // 对手首先入场
            battleRounds.Add(new PlayerEnterSceneRound(this, null, this.OpponentTeamIndex, false, false, false).Initialize());
            // 挑战者入场,

            if (this.BattleScene != null)
                battleRounds.Add(new PlayerEnterSceneRound(this, null, this.SponsorTeamIndex, this.BattleScene.NeedCameraTrace(), true, true).Initialize());
            // 播放入场对话
            battleRounds.Add(new CampaignDialogueRound(this, battleRounds[battleRounds.Count - 1], _StateDialogueType.BeforeBattle).Initialize());
        }
        else
        {
            // Do not use now
            // Revive dead role in last battle
            battleRounds.Add(new SponsorReviveRound(this).Initialize());

            if (BattleScene.forceUseFirstCombatMarker == false)
                battleRounds.Add(new SponsorGoToOpponentRound(this, battleRounds[battleRounds.Count - 1]).Initialize());
            // 对手入场, 对于千层楼战斗playOpponentEnterFx == true
            battleRounds.Add(new PlayerEnterSceneRound(this, battleRounds[battleRounds.Count - 1], this.OpponentTeamIndex, false, this.BattleScene.playOpponentEnterFx, false).Initialize());
        }

        // 播放场景与定义的摄像机动画, 现在在竞技场战斗中使用
        if (battleScene.GetCameraAnimationCount() > 0)
            battleRounds.Add(new CameraAnimationBattleRound(this, null).Initialize());

        // 将摄像机旋转到战斗位置, 只在第一场战斗旋转相机.
        if (battleScene.NeedCameraTrace() && FirstBattle)
            battleRounds.Add(new CameraTraceRound(this, battleRounds[battleRounds.Count - 1]).Initialize());

        // Special for process Tutorial Battle
        if (IsTutorial)
            battleRounds.Add(new TutorialRound(this, battleRounds[battleRounds.Count - 1]).Initialize());

        // Campaign Dialogue
        battleRounds.Add(new CampaignDialogueRound(this, battleRounds[battleRounds.Count - 1], _StateDialogueType.BeforeStage).Initialize());

        // battle start
        battleRounds.Add(new BattleStartRound(this, battleRounds[battleRounds.Count - 1]).Initialize());

        // 摄像机拉近到战斗位, 对于强制在固定点的战斗第一场之外不拉近
        if (FirstBattle || BattleScene.forceUseFirstCombatMarker == false)
            battleRounds.Add(new CameraEnterBattleRound(this, battleRounds[battleRounds.Count - 1]).Initialize());

        /*
         * Build combat round 战斗过程中的播放顺序插入
         */
        for (int i = 0; i < battleRecord.CombatRecord.RoundRecords.Count; ++i)
        {
            // Grab current round record
            var roundRecord = battleRecord.CombatRecord.RoundRecords[i];

            // Create current combat round
            BattleRound battleRound = null;


            switch (roundRecord.RoundType)
            {
                case _CombatRoundType.EnterBattleGround:
                    battleRound = new EnterBattleGroundRound(roundRecord, this);
                    break;

                case _CombatRoundType.EnterBattleSkill:
                    battleRound = new EnterBattleSkillRound(roundRecord, this);
                    break;

                case _CombatRoundType.NormalCombat:
                    battleRound = new NormalCombatRound(roundRecord, this);
                    break;

                case _CombatRoundType.ActiveSkillCombat:
                    battleRound = new SkillCombatRound(roundRecord, this);
                    break;

                case _CombatRoundType.CompositeSkillCombat:
                    battleRound = new SkillCombatRound(roundRecord, this);
                    break;
            }

            if (battleRound == null)
            {
                Debug.Log("combatRound == null roundRecord.RoundType:" + _CombatRoundType.GetNameByType(roundRecord.RoundType));
            }

            /*
             * Set round dependency
             */
            // Grab previous round for this team row
            battleRound.SequenceRound = null;
            for (int prevIdx = i - 1; prevIdx >= 0; --prevIdx)
            {
                CombatRound round = battleRounds[prevIdx] as CombatRound;

                if (round == null)
                    continue;

                if (round.RoundRecord.TeamIndex == battleRound.RoundRecord.TeamIndex && round.RoundRecord.RowIndex == battleRound.RoundRecord.RowIndex)
                {
                    battleRound.SequenceRound = round;
                    break;
                }
            }

            // Grab previous round record
            var prevRoundRecord = i != 0 ? battleRecord.CombatRecord.RoundRecords[i - 1] : null;

            // Grab previous combat round
            var prevRound = battleRounds.Count != 0 ? battleRounds[battleRounds.Count - 1] : null;

            // This round should not be start before previous round is started
            battleRound.PrevRound = prevRound;

            //			if (prevRound != null && prevRound is EnterBattleGroundRound)
            //			{
            //				combatRound.AfterRound = prevRound;
            //			}
            //			else
            if (prevRoundRecord != null)
            {
                switch (prevRoundRecord.RoundType)
                {
                    case _CombatRoundType.NormalCombat:
                        switch (battleRound.RoundRecord.RoundType)
                        {
                            //case _CombatRoundType.NormalCombat:
                            //    var prevCombatRound = prevRound as CombatRound;
                            //    if (prevCombatRound != null && combatRound.RoundRecord.RowIndex == prevCombatRound.RoundRecord.RowIndex)
                            //        combatRound.AfterRound = prevRound;

                            //    break;

                            default:
                                battleRound.AfterRound = prevRound;
                                break;
                        }
                        break;

                    default:
                        battleRound.AfterRound = prevRound;
                        break;
                }
            }
            else
            {
                battleRound.AfterRound = prevRound;
            }

            battleRounds.Add(battleRound);
        }

        if (battleRecord.TeamRecords[0].IsWinner)
        {
            battleRounds.Add(new CampaignDialogueRound(this, battleRounds[battleRounds.Count - 1], _StateDialogueType.AfterStageWin).Initialize());
        }
        else
        {
            battleRounds.Add(new CampaignDialogueRound(this, battleRounds[battleRounds.Count - 1], _StateDialogueType.AfterStageLost).Initialize());
        }

        if (lastBattle)
        {
            battleRounds.Add(new BattleSummaryRound(this, battleRounds[battleRounds.Count - 1]));
        }
        else
        {
            battleRounds.Add(new WaitForNextBattleRound(this, battleRounds[battleRounds.Count - 1]));
        }
    }

    //播放手动配置的剧情战斗
    public void BuildPlayerForCustomBattle()
    {
        battleRounds.Clear();
        /*
         * Build battle rounds
         */
        for (int i = 0; i < battleRecord.CombatRecord.RoundRecords.Count; ++i)
        {
            // Grab current round record
            var roundRecord = battleRecord.CombatRecord.RoundRecords[i];

            // Create current battle round
            BattleRound battleRound = null;

            switch (roundRecord.RoundType)
            {
                case _CombatRoundType.Custom_PrepareEnterScene://战斗前准备，初始化相机位置、设置traceTarget等，但不控制角色进场
                    battleRound = new Custom_PrepareEnterSceneRound(roundRecord, this);
                    break;
                case _CombatRoundType.CameraTrace://转向机特效，将相机从左上角转到正视角
                    battleRound = new CameraTraceRound(roundRecord, this);
                    break;
                case _CombatRoundType.SponsorGoToOpponent://我方跑向敌方
                    battleRound = new SponsorGoToOpponentRound(roundRecord, this);
                    break;
                case _CombatRoundType.BattleStart://战斗开始（UI）
                    battleRound = new BattleStartRound(roundRecord, this);
                    break;
                case _CombatRoundType.CameraEnterBattle://拉近摄像机
                    battleRound = new CameraEnterBattleRound(roundRecord, this);
                    break;
                case _CombatRoundType.EnterBattleGround://进场Round
                    battleRound = new EnterBattleGroundRound(roundRecord, this);
                    break;
                case _CombatRoundType.EnterBattleSkill://霸气技
                    battleRound = new EnterBattleSkillRound(roundRecord, this);
                    break;
                case _CombatRoundType.NormalCombat://蓄力技
                    battleRound = new NormalCombatRound(roundRecord, this);
                    break;
                case _CombatRoundType.ActiveSkillCombat://暴走技
                    battleRound = new SkillCombatRound(roundRecord, this);
                    break;
                case _CombatRoundType.Custom_CameraAnimRound://转动相机
                    battleRound = new Custom_CameraAnimRound(roundRecord, this);
                    break;
                case _CombatRoundType.Custom_RoleTween://角色变化（大小）
                    battleRound = new Custom_RoleTweenRound(roundRecord, this);
                    break;
                case _CombatRoundType.PlotBattleInterludeEffect://战前文字特效（字幕等）
                    battleRound = new PlotBattleInterludeEffectRound(roundRecord, this);
                    break;
            }

            if (battleRound == null)
            {
                Debug.Log("battleRound == null roundRecord.RoundType:" + roundRecord.RoundType);
            }
            else
                battleRound.Initialize();

            /*
             * Set round dependency
             */
            // Grab previous round for this team row
            battleRound.SequenceRound = null;
            for (int prevIdx = i - 1; prevIdx >= 0; --prevIdx)
            {
                CombatRound round = battleRounds[prevIdx] as CombatRound;

                if (round == null)
                    continue;

                if (round.RoundRecord.TeamIndex == battleRound.RoundRecord.TeamIndex && round.RoundRecord.RowIndex == battleRound.RoundRecord.RowIndex)
                {
                    battleRound.SequenceRound = round;
                    break;
                }
            }

            // Grab previous round record
            var prevRoundRecord = i != 0 ? battleRecord.CombatRecord.RoundRecords[i - 1] : null;

            // Grab previous combat round
            var prevRound = battleRounds.Count != 0 ? battleRounds[battleRounds.Count - 1] : null;

            // This round should not be start before previous round is started
            battleRound.PrevRound = prevRound;

            //			if (prevRound != null && prevRound is EnterBattleGroundRound)
            //			{
            //				combatRound.AfterRound = prevRound;
            //			}
            //			else
            if (prevRoundRecord != null)
            {
                switch (prevRoundRecord.RoundType)
                {
                    case _CombatRoundType.NormalCombat:
                        switch (battleRound.RoundRecord.RoundType)
                        {
                            //case _CombatRoundType.NormalCombat:
                            //    var prevCombatRound = prevRound as CombatRound;
                            //    if (prevCombatRound != null && combatRound.RoundRecord.RowIndex == prevCombatRound.RoundRecord.RowIndex)
                            //        combatRound.AfterRound = prevRound;

                            //    break;

                            default:
                                battleRound.AfterRound = prevRound;
                                break;
                        }
                        break;

                    default:
                        battleRound.AfterRound = prevRound;
                        break;
                }
            }
            else
            {
                battleRound.AfterRound = prevRound;
            }

            battleRounds.Add(battleRound);
        }

        if (lastBattle)
        {
            battleRounds.Add(new BattleSummaryRound(this, battleRounds[battleRounds.Count - 1]));
        }
        else
        {
            battleRounds.Add(new WaitForNextBattleRound(this, battleRounds[battleRounds.Count - 1]));
        }
    }

    public void LoadResource()
    {
        // Load resource
        foreach (var battleRole in battleRoles)
        {
            if (battleRole.Avatar.Loaded)
                continue;

            // Load avatar
            var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(battleRole.AvatarData.ResourceId);
            if (avatarCfg == null)
                Debug.LogError(string.Format("Miss Avatar Setting : Id({0:X})", battleRole.AvatarData.ResourceId));

            // 加载角色和角色武器
            battleRole.Avatar.Load(avatarCfg.GetAvatarAssetId(battleRole.AvatarData.BreakThrough));

            //加载武器
            battleRole.UseDefaultWeapons();
            battleRole.UseIllusionWeapons();

            // Process shadow for click role button
            {
                GameObject click_button = new GameObject("click_button");

                UIButton3D button3D = click_button.AddComponent<UIButton3D>();
                BattleRole roleData = battleRole.Avatar.gameObject.GetComponent<BattleRole>();
                UIPnlBattleRoleInfo battleRoleInfo = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlBattleRoleInfo>();
                button3D.scriptWithMethodToInvoke = battleRoleInfo;
                button3D.methodToInvoke = "OnClickRole";
                button3D.Data = roleData;

                click_button.AddComponent<BoxCollider>().size = new Vector3(1.2f, 2.0f, 1.2f);
                click_button.gameObject.layer = GameDefines.UISceneRaycastLayer;
                ObjectUtility.AttachToParentAndResetLocalPosAndRotation(battleRole.Avatar.Shadow, click_button);
                click_button.gameObject.transform.localPosition = new Vector3(0, 1, 0);
                battleRole.Click_Button = button3D;
            }

            // Load battle bar
            SysFx sysFx = SysModuleManager.Instance.GetSysModule<SysFx>();
            if (sysFx != null)
            {
                string battleBarName = GameDefines.uiFXBattleBar;
                if (battleRole.TeamIndex != 0 && string.IsNullOrEmpty(battleScene.sponsorLiftBar) == false)
                    battleBarName = battleScene.sponsorLiftBar;

                FXController battleBar = sysFx.PopUIObj(GameDefines.uiEffectPath, battleBarName, Vector3.zero, true);
                if (battleBar != null)
                {
                    battleRole.BattleBar = battleBar.GetComponent<UIAvatarBattleBar>();
                    battleRole.InitializeSkillPower();
                }
            }

            battleRole.PlayIdleAction();

            // Hide as default
            battleRole.Hide = true;
            battleRole.BattleBar.Hide(true);
        }
    }

    public void Start()
    {
        // Show battle UI
        SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
        if (uiEnv != null)
            uiEnv.ShowUIModule(typeof(UIPnlBattleBar));

        //send data to CameraController,So when some role outof screen,camera can change distance appropriatly.
        CameraController_Battle ctemp = KodGames.Camera.main.GetComponent<CameraController_Battle>();
        if (ctemp != null)
        {
            CameraController_Battle._WatchData wachData = new CameraController_Battle._WatchData();
            wachData.CurrentRoles = new List<BattleRole>();
            wachData.CurrentRoles = this.BattleRoles;

            wachData.isRoleVisible = (idx) =>
            {
                if (battleRoles == null || battleRoles.Count == 0 || idx < 0 || idx >= battleRoles.Count) return false;
                return !battleRoles[idx].Hide;
            };
            ctemp.WatchData = wachData;
        }

        IsFinished = false;
        SetPlayerState(_PlayerState.Playering);
    }

    public void Stop()
    {

    }

    public void Pause()
    {
        SetPlayerState(_PlayerState.Pause);
    }

    public void Resume()
    {
        SetPlayerState(_PlayerState.Playering);
    }

    public void Skip()
    {
        foreach (var battleRound in this.battleRounds)
        {
            if (battleRound.GetType() == typeof(SkipBattleRound) ||
                battleRound.GetType() == typeof(WaitForNextBattleRound) ||
                battleRound.GetType() == typeof(BattleSummaryRound))
            {
                continue;
            }
            battleRound.Finish();
        }

        // Remove last round and add a BattleSummaryRound at the last
        battleRounds.RemoveAt(battleRounds.Count - 1);
        battleRounds.Add(new BattleSummaryRound(this, battleRounds[battleRounds.Count - 1]));

        // insert SkipBattleRound before the lastRound.
        BattleRound lastRound = battleRounds[battleRounds.Count - 1];
        SkipBattleRound skipBattleRound = new SkipBattleRound(this, battleRounds[battleRounds.Count - 2]);
        battleRounds.Insert(battleRounds.Count - 1, skipBattleRound.Initialize());
        lastRound.AfterRound = skipBattleRound;
    }

    public void ScaleTime(float timeScale)
    {
        if (SysModuleManager.Instance.GetSysModule<SysFx>() == null)
        {
            return;
        }

        SysModuleManager.Instance.GetSysModule<SysFx>().ScaleTime(timeScale, float.MaxValue);
    }

    public void ResetScaleTime()
    {
        if (SysModuleManager.Instance.GetSysModule<SysFx>() == null)
        {
            return;
        }

        SysModuleManager.Instance.GetSysModule<SysFx>().ResumeTimeScale();
    }

    public float GetScaleTime()
    {
        return SysModuleManager.Instance.GetSysModule<SysFx>().GetScaleTime();
    }

    private void UpdateSkipButton()
    {
        if (canSkip && !hasShowSkip && CanShowSkip)
        {
            skipTimer += Time.deltaTime;
            if (skipTimer >= skipDelayTime)
            {
                SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
                if (uiEnv != null)
                {
                    UIPnlBattleBar battleBar = uiEnv.GetUIModule<UIPnlBattleBar>();
                    battleBar.HideSkipButton(false);
                    hasShowSkip = true;
                }
            }
        }
    }

    private void UpdateSkip()
    {
        if (IsSkip && !hasSkip)
        {
            Skip();

            // execute once
            hasSkip = true;
        }
    }

    private void UpdateBattleRound()
    {
        // Update state
        foreach (var battleRound in this.battleRounds)
        {
            battleRound.Update();
        }

        // Check starting condition
        foreach (var battleRound in this.battleRounds)
        {
            if (battleRound.RoundState != BattleRound._RoundState.NotStarted)
                continue;

            if (battleRound.CanStart() == false)
                continue;

            battleRound.Start();
        }
    }

    public void Update()
    {
        if (playerState != _PlayerState.Playering)
            return;

        UpdateSkipButton();

        UpdateSkip();

        UpdateBattleRound();
    }

    public void SetPlayerState(_PlayerState newState)
    {
        if (playerState == newState)
            return;

        _PlayerState oldState = playerState;
        playerState = newState;

        if (playerStateChangedDelegate != null)
            playerStateChangedDelegate(this, oldState, newState);
    }

    public BattleRole GetBattleAvatarByIndex(int index)
    {
        if (index >= battleRoles.Count || index < 0)
        {
            Debug.LogError("GetBattleAvatarByIndex Index out of rang " + index.ToString());
            //return null;
        }

        return battleRoles[index];
    }

    // Prior select target which is the same column with the src avatar.
    public BattleRole GetBattleTargetAvatar(ActionRecord actionRecord, int srcColumn, int srcTeamIndex)
    {
        foreach (int targetIndex in actionRecord.TargetAvatarIndices)
        {
            BattleRole role = GetBattleAvatarByIndex(targetIndex);

            if (role == null || role.TeamIndex == srcTeamIndex)
            {
                continue;
            }

            if (role.GetBattlePositionColumn() == srcColumn && !role.IsDead())
            {
                return role;
            }
        }

        return null;
    }

    //	public void SetLoadingFinishDelegate(LoadingFinishDelegate del)
    //	{
    //		loadingFinishDelegate = del;
    //	}

    public void SetPlayeStateChangedDelegate(PlayerStateChangedDelegate del)
    {
        playerStateChangedDelegate = del;
    }

    public bool PlayActionRecord(ActionRecord actionRecord)
    {
        if (actionRecord.TargetAvatarIndices.Count == 0)
        {
            Debug.LogError("No target in action record : " + actionRecord.ActionId.ToString("X8"));
            return false;
        }

        //Debug.Log(string.Format("PlayAction ActionId={0}", actionRecord.ActionId.ToString("X")));

        // Find current role.
        BattleRole actionAvatar = GetBattleAvatarByIndex(actionRecord.SrcAvatarIndex);

        // Find target role.
        BattleRole targetAvatar = GetBattleTargetAvatar(actionRecord, actionAvatar.GetBattlePositionColumn(), actionAvatar.TeamIndex);
        if (targetAvatar == null)
        {
            targetAvatar = GetBattleAvatarByIndex(actionRecord.TargetAvatarIndices[0]);
        }
        //		BattleRole targetAvatar = GetBattleAvatarByIndex(actionRecord.TargetAvatarIndices[0]);

        // Error happened, there's no focus role.
        if (actionAvatar == null)
        {
            Debug.LogError("PlayActionRecord actionAvatar == null actionRecord Id " + actionRecord.ActionId.ToString());
            return false;
        }


        // Log normal action.
        //Debug.Log(string.Format("PlayActionRecord ActRole:{0}, DstRole:{1}, Normal action:{2:X}",
        //    actionAvatar.BattlePosition.ToString("X8"),
        //    targetAvatar != null ? targetAvatar.BattlePosition.ToString("X8") : "null",
        //    actionCfg.id.ToString("X8")));



        if (actionRecord.ActionId == IDSeg.InvalidId)
        {
            // Process logic event directly.
            foreach (var eventRecord in actionRecord.EventRecords)
            {
                // Process event record.
                ProcessEventRecord(actionRecord.ActionId, eventRecord, null, actionAvatar);
            }
        }
        else
        {
            // Action config data.
            AvatarAction actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionById(actionRecord.ActionId);

            // 千层楼战斗，我方原地不动，忽略EnterBattleGround事件
            if (this.BattleScene.playOpponentEnterFx && !FirstBattle && actionAvatar.TeamIndex == sponsorTeamIndex && actionCfg.actionType == AvatarAction._Type.EnterBattleGround)
                return true;

            // Get buff action config data
            if (AvatarAction.IsBuffActionID(actionRecord.ActionId))
            {
                int buffID = Buff.GetBuffIDFromBuffActionID(actionRecord.ActionId);
                Buff buffCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetBuffById(buffID);

                if (buffCfg == null)
                {
                    Debug.LogError("buffCfg == null buffID " + buffID.ToString("X8"));
                }

                actionCfg = buffCfg.GetActionById(Buff.GetActionIDFromBuffActionID(actionRecord.ActionId));
            }

            if (actionCfg == null)
                Debug.LogError("actionCfg == null " + actionRecord.ActionId.ToString("X8"));

            bool prcEvnDrct = false;
           // Debug.LogError(actionCfg.id + "      " + actionRecord.ActionId);
           // Debug.LogError(AvatarAction.IsBuffActionID(actionCfg.id) + "*******" + !actionAvatar.PlayAction(actionRecord.ActionId));
            if (AvatarAction.IsBuffActionID(actionCfg.id) || !actionAvatar.PlayAction(actionRecord.ActionId))
                prcEvnDrct = true;

            if (prcEvnDrct)
            {
                // Process non-logic event directly
                foreach (var eventCfg in actionCfg.events)
                {
                    if (eventCfg.IsLogicEvent())
                        continue;

                    // Construct event record to process
                    EventRecord eventRecord = new EventRecord();
                    eventRecord.EventIndex = eventCfg.index;

                    foreach (var targetAvatarIndex in actionRecord.TargetAvatarIndices)
                    {
                        EventTargetRecord eventTargetRecord = new EventTargetRecord();
                        eventTargetRecord.EventType = eventCfg.eventType;
                        eventTargetRecord.TargetIndex = targetAvatarIndex;

                        eventRecord.EventTargetRecords.Add(eventTargetRecord);
                    }

                    ProcessEventRecord(actionCfg.id, eventRecord, eventCfg, actionAvatar);
                }

                // Process logic event directly.
                for (int i = 0; i < actionRecord.EventRecords.Count; i++)
                {
                    EventRecord eventRecord = actionRecord.EventRecords[i];

                    AvatarAction.Event eventCfg = actionCfg.events[eventRecord.EventIndex];

                    // Process event record.
                    ProcessEventRecord(actionCfg.id, eventRecord, eventCfg, actionAvatar);
                }
            }
            else
            {
                // Set event callback.
                actionAvatar.Avatar.SetAnimEventCb(ActionEventCallback);

                // Push non-logic event
                foreach (var eventCfg in actionCfg.events)
                {
                    if (eventCfg.IsLogicEvent())
                        continue;

                    // Construct event record to process
                    EventRecord eventRecord = new EventRecord();
                    eventRecord.EventIndex = eventCfg.index;

                    foreach (var targetAvatarIndex in actionRecord.TargetAvatarIndices)
                    {
                        EventTargetRecord eventTargetRecord = new EventTargetRecord();
                        eventTargetRecord.EventType = eventCfg.eventType;
                        eventTargetRecord.TargetIndex = targetAvatarIndex;

                        eventRecord.EventTargetRecords.Add(eventTargetRecord);
                    }

                    List<object> parameters = new List<object>();
                    parameters.Add(eventRecord);
                    parameters.Add(eventCfg);
                    parameters.Add(actionAvatar);

                    // Push this event.
                    actionAvatar.Avatar.AddAnimationEvent(eventCfg.keyFrameId, eventCfg.loop, parameters, actionCfg.id);
                }
                // Push logic event list.
                foreach (EventRecord eventRecord in actionRecord.EventRecords)
                {
                    if (eventRecord.EventIndex >= actionCfg.events.Count)
                    {
                        Debug.LogError("eventRecord.EventIndex >= actionCfg.events.Count!");
                        Debug.LogError("eventRecord.EventIndex  " + eventRecord.EventIndex);
                        Debug.LogError("actionCfg.id  " + actionCfg.id.ToString("X"));
                    }

                    AvatarAction.Event eventCfg = actionCfg.events[eventRecord.EventIndex];
                    if (eventCfg.IsLogicEvent() == false)
                        continue;

                    List<object> parameters = new List<object>();
                    parameters.Add(eventRecord);
                    parameters.Add(eventCfg);
                    parameters.Add(actionAvatar);

                    // Push this event.
                    actionAvatar.Avatar.AddAnimationEvent(eventCfg.keyFrameId, eventCfg.loop, parameters, actionCfg.id);
                }
            }
        }


        // SrcRole face to target.
        if (targetAvatar != null)
        {
            // Only face to enemy
            if (actionAvatar.TeamIndex != targetAvatar.TeamIndex)
            {
                actionAvatar.Forward = targetAvatar.Avatar.CachedTransform.position - actionAvatar.Avatar.CachedTransform.position;
                targetAvatar.Forward = actionAvatar.Avatar.CachedTransform.position - targetAvatar.Avatar.CachedTransform.position;
            }

            actionAvatar.TargetRole = targetAvatar;
            targetAvatar.TargetRole = actionAvatar;
        }

        AvatarAction act = ConfigDatabase.DefaultCfg.ActionConfig.GetActionById(actionRecord.ActionId);
        if (act != null)
        {
            if (act.actionType == AvatarAction._Type.TurnRush)
            {
                Vector3 dst = targetAvatar.Position - actionAvatar.Position;
                float dis = dst.magnitude;
                dst.Normalize();

                dst = actionAvatar.Position + dst * (dis - GameDefines.btAvatarRadius);

                actionAvatar.MoveToWithoutAdjustAnimation(dst, ConfigDatabase.DefaultCfg.GameConfig.combatSetting.attackRunSpeed);
            }
            else if (act.actionType == AvatarAction._Type.TurnBack)
            {
                actionAvatar.MoveTo(actionAvatar.Foothold, ConfigDatabase.DefaultCfg.GameConfig.combatSetting.jumpSpeed);
            }
        }

        return true;
    }

    // Process event record.
    public void ProcessEventRecord(int actionId, EventRecord eventRecord, AvatarAction.Event actionEvent, BattleRole battleAvatar)
    {
        // Process event targets.
        bool processedOnTarget = false;

        foreach (var eventTargetRecord in eventRecord.EventTargetRecords)
        {
            BattleRole targetAvatar = GetBattleAvatarByIndex(eventTargetRecord.TargetIndex);

            if (targetAvatar == null)
                continue;

            // 对于非逻辑事件, 判断是否需要在所有目标上面播放. (逻辑事件从原理上面讲应该需要在所有目标上面播放)
            if (actionEvent != null && actionEvent.IsLogicEvent() == false && actionEvent.playOnAllTarget == false && processedOnTarget == true)
                break;

            processedOnTarget = true;

            // Process event AI.
            BattleEvent battleEvent = BattleEvent.CreateEvent(eventTargetRecord.EventType);
            battleEvent.battleRecordPlayer = this;
            battleEvent.actAvatar = battleAvatar;
            battleEvent.targetAvatar = targetAvatar;
            battleEvent.eventTargetRecord = eventTargetRecord;
            battleEvent.actionEventCfg = actionEvent;

            battleEvent.Process();

            //// Log.
            //if (LogEvent)
            //    LogMsg(String.Format("ProcessEventRecord EvnTrgIndex:{0} EvnTrgAvtID:{1} EnvTrgType:{2} AIEvn:{3}", i, targetAvatar.RollData.RollId, eventTargetRecord.EventType, battleEvent.ToString()));
        }

        //// Log.
        //if (LogEvent)
        //    LogMsg(String.Format("ProcessEventRecord EvnIndex:{0} EvnTrgNum:{1} EvnAvtID:{2} EvnType:{3} ActID:{4:X}", eventRecord.EventIndex, eventRecord.GetRecordCount(), battleAvatar.RollData.RollId, actionEventCfg.EventType, actionId));
    }

    private void ActionEventCallback(object userData0, object userData1)
    {
        List<object> parameters = (List<object>)userData0;
        EventRecord eventRecord = parameters[0] as EventRecord;
        AvatarAction.Event actionEventCfg = parameters[1] as AvatarAction.Event;
        BattleRole actionAvatar = parameters[2] as BattleRole;

        int actionId = (int)userData1;

        // Process event record.
        ProcessEventRecord(actionId, eventRecord, actionEventCfg, actionAvatar);
    }
}
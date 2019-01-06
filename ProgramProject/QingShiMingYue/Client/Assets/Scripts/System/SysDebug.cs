using UnityEngine;
using System;
using System.Collections.Generic;
using KodGames;

/// <summary>
/// Debug system to provide debug commands. 
/// </summary>
public class SysDebug : SysModule
{
	/// <summary>
	/// Help command. 
	/// </summary>
	protected class CmdHelp : IConsoleCommand
	{
		public string Name { get { return "Help"; }  }
		public string Description 
		{ 
			get 
			{ 
				string desc = "";
				
				SysDebug dbg = SysModuleManager.Instance.GetSysModule<SysDebug>();
				
				for ( int i = 0; i < dbg.GetCmdCount(); i ++ )
				{
					IConsoleCommand cmd = dbg.GetCmdByIndex( i );
					if ( cmd is CmdHelp )
						continue;
					
					desc += "\n" + cmd.Name + "    " + cmd.Description;
				}
				
				return desc;
			}
		}
		
		public bool Execute(string[] parameters)
		{
			DebugConsole.AddOutputString( Name + ": \n" + Description );
			
			return true;
		}
	}
	
//	/// <summary>
//	/// Request command. 
//	/// </summary>
//	protected class CmdRequest : IConsoleCommand
//	{
//		public string Name { get { return "Request"; } }
//		public string Description { get { return "Log request or response to server."; } }
//		public bool Execute(string[] parameters)
//		{
//			RequestMgr.EnableLog = !RequestMgr.EnableLog;
//			DebugConsole.AddOutputString( Name + " : " + RequestMgr.EnableLog );
//			return true;
//		}
//	}
	
	/// <summary>
	/// Command base with parameters. 
	/// </summary>
	protected abstract class ParamCmd : IConsoleCommand
	{
		protected delegate bool CmdDo();
		protected class CmdPrm
		{
			public CmdPrm( string prm, string dsc, CmdDo doFun )
			{
				param = prm;
				desc = dsc;
				cmdDo = doFun;
			}
			
			public string param;
			public string desc;
			public CmdDo cmdDo;
		}
		
		public virtual string Name { get { return "ParamCmd"; } }
		public virtual string Description 
		{
			get 
			{
				string desc = "";
				
				foreach ( CmdPrm prm in prms )
					desc += " [" + prm.param + "] ";
				
				desc += "\n";
				
				foreach ( CmdPrm prm in prms )
					desc += String.Format( "     {0,-30}    {1}\n", prm.param, prm.desc );
				
				return desc;
			} 
		}
		
		public virtual bool Execute(string[] parameters)
		{
			if ( parameters.Length == 1 )
			{	
				DebugConsole.AddOutputString( Name + "\n" + Description );
				return false;
			}
			
			for ( int i = 1; i < parameters.Length; i ++ )
			{
				string inPrm = parameters[i];
					
				foreach ( CmdPrm prm in prms )
				{
					if ( prm.param.ToLower() == inPrm.ToLower() )
					{
						DebugConsole.AddOutputString( inPrm + " : " + prm.cmdDo() );
						break;
					}
				}
			}
			
			return true;
		}
		
		protected List<CmdPrm> prms = new List<CmdPrm>();
	}
	
	///// <summary>
	///// Game main debug command. 
	///// </summary>
	//protected class CmdGameMain : ParamCmd
	//{
	//    public override string Name { get { return "GameMain"; } }
	//    public override string Description { get { return "Log GameMain information.\nGameMain " + base.Description; } }
		
	//    public CmdGameMain()
	//    {
	//        prms.Add( new CmdPrm( "State", "Log GameMain's state changing.", LogState ) );
	//    }
		
	//    private static bool LogState()
	//    {
	//        GameMain.LogState = !GameMain.LogState;
	//        return GameMain.LogState;
	//    }
	//}
	
	///// <summary>
	///// Battle debug command. 
	///// </summary>
	//protected class CmdBattle : ParamCmd
	//{
	//    public override string Name { get { return "Battle"; } }
	//    public override string Description { get { return "Log battle information.\nBattle " + base.Description; } }
		
	//    public CmdBattle()
	//    {
	//        prms.Add( new CmdPrm( "ActionRecord", "Log battle's action record.", LogActRcd ) );
	//        prms.Add( new CmdPrm( "Step", "Log battle's playing step.", LogStep ) );
	//        prms.Add( new CmdPrm( "Event", "Log battle's event process.", LogEvent ) );
	//        prms.Add( new CmdPrm( "SaveBattle", "Save battle data to local.", SaveBattle ) );
	//    }
		
	//    private static bool LogActRcd()
	//    {
	//        SysBtPlayer.LogActRcd = !SysBtPlayer.LogActRcd;
	//        return SysBtPlayer.LogActRcd;
	//    }
		
	//    private static  bool LogStep()
	//    {
	//        SysBtPlayer.LogStep = !SysBtPlayer.LogStep;
	//        return SysBtPlayer.LogStep;
	//    }
		
	//    private static bool LogEvent()
	//    {
	//        SysBtPlayer.LogEvent = !SysBtPlayer.LogEvent;
	//        return SysBtPlayer.LogEvent;
	//    }
		
	//    private static bool SaveBattle()
	//    {
	//        SysBtPlayer.SaveBattle = !SysBtPlayer.SaveBattle;
	//        return SysBtPlayer.SaveBattle;
	//    }
	//}
	
	///// <summary>
	///// Role command. 
	///// </summary>
	//protected class CmdRole : ParamCmd
	//{
	//    public override string Name { get { return "Role"; } }
	//    public override string Description { get { return "Log role information.\nRole " + base.Description; } }
		
	//    public CmdRole()
	//    {
	//        prms.Add( new CmdPrm( "State", "Log role's state changing.", LogState ) );
	//        prms.Add( new CmdPrm( "CmbState", "Log role's combat state changing.", LogCmbState ) );
	//        prms.Add( new CmdPrm( "Delay", "Log role's delay objects.", LogDly ) );
	//        prms.Add( new CmdPrm( "Weapon", "Log role's weapon changing.", LogWpn ) );
	//        prms.Add( new CmdPrm( "Action", "Log role's action changing.", LogAction ) );
	//        prms.Add( new CmdPrm( "Move", "Log role's movition changing.", LogMove ) );
	//        prms.Add( new CmdPrm( "Buf", "Log role's buff changing.", LogBuf ) );
	//    }
		
	//    private static bool LogState()
	//    {
	//        BattleRole.LogState = !BattleRole.LogState;
	//        return BattleRole.LogState;
	//    }
		
	//    private static  bool LogCmbState()
	//    {
	//        BattleRole.LogCmbState = !BattleRole.LogCmbState;
	//        return BattleRole.LogCmbState;
	//    }
		
	//    private static bool LogDly()
	//    {
	//        BattleRole.LogDly = !BattleRole.LogDly;
	//        return BattleRole.LogDly;
	//    }
		
	//    private static bool LogWpn()
	//    {
	//        BattleRole.LogWpn = !BattleRole.LogWpn;
	//        return BattleRole.LogWpn;
	//    }
		
	//    private static bool LogAction()
	//    {
	//        Role.LogAction = !Role.LogAction;
	//        return Role.LogAction;
	//    }
		
	//    private static bool LogMove()
	//    {
	//        Role.LogMove = !Role.LogMove;
	//        return Role.LogMove;
	//    }
		
	//    private static bool LogBuf()
	//    {
	//        BattleRole.LogBuf = !BattleRole.LogBuf;
	//        return BattleRole.LogBuf;
	//    }
	//}
	
	///// <summary>
	///// Avatar command. 
	///// </summary>
	//protected class CmdAvatar : ParamCmd
	//{
	//    public override string Name { get { return "Avatar"; } }
	//    public override string Description { get { return "Log avatar information.\nAvatar " + base.Description; } }
		
	//    public CmdAvatar()
	//    {
	//        prms.Add( new CmdPrm( "Animation", "Log avatar's animation changing.", LogAnim ) );
	//    }
		
	//    private static bool LogAnim()
	//    {
	//        Avatar.LogAnim = !Avatar.LogAnim;
	//        return Avatar.LogAnim;
	//    }
	//}
	
	public override bool Initialize()
	{	
		DebugConsole.Initialize();
		
		//allCmds.Add( new CmdGameMain() );
		allCmds.Add( new CmdHelp() );
//		allCmds.Add( new CmdRequest() );
		//allCmds.Add( new CmdBattle() );
		//allCmds.Add( new CmdRole() );
		//allCmds.Add( new CmdAvatar() );
		
		// Register command
		foreach ( IConsoleCommand cmd in allCmds )
			DebugConsole.RegisterCommand( cmd );
		
		//DebugConsole.Active = true;
		DebugConsole.AutoScroll = false;
		
		if ( !Application.isEditor )
			DebugConsole.ListenLogOutput = true;
		
		return true;
	}
	
	public override void Dispose()
	{
		allCmds.Clear();
	}
	
	public override void Run( object usd )
	{	
	}
	
	public override void OnUpdate()
	{
		DebugConsole.Update();
	}
	
	public override void OnGUIUpdate()
	{
		DebugConsole.OnGUI();
	}
	
	// Get registered commands count.
	public int GetCmdCount()
	{
		return allCmds.Count;
	}
	
	// Get registered command by index.
	public IConsoleCommand GetCmdByIndex( int index )
	{
		return allCmds[index];
	}
	
	protected List<IConsoleCommand> allCmds = new List<IConsoleCommand>();
}


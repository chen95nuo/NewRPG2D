using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class LifeBar : MonoBehaviour 
{
    public UIProgressBar barMain;
    public UIProgressBar barDnm;
    public UIProgressBar barPower;

    private float dnmDur = 0.5f;
    private float dnmDurTimer;
    private float dnmDltVal;
	private float spValue;
	// Use this for initialization
	void Start () 
	{
        dnmDurTimer = dnmDur;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( barMain == null || barDnm == null )
			return;
		
		if ( dnmDurTimer <= 0.0f )
			return;
		
		dnmDurTimer -= Time.deltaTime;
		barDnm.Value += dnmDltVal * Time.deltaTime;
		
		if ( dnmDurTimer <= 0.0f )
		{
			barDnm.Value = barMain.Value;
			dnmDurTimer = 0.0f;
		}
	}
	
	public float Value
	{
		get { return barMain.Value; }
		set
		{
			if (barMain.Value <= value)
			{
				barMain.Value = value;
				barDnm.Value = value;
				
				dnmDurTimer = 0;
			}
			else
			{
                dnmDurTimer = dnmDur;
				barMain.Value = value;
				
				dnmDltVal = ( barMain.Value - barDnm.Value ) / dnmDurTimer;
			}
		}
	}

    public float BarPowerValue
    {
        get { return barPower.Value; }
        set
        {
			spValue = value;
            barPower.Value = value / ConfigDatabase.DefaultCfg.GameConfig.combatSetting.maxSkillPower;
        }
    }

	public float SPValue
	{
		get { return spValue; }
	}
	
	public void Hide(bool tf)
	{
		if (barMain == null || barDnm == null || barPower == null)
		{
			return;
		}

		barMain.Hide(tf);
		barDnm.Hide(tf);
        barPower.Hide(tf);
	}
}

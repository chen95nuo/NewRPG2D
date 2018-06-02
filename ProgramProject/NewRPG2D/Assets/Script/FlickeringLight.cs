using UnityEngine;
using System.Collections;
using Assets.Script.AudioMgr;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour {

    // Flickering Styles
    public enum flickerinLightStyles { CampFire = 0, Fluorescent = 1 , Thunder = 2};
    public flickerinLightStyles flickeringLightStyle = flickerinLightStyles.CampFire;
    // Campfire Methods
    public enum campfireMethods { Intensity = 0, Range = 1, Both = 2 };
    public campfireMethods campfireMethod = campfireMethods.Intensity;
    // Intensity Styles
    public enum campfireIntesityStyles { Sine = 0, Random = 1 };
    public campfireIntesityStyles campfireIntesityStyle = campfireIntesityStyles.Random;
    // Range Styles
    public enum campfireRangeStyles { Sine = 0, Random = 1 };
    public campfireRangeStyles campfireRangeStyle = campfireRangeStyles.Random;
    // Base Intensity Value
    public float CampfireIntensityBaseValue = 0.5f;
    // Intensity Flickering Power
    public float CampfireIntensityFlickerValue = 0.1f;
    // Base Range Value
    public float CampfireRangeBaseValue = 10.0f;
    // Range Flickering Power
    public float CampfireRangeFlickerValue = 2.0f;
    // If Style is Sine
    private float CampfireSineCycleIntensity = 0.0f;
    private float CampfireSineCycleRange = 0.0f;
    // "Glow" Speeds
    public float CampfireSineCycleIntensitySpeed = 5.0f;
    public float CampfireSineCycleRangeSpeed = 5.0f;
    public float FluorescentFlickerMin = 0.4f;
    public float FluorescentFlickerMax = 0.5f;
    public float FluorescentFlicerPercent = 0.95f;

    public float ThunderFlickerTime = 0.1f;
    public float thunderTime;
    public float thunderDelay;
    public float intervalMax=0.12f;
    public float intervalMin=0.08f;

    // NOT IMPLEMENTED YET !!!!
    public bool FluorescentFlickerPlaySound = false;
    public AudioClip FluorescentFlickerAudioClip;
    public SpriteRenderer mSpriteRenderer;
    public Color lightColor;
  

    private bool onThunder;
    private float timer;
    private bool filp = true;
    // ------------------------
    // Use this for initialization
    void Start()
    {
        timer = Time.time;
      //  StartThunder(1);
    }
    // Update is called once per frame
    void Update()
    {
        switch (flickeringLightStyle)
        {
            // If Flickering Style is Campfire
            case flickerinLightStyles.CampFire:
                // If campfire method is Intesity OR Both
                if (campfireMethod == campfireMethods.Intensity || campfireMethod == campfireMethods.Both)
                {
                    // If Intensity style is Sine
                    if (campfireIntesityStyle == campfireIntesityStyles.Sine)
                    {
                        // Cycle the Campfire angle
                        CampfireSineCycleIntensity += CampfireSineCycleIntensitySpeed;
                        if (CampfireSineCycleIntensity > 360.0f) CampfireSineCycleIntensity = 0.0f;
                        // Base + Values
                        GetComponent<Light>().intensity = CampfireIntensityBaseValue + ((Mathf.Sin(CampfireSineCycleIntensity * Mathf.Deg2Rad) * (CampfireIntensityFlickerValue / 2.0f)) + (CampfireIntensityFlickerValue / 2.0f));
                    }
                    else GetComponent<Light>().intensity = CampfireIntensityBaseValue + Random.Range(0.0f, CampfireIntensityFlickerValue);
                }
                // If campfire method is Range OR Both
                if (campfireMethod == campfireMethods.Range || campfireMethod == campfireMethods.Both)
                {
                    // If Range style is Sine
                    if (campfireRangeStyle == campfireRangeStyles.Sine)
                    {
                        // Cycle the Campfire angle
                        CampfireSineCycleRange += CampfireSineCycleRangeSpeed;
                        if (CampfireSineCycleRange > 360.0f) CampfireSineCycleRange = 0.0f;
                        // Base + Values
                        GetComponent<Light>().range = CampfireRangeBaseValue + ((Mathf.Sin(CampfireSineCycleRange * Mathf.Deg2Rad) * (CampfireSineCycleRange / 2.0f)) + (CampfireSineCycleRange / 2.0f));
                    }
                    else GetComponent<Light>().range = CampfireRangeBaseValue + Random.Range(0.0f, CampfireRangeFlickerValue);
                }
                break;
            // If Flickering Style is Fluorescent
            case flickerinLightStyles.Fluorescent:
                if (Random.Range(0.0f, 1.0f) > FluorescentFlicerPercent)
                {
                    GetComponent<Light>().intensity = FluorescentFlickerMin;
                    // Check Audio - NOT IMPLEMENTED YET
                    if (FluorescentFlickerPlaySound)
                    {
                    }
                }
                else GetComponent<Light>().intensity = FluorescentFlickerMax;
                break;
            case flickerinLightStyles.Thunder:
                if (onThunder)
                {
                    ThunderFlickerTime = Random.Range(intervalMin, intervalMax);
                    if (Time.time - timer > ThunderFlickerTime)
                    {
                        timer = Time.time;
                        if (filp)
                        {
                            GetComponent<Light>().intensity = FluorescentFlickerMin;
                            filp = false;
                          //  Debug.Log("min");
                        }
                        else
                        {
                            GetComponent<Light>().intensity = FluorescentFlickerMax;
                            filp = true;
                          //  Debug.Log("max");
                        }      
                    }
                   
                }
                break;
            default:
                // You should not be here.
                break;
        }

        if (mSpriteRenderer) mSpriteRenderer.color = new Color(mSpriteRenderer.color.r, mSpriteRenderer.color.g, mSpriteRenderer.color.b, GetComponent<Light>().intensity / CampfireRangeBaseValue);
    }

    public void StartThunder() {
        onThunder = true;
        GetComponent<Light>().color = lightColor;
        Invoke("DelayAudio",thunderDelay);      
        Invoke("StopThunder",thunderTime);    
    }
    void DelayAudio() {
        //Assets.Script.AudioMgr.AudioControl.GetInstance().PlayAudioAtWorld(new AudioSetting(FluorescentFlickerAudioClip, 0, Assets.Script.AudioMgr.AudioControl.GetInstance().audioVolume), PlayerCreator.Instance.transform.position);
    }
    public void StopThunder() {
        onThunder = false;
        GetComponent<Light>().intensity = 0;
    }

}

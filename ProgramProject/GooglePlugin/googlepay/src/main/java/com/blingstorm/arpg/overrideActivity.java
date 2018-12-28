package com.blingstorm.arpg;
 
import com.unity3d.player.UnityPlayerActivity;
 
import android.os.Bundle;
import android.util.Log;
import android.content.Intent;
 
public class overrideActivity extends UnityPlayerActivity {
    public interface cbEvent{
        public boolean cbEvent(int requestCode, int resultCode, Intent data);
    }
 
    protected cbEvent ie;
    static protected overrideActivity inst;
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        inst =this;
         
        // print debug message to logcat
        Log.d("overrideActivity", "onCreate called!");
    }
 
    @Override
 public void onDestroy(){
        super.onDestroy();
        inst =null;
        Log.d("overrideActivity", "onDestroy called!");
    }
 
    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data){
        Log.d("overrideActivity", "onActivityResult called!");
         
        boolean ret =false;
        if (ie !=null){
            try{
                ret =ie.cbEvent(requestCode, resultCode, data);
            }
            catch(Exception e){
                ret =false;
            }
        }
 
        if (ret ==false){
            super.onActivityResult(requestCode, resultCode, data);
        }
    }
     
    static public void registerOnActivityResultCBFunc(final cbEvent pcbfunc){
        if (inst !=null)
            inst.ie =pcbfunc;
    }
} 
using UnityEngine;
using System.IO;
using System.Text;
namespace admob {
    [System.Serializable]
    public class AdProperties {
        public bool isForChildDirectedTreatment=false;
        public bool isUnderAgeOfConsent=false;
        public bool isAppMuted=false;
        public bool isTesting=false;
        public bool nonPersonalizedAdsOnly=false;
        public int appVolume=100;
        public string maxAdContentRating;//value can been null or one of G, PG, T, MA,
        public string[] keywords;
        public string toString() {
            string result=JsonUtility.ToJson(this);
            return result;
        }
    }
}

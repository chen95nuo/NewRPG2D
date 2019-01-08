using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
using System.Xml;
#endif
using System.IO;

public static class XCodePostProcess
{
	static string path = "";
    #if UNITY_EDITOR
    [PostProcessBuild (100)]
    public static void OnPostProcessBuild (BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iPhone) {
            Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
            return;
        }

        //得到xcode工程的路径
        path = Path.GetFullPath (pathToBuiltProject);

        // Create a new project object from build target
        XCProject project = new XCProject (pathToBuiltProject);

        // Find and run through all projmods files to patch the project.
        // Please pay attention that ALL projmods files in your project folder will be excuted!
        //在这里面把frameworks添加在你的xcode工程里面
        string[] files = Directory.GetFiles (Application.dataPath, "*.projmods", SearchOption.AllDirectories);
        foreach (string file in files) {
            project.ApplyMod (file);
        }

        //增加一个编译标记。。没有的话sharesdk会报错。。
//        project.AddOtherLinkerFlags("-licucore");
//SDK_JYIOS
//		project.AddOtherLinkerFlags("-ObjC");

//SDK_TONGBU
//		project.AddOtherLinkerFlags("-ObjC");
//		project.AddOtherLinkerFlags("-lz");
//		project.overwriteBuildSetting ("CODE_SIGN_IDENTITY", "iPhone Developer: yang long (RW9JAQ2442)", "Release");
//		project.overwriteBuildSetting ("CODE_SIGN_IDENTITY", "iPhone Developer: yang long (RW9JAQ2442)", "Debug");

//SDK_XY
//		project.AddOtherLinkerFlags("-ObjC");
//		project.AddOtherLinkerFlags("-lz");

//SDK_ZSY
//		project.AddOtherLinkerFlags("-ObjC");
//		project.AddOtherLinkerFlags("-licucore");

//SDK_HM
//		project.AddOtherLinkerFlags("-ObjC");

//SDK_I4
		//have no otherLinkerFlags
//SDK_itools
		//have no otherLinkerFlags
//SDK_KUAIYONG
		//have no otherLinkerFlags
//SDK_PP
		//have no otherLinkerFlags


		project.overwriteBuildSetting ("CODE_SIGN_IDENTITY", "iPhone Developer: kaihong zhou (TB85AS2SAW)", "Release");
		project.overwriteBuildSetting ("CODE_SIGN_IDENTITY", "iPhone Developer: kaihong zhou (TB85AS2SAW)", "Debug");

		//        // 编辑plist 文件
//        EditorPlist(path);
        //编辑代码文件
        EditorCode(path);
        // Finally save the xcode project
        project.Save ();
        if(projectName== "91")
        {
             //当我们在打91包的时候 这里面做一些 操作。

        }

    }

	public static string projectName
	{
		get
		{
			foreach(string arg in System.Environment.GetCommandLineArgs()) {
				Debug.Log("project name = " + arg.StartsWith("project"));
				if(arg.StartsWith("project"))
				{
					return arg.Split("-"[0])[1];
				}
			}
			return "test";
		}
	}
    private static void EditorPlist(string filePath)
    {
     
        XCPlist list =new XCPlist(filePath);
		string bundle = "com.zealm.${PRODUCT_NAME";

        string PlistAdd = @"  
            <key>CFBundleURLTypes</key>
            <array>
            <dict>
            <key>CFBundleTypeRole</key>
            <string>Editor</string>
            <key>CFBundleURLIconFile</key>
            <string>Icon@2x</string>
            <key>CFBundleURLName</key>
            <string>"+bundle+@"</string>
            <key>CFBundleURLSchemes</key>
            <array>
            <string>ww123456</string>
            </array>
            </dict>
            </array>";
        
        //在plist里面增加一行
        list.AddKey(PlistAdd);
        //在plist里面替换一行
		list.ReplaceKey("<string>com.zealm.${PRODUCT_NAME}</string>","<string>"+bundle+"</string>");
        //保存
        list.Save();

    }

    private static void EditorCode(string filePath)
    {
		//读取UnityAppController.mm文件
        XClass UnityAppController = new XClass(filePath + "/Classes/UnityAppController.mm"); 
//SDK_JYIOS
		//在指定代码后面增加一行代码
//		UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"","#import <NdComPlatform/NdComPlatform.h>");
//		//在指定代码后面增加一行
//		UnityAppController.WriteBelow("- (void)applicationDidEnterBackground:(UIApplication *)application\n{","[[NdComPlatform defaultPlatform] NdPause];\n");

//SDK_TONGBU
//		have no EditorCode
//SDK_XY
//		UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"","#import <XYPlatform/XYPlatform.h>");
//		UnityAppController.WriteBelow ("- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions\n{","[XYPlatform defaultPlatform];");
//		UnityAppController.WriteBelow ("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);","[[XYPlatform defaultPlatform] XYHandleOpenURL:url];\n");
//		UnityAppController.WriteBelow ("- (NSUInteger)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window\n{","[[XYPlatform defaultPlatform] application:application supportedInterfaceOrientationsForWindow:window];\n");
//		UnityAppController.WriteBelow ("UnitySendRemoteNotificationError(error);\n}","-(BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url\n{\n[[XYPlatform defaultPlatform] XYHandleOpenURL:url];\nreturn YES;\n}");

//SDK_ZSY
//		UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"","#import <Cmge/Cmge.h>\n#import \"ZSYSDKTools.h\"");
//		UnityAppController.WriteBelow ("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);","[[CmgePlatform defaultPlatform] application:application openURL:url sourceApplication:sourceApplication annotation:annotation];\n");
//		UnityAppController.WriteBelow ("UnityInitApplicationNoGraphics([[[NSBundle mainBundle] bundlePath]UTF8String]);","[[ZSYSDKTools alloc] ZSYInit:launchOptions];");

//SDK_HM
//		UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"","#import <IPAYiAppPay.h>");
//		UnityAppController.WriteBelow ("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);","[[IPAYiAppPay sharediAppPay] handleOpenurl:url];\n");
//		UnityAppController.WriteBelow ("UnitySendRemoteNotificationError(error);\n}","-(BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url\n{\n[[IPAYiAppPay sharediAppPay] handleOpenurl:url];\nreturn YES;\n}");

//SDK_I4
//		UnityAppController.WriteBelow ("#include \"PluginBase/AppDelegateListener.h\"","#import \"AsInfoKit.h\"");
//		UnityAppController.WriteBelow ("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);","if ([sourceApplication isEqualToString:@\"com.alipay.iphoneclient\"])\n{\n [[AsInfoKit sharedInstance] alixPayResult:url];\n}\nelse if ([sourceApplication isEqualToString:@\"com.alipay.safepayclient\"])\n{\n[[AsInfoKit sharedInstance] alixPayResult:url];\n}\nelse if ([sourceApplication isEqualToString:@\"com.tencent.xin\"])\n{\n[[AsInfoKit sharedInstance] weChatPayResult:url];\n}\n");

//SDK_itools
		//		have no EditorCode

//SDK_KUAIYONG
//		UnityAppController.WriteBelow ("#include \"PluginBase/AppDelegateListener.h\"","#import <AlipaySDK/AlipaySDK.h>");
//		UnityAppController.WriteBelow ("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);"," if ([url.host isEqualToString:@\"safepay\"]) {\n[[AlipaySDK defaultService] processOderWithPaymentResult:url];\n}");

//SDK_PP
//		UnityAppController.WriteBelow ("#include \"PluginBase/AppDelegateListener.h\"","#import <PPAppPlatformKit/PPAppPlatformKit.h>");
//		UnityAppController.WriteBelow ("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);","[[PPAppPlatformKit sharedInstance] alixPayResult:url];\n");





		//在指定代码中替换一行
//        UnityAppController.Replace("return YES;","return [ShareSDK handleOpenURL:url sourceApplication:sourceApplication annotation:annotation wxDelegate:nil];");

		XClass UnityViewControllerBase = new XClass (filePath + "/Classes/UI/UnityViewControllerBase.h");
		UnityViewControllerBase.WriteBelow ("#import <UIKit/UIKit.h>", "#import \"MBProgressHUD.h\"");
		UnityViewControllerBase.WriteBelow ("- (void)didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation;", "-(void)showSimple;");

		XClass UnityViewControllerBasem = new XClass (filePath + "/Classes/UI/UnityViewControllerBase.mm");
		UnityViewControllerBasem.WriteBelow ("AddStatusBarSupportDefaultImpl(targetClass);\n}","@interface UnityViewControllerBase () <MBProgressHUDDelegate> {\r  MBProgressHUD *HUD;\r long long expectedLength;\r long long currentLength;\r}\r@end");
		UnityViewControllerBasem.WriteBelow ("[UIView setAnimationsEnabled:YES];\n}", "-(void)showSimple \r{\r HUD = [[MBProgressHUD alloc] initWithView:UnityGetGLView()];\r [UnityGetGLView() addSubview:HUD];\r [UnityGetGLView() bringSubviewToFront:HUD];\r // Regiser for HUD callbacks so we can remove it from the window at the right time \r HUD.delegate = self;\r // Show the HUD while the provided method executes in a new thread \r [HUD showWhileExecuting:@selector(myTask) onTarget:self withObject:nil animated:YES];\r}\r - (void)myTask {\r // Do something usefull in here instead of sleeping ... \r sleep(1);\r}");

    }

    #endif
}

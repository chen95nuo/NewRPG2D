using UnityEngine;
using System.Collections;

/**向服务器发送请求的类都必须继承此接口,用于处理返回结果**/
public interface ProcessResponse
{
	void receiveResponse(string json);
}

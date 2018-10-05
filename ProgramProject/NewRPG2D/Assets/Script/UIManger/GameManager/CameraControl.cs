using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;

    private Vector2 v;
    private Camera m_Camera;
    private Vector3 m_CameraPosition;

    public float MoveSpeed = 1f;
    public float dragSpeed = 1f;
    public float scaleSpeed = 1f;

    private Vector3 oldMousePosition;
    private Touch oldTouch0;
    private Touch oldTouch1;
    private Touch oldTouch2;

    public float zMin = 5.4f;
    public float zMax = 28f;

    private float a = 0;
    private float b = 0;

    private bool isMove = false; //移动了
    private float currentTime = 0;
    private float testTime = 0;

    private RoomMgr room;
    private bool moveRoomType = false; //判断放大缩小
    private bool moving = false; //移动中
    public bool isShowEdit = false;//是否显示建造提示

    private void Awake()
    {
        instance = this;

        m_Camera = GetComponent<Camera>();
        m_CameraPosition = m_Camera.transform.position;

        GetSpeed();
    }

    private void Update()
    {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        PCMove();
#elif UNITY_ANDROID
        AndroidMove();
#elif UNITY_IPHONE

#endif

        if (isMove == true)
        {
            HallEventManager.instance.SendEvent(HallEventDefineEnum.CameraMove);
        }

        isMove = false;
    }

    private void AndroidMove()
    {
        if (Input.touchCount != 0)
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    oldTouch0 = Input.GetTouch(0);
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    isMove = true;//镜头移动了
                    moving = false;//如果主动移动镜头关闭自动移动

                    v.x = Input.GetTouch(0).deltaPosition.x * Time.deltaTime;
                    v.y = Input.GetTouch(0).deltaPosition.y * Time.deltaTime;

                    this.transform.position += new Vector3(-v.x, -v.y, 0) * MoveSpeed;
                }
                //长按
                if (Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    ChickLongPress();
                }
                //点击结束时
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (isMove == false)
                    {
                        ChickClick();
                    }
                }
            }
            if (Input.touchCount > 1)
            {
                if (Input.GetTouch(1).phase == TouchPhase.Began)
                {
                    oldTouch1 = Input.GetTouch(0);
                    oldTouch2 = Input.GetTouch(1);
                    return;
                }
                //记录坐标
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    //isMove = true;

                    Touch newTouch1 = Input.GetTouch(0);
                    Touch newTouch2 = Input.GetTouch(1);
                    float touch1Dis = Vector2.Distance(newTouch1.position, oldTouch1.position) * Time.deltaTime;
                    float touch2Dis = Vector2.Distance(newTouch2.position, oldTouch2.position) * Time.deltaTime;

                    float oldDis = Vector2.Distance(oldTouch1.position, oldTouch2.position) * Time.deltaTime;
                    float newDis = Vector2.Distance(newTouch1.position, newTouch2.position) * Time.deltaTime;

                    float dis = oldDis - newDis;
                    float zPoint = Mathf.Clamp(m_Camera.orthographicSize + (dis * scaleSpeed), zMin, zMax);
                    m_Camera.orthographicSize = zPoint;
                    //Vector3 pos = new Vector3(transform.position.x, transform.position.y, zPoint);
                    //Debug.Log(pos.z);
                    //transform.position = pos;

                    oldTouch1 = newTouch1;
                    oldTouch2 = newTouch2;
                }
                MoveSpeed = (a * m_Camera.orthographicSize) + b;
            }
        }
    }

    private void PCMove()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Mouse ScrollWheel");

        if (x != 0 || y != 0 || z != 0)
        {
            isMove = true;
        }
        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize += -(z * 4.0f), zMin, zMax);
        transform.localPosition += (new Vector3(x * 0.2f, y * 0.2f, 0));
        if (Input.GetMouseButton(0) && oldMousePosition == Input.mousePosition)
        {
            if (isMove == false)
            {
                ChickLongPress();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isMove == false)
            {
                ChickClick();
            }
            isMove = true;
        }
        oldMousePosition = Input.mousePosition;
    }

    /// <summary>
    /// 关闭房间锁定
    /// </summary>
    public void CloseRoomLock()
    {
        if (room != null)
        {
            room.roomLock.SetActive(false);
            UIPanelManager.instance.ClosePage<UILockRoomTip>();
        }
        room = null;
    }

    /// <summary>
    /// 重置锁定的房间
    /// </summary>
    /// <param name="roomMgr"></param>
    public void RefreshRoomLock(RoomMgr roomMgr)
    {
        if (room != null && room == roomMgr)
        {
            UIPanelManager.instance.ClosePage<UILockRoomTip>();
            UIPanelManager.instance.ShowPage<UILockRoomTip>(roomMgr);
        }
    }

    /// <summary>
    /// 检查点击
    /// </summary>
    private void ChickClick()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) { }
        if (MapControl.instance.type == CastleType.main)
        {
            if (hit.collider == null)
            {
                if (room != null)
                {
                    room.roomLock.SetActive(false);
                    room = null;
                    UIPanelManager.instance.ClosePage<UILockRoomTip>();//关闭底侧UI
                }
                return;
            }
            if (hit.collider.tag == "Room")
            {
                ChickTouchRoom(hit);
            }
            else if (hit.collider.tag == "BuildTip")
            {
                MainCastle.instance.ChickReycast(hit);
            }
            else if (hit.collider.tag == "Role")
            {

            }
        }
        else //编辑模式点击效果
        {
            if (hit.collider == null)
            {
                if (room != null)
                {
                    room.roomLock.SetActive(false);
                    room = null;
                    //关闭提示框
                    UIEditMode.instance.ShowMenu(null);
                }
                return;
            }
            else if (hit.collider.tag == "Room")
            {
                ChickEditTouch(hit);
            }
            else if (hit.collider.tag == "BuildTip")
            {
                EditCastle.instance.ChickReycast(hit);
            }
        }
    }

    /// <summary>
    /// 检测长按 非移动状态 进入建造模式
    /// </summary>
    private void ChickLongPress()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) { }
        if (hit.collider == null) return;
        if (MapControl.instance.type == CastleType.main && isShowEdit == false
            && hit.collider.tag == "Room")
        {
            currentTime += Time.deltaTime;//建造模式计时
            if (currentTime > 0.5f)
            {
                isShowEdit = true;
                currentTime = 0;
                UIPanelManager.instance.ShowPage<UIEditModeTip>();
                return;
            }
        }
    }

    private void CameraMove(Vector3 point)
    {
        Vector3 v3 = new Vector3(transform.position.x, transform.position.y, m_Camera.orthographicSize);
        v3 = Vector3.Lerp(v3, point, .5f / (Vector3.Distance(v3, point)));
        transform.position = new Vector3(v3.x, v3.y, transform.position.z);
        m_Camera.orthographicSize = v3.z;
        MoveSpeed = (a * m_Camera.orthographicSize) + b;
    }

    /// <summary>
    /// 主城模式点击效果
    /// </summary>
    /// <param name="hit"></param>
    private void ChickTouchRoom(RaycastHit hit)
    {
        RoomMgr data = hit.collider.GetComponent<RoomMgr>();
        if (data.IsHarvest)//如果有产出那么获取产出
        {
            Debug.Log("获取产出");
            float temp = data.currentBuildData.Stock;
            ChickPlayerInfo.instance.GetProductionStock(data.currentBuildData);

            //检测资源是否被取走
            if (temp == data.currentBuildData.Stock && temp > 0)//如果资源满了 无法取走 那么更换选中房间
            {
                ChangeRoomMgr(data);
            }
        }
        if (room == null)
        {
            room = data;
            //子物体启动
            room.roomLock.SetActive(true);
            UIPanelManager.instance.ShowPage<UILockRoomTip>(room);//显示底侧UI
        }
        else if (room != data)
        {
            ChangeRoomMgr(data);
        }
        //这里是双击状态 room == data 那么根据镜头距离进行移动
        else if (m_Camera.orthographicSize > 5.4f)
        {
            moveRoomType = true;
            moving = true;
        }
        else
        {
            moveRoomType = false;
            moving = true;
        }
    }

    /// <summary>
    /// 建造模式点击效果
    /// </summary>
    /// <param name="hit"></param>
    private void ChickEditTouch(RaycastHit hit)
    {
        if (hit.collider.tag == "Room")
        {
            RoomMgr data = hit.collider.GetComponent<RoomMgr>();
            if (room != null && room != data)//更换选中房间
            {
                room.roomLock.SetActive(false);
                data.roomLock.SetActive(true);
                room = data;
                //切换提示框
                UIEditMode.instance.ShowMenu(data);
            }
            else /*(room == null)*/
            {
                room = data;
                //子物体启动
                room.roomLock.SetActive(true);
                //出现提示框
                UIEditMode.instance.ShowMenu(data);
            }
        }
        else if (hit.collider.tag == "BuildTip")
        {
            EditCastle.instance.ChickReycast(hit);
        }
    }
    /// <summary>
    /// 更换选中房间
    /// </summary>
    /// <param name="data">新选中的房间</param>
    private void ChangeRoomMgr(RoomMgr data)
    {
        UIPanelManager.instance.ShowPage<UILockRoomTip>(data);

        room.roomLock.SetActive(false);
        data.roomLock.SetActive(true);
        room = data;
    }

    /// <summary>
    /// 计算镜头距离速度比
    /// </summary>
    private void GetSpeed()
    {
        float x = 30 - 5.4f;
        float y = 2 - 0.6f;
        a = y / x;
        b = y - (x * a);
    }
}


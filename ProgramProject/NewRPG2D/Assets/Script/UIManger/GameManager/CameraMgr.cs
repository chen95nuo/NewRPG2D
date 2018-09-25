using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class CameraMgr : MonoBehaviour
{
    private Vector2 v;
    private Camera m_Camera;
    private Vector3 m_CameraPosition;

    public float MoveSpeed = 1f;
    public float dragSpeed = 1f;
    public float scaleSpeed = 1f;

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
    private bool lockRoom = false; //是否锁定房间
    private bool moving = false; //移动中
    private bool isShowEdit = false;//是否显示建造提示
    public MapType mapType;
    public MapControl Map;

    private void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.InEditMode, InTheEditMode);
        HallEventManager.instance.AddListener(HallEventDefineEnum.CloseRoomLock, ClickRoomLock);
        HallEventManager.instance.AddListener<RoomMgr>(HallEventDefineEnum.CloseRoomLock, ClickRoomLock);

        m_Camera = GetComponent<Camera>();
        m_CameraPosition = m_Camera.transform.position;

        GetSpeed();
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.InEditMode, InTheEditMode);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CloseRoomLock, ClickRoomLock);
        HallEventManager.instance.RemoveListener<RoomMgr>(HallEventDefineEnum.CloseRoomLock, ClickRoomLock);
    }

    private void Update()
    {
        if (mapType == MapType.MainMap)
        {
            MainMap();
            TouchMove();
        }
        else
        {
            EditMap();
        }
    }
    /// <summary>
    /// 主城模式
    /// </summary>
    private void MainMap()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Mouse ScrollWheel");
        HallEventManager.instance.SendEvent(HallEventDefineEnum.CameraMove);
        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize += -(z * 4.0f), zMin, zMax);
        transform.localPosition += (new Vector3(x * 0.2f, y * 0.2f, 0));
        if (Input.GetMouseButton(0))
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) { }
            if (hit.collider != null && hit.collider.tag == "Room")
            {
                currentTime += Time.deltaTime;//建造模式计时
                if (currentTime > 0.5f && isShowEdit == false)
                {
                    isShowEdit = true;
                    Debug.Log("正在进入建造计时");
                    UIPanelManager.instance.ShowPage<UIEditModeTip>();
                    return;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) { }

            if (isMove == true)
            {
            }
            else
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
                else
                {
                    if (hit.collider.tag == "Room")
                    {
                        RoomMgr data = hit.collider.GetComponent<RoomMgr>();
                        if (data.isHarvest)//如果有产出那么不更换选中房间
                        {
                            Debug.Log("获得产出");
                            data.ProductionType();
                            return;
                        }
                        if (room == null)
                        {
                            room = data;
                            //子物体启动
                            room.roomLock.SetActive(true);
                            UIPanelManager.instance.ShowPage<UILockRoomTip>();//显示底侧UI
                            HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.UILockRoomTip, data);
                        }
                        else if (room != data)//更换选中房间
                        {
                            UIPanelManager.instance.ClosePage<UILockRoomTip>();//更换UI
                            UIPanelManager.instance.ShowPage<UILockRoomTip>();
                            HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.UILockRoomTip, data);

                            room.roomLock.SetActive(false);
                            data.roomLock.SetActive(true);
                            room = data;
                        }
                        else if (m_Camera.orthographicSize > 5.4f)
                        {
                            lockRoom = true;
                            moving = true;
                        }
                        else
                        {
                            lockRoom = false;
                            moving = true;
                        }
                        isMove = false;
                        isShowEdit = false;
                        if (currentTime > 0.5f && currentTime < 1.5f)
                        {
                            Debug.Log("关闭EditModeTip");
                            UIPanelManager.instance.ClosePage<UIEditModeTip>();
                        }
                    }
                    else if (hit.collider.tag == "BuildTip")
                    {
                        HallEventManager.instance.SendEvent<RaycastHit>(HallEventDefineEnum.InEditMode, hit);
                    }
                }
            }

            currentTime = 0;
        }
    }

    /// <summary>
    /// 建造模式
    /// </summary>
    private void EditMap()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Mouse ScrollWheel");

        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize += -(z * 4.0f), zMin, zMax);
        transform.localPosition += (new Vector3(x * 0.2f, y * 0.2f, 0));
        if (Input.GetMouseButtonUp(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) { }
            if (hit.collider == null)
            {
                if (room != null)
                {
                    room.roomLock.SetActive(false);
                    room = null;
                    //关闭提示框
                    HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.EditMode, null);
                }
                return;
            }
            else
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
                        HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.EditMode, data);
                    }
                    else /*(room == null)*/
                    {
                        room = data;
                        //子物体启动
                        room.roomLock.SetActive(true);
                        //出现提示框
                        HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.EditMode, data);
                    }
                }
                else if (hit.collider.tag == "BuildTip")
                {
                    HallEventManager.instance.SendEvent<RaycastHit>(HallEventDefineEnum.InEditMode, hit);
                }
            }
        }
    }

    private void InTheEditMode() //进入建造模式
    {
        UIPanelManager.instance.ClosePage<UIMain>();
        UIPanelManager.instance.ShowPage<UIEditMode>();
    }

    /// <summary>
    /// 关闭锁定状态
    /// </summary>
    private void ClickRoomLock()
    {
        if (room != null)
        {
            room.roomLock.SetActive(false);
        }
        room = null;
    }
    /// <summary>
    /// 刷新锁定状态
    /// </summary>
    private void ClickRoomLock(RoomMgr data)
    {
        if (room != null && room == data)
        {
            UIPanelManager.instance.ClosePage<UILockRoomTip>();
            UIPanelManager.instance.ShowPage<UILockRoomTip>();
            HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.UILockRoomTip, data);
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

    private void GetSpeed()
    {
        float x = 30 - 5.4f;
        float y = 2 - 0.6f;
        a = y / x;
        b = y - (x * a);
    }

    private void TouchMove()
    {
        if (moving)
        {
            if (lockRoom)
            {
                Vector3 point = new Vector3(room.roomLock.transform.position.x, room.roomLock.transform.position.y, zMin);

                CameraMove(point);
                if (m_Camera.orthographicSize == zMin)
                {
                    moving = false;
                }
            }
            else
            {
                Vector3 point = new Vector3(room.roomLock.transform.position.x, room.roomLock.transform.position.y, zMax);
                if (m_Camera.orthographicSize == zMax)
                {
                    moving = false;
                }
                CameraMove(point);
            }
        }
        if (Input.touchCount != 0)
        {
            if (Input.touchCount == 1)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) { }

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    oldTouch0 = Input.GetTouch(0);
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    isMove = true;//移动镜头了
                    moving = false;//是否自动移动镜头

                    v.x = Input.GetTouch(0).deltaPosition.x * Time.deltaTime;
                    v.y = Input.GetTouch(0).deltaPosition.y * Time.deltaTime;

                    this.transform.position += new Vector3(-v.x, -v.y, 0) * MoveSpeed;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    if (isMove == false && hit.collider.tag == "Room")//如果没移动过切点击的目标是房间 那么进入建造模式
                    {

                        currentTime += Time.deltaTime;//建造模式计时
                        //testBuildGO.SetActive(true);
                        //testBuild.fillAmount = 1 - currentTime;

                        if (currentTime > 1.0f)
                        {
                            currentTime = 0;
                            isMove = true;
                            //testBuildGO.SetActive(false);
                            //testBuild.fillAmount = 1;
                        }
                    }
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (isMove == true)
                    {
                    }
                    else
                    {
                        if (hit.collider == null)
                        {
                            if (room != null)
                            {
                                room.roomLock.SetActive(false);
                            }
                            return;
                        }
                        RoomMgr data = hit.collider.GetComponent<RoomMgr>();
                        if (room == null)
                        {
                            //text.text = "选中房间" + hit.collider.name;
                            room = data;
                            //子物体启动
                            room.roomLock.SetActive(true);
                        }
                        else if (room != data)//更换选中房间
                        {
                            //text.text = "更换选中房间" + hit.collider.name;
                            room.roomLock.SetActive(false);
                            data.roomLock.SetActive(true);
                            //room子物体关闭 data子物体启动
                            room = data;
                        }
                        else if (m_Camera.orthographicSize > 5.4f)
                        {
                            //text.text = "锁定房间";
                            lockRoom = true;
                            moving = true;
                        }
                        else
                        {
                            //text.text = "解锁房间";
                            lockRoom = false;
                            moving = true;
                        }
                    }
                    isMove = false;
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
}


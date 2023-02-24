using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIFollowMainCameraRotation : MonoBehaviour
{
    public Transform relativeObj;
    Vector3 relativeObjOrignal;
    Vector3 objOrignal;
    void Start()
    {
        if(relativeObj == null) relativeObj = transform.parent;

        relativeObjOrignal = relativeObj.localPosition;//相对对象的初始位置
        objOrignal = transform.localPosition;//UI初始位置
    }

    
    void LateUpdate()
    {
        //transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);//旋转
        if (Camera.main)
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);//旋转
        //（移动距离 + 移动方向） + 初始位置= 新的位置
        transform.localPosition = (relativeObj.localPosition - relativeObjOrignal) + objOrignal;
    }
}

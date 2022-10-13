#define PC
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VRInput
{
#if PC
    public enum ButtonTarget
    {
        Fire1,
        Fire2,
        Fire3,
        Jump,
    }
#endif
    public enum Button
    {
#if PC
        One = ButtonTarget.Fire1,
        Two = ButtonTarget.Jump,
        Thumbstick = ButtonTarget.Fire1,
        IndexTrigger = ButtonTarget.Fire3,
        HandTrigger = ButtonTarget.Fire2,
#endif  
    }
    public enum Controller
    {
#if PC
        LTouch,
        RTouch,
#endif
    }

    public static Vector3 RHandPosition
    {
        get
        {
#if PC
            Vector3 pos = Input.mousePosition;
            pos.z = 0.7f;  // 임의의 값
            pos = Camera.main.ScreenToWorldPoint(pos);
            RHand.position = pos;
            return pos;
#endif
        }
    }
    public static Vector3 LHandPosition
    {
        get
        {
#if PC
            Vector3 pos = Input.mousePosition;
            pos.z = 0.7f;  // 임의의 값
            pos = Camera.main.ScreenToWorldPoint(pos);
            LHand.position = pos;
            return pos;
#endif
        }
    }

    public static Vector3 RHandDirection
    {
        get
        {
#if PC
            Vector3 direction = RHandPosition - Camera.main.transform.position;
            RHand.forward = direction;
            return direction;
#endif
        }
    }
    public static Vector3 LHandDirection
    {
        get
        {
#if PC
            Vector3 direction = LHandPosition - Camera.main.transform.position;
            RHand.forward = direction;
            return direction;
#endif
        }
    }

    private static Transform lHand;
    public static Transform LHand
    {
        get
        {
            if (lHand == null)
            {
#if PC
                GameObject handObj = new GameObject("LHand");
                lHand = handObj.transform;
                lHand.parent = Camera.main.transform;
#endif
            }
            return lHand;
        }
    }

    private static Transform rHand;
    public static Transform RHand
    {
        get
        {
            if (rHand == null)
            {
#if PC
                GameObject handObj = new GameObject("RHand");
                rHand = handObj.transform;
                rHand.parent = Camera.main.transform;
#endif
            }
            return rHand;
        }
    }

    public static bool Get(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButton(((ButtonTarget)virtualMask).ToString());
#endif
    }
    public static bool GetDown(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonDown(((ButtonTarget)virtualMask).ToString());
#endif
    }
    public static bool GetUp(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonUp(((ButtonTarget)virtualMask).ToString());
#endif
    }

    public static float GetAxis(string axis, Controller hand = Controller.LTouch)
    {
#if PC
        return Input.GetAxis(axis);
#endif
    }

    public static void PlayVibration(Controller hand)
    {
        // PC로는 진동 구현 불가능
    }

#if PC
    static Vector3 originScale = Vector3.one * 0.02f;
#endif
    public static void DrawCrosshair(Transform crosshair, bool isHand = true, Controller hand = Controller.RTouch)
    {
        Ray ray;
        if(isHand)
        {
#if PC
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif
        }
        else
        {
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        Plane plane = new Plane(Vector3.up, 0);
        float distance = 0;

        if(plane.Raycast(ray, out distance))
        {
            crosshair.position = ray.GetPoint(distance);
            crosshair.forward = Camera.main.transform.forward;
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
        else
        {
            crosshair.position = ray.origin + ray.direction * 100;
            crosshair.forward = -Camera.main.transform.forward;
            distance = (crosshair.position - ray.origin).magnitude;
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
    }
}

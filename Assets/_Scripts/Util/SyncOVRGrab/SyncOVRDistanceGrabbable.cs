/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


using System;
using UnityEngine;
using OVRTouchSample;

/* 
 * OVRDistanceGrabbable을 사용하기 위하여 만든 스크립트.
 * SyncOVRGrabbable을 상속받으며, 기존의 DistanceGrabbable에 있던 Crosshair는 사용하지 않음.
 */
public class SyncOVRDistanceGrabbable : SyncOVRGrabbable
{
    public string m_materialColorField;


    Renderer m_renderer;
    MaterialPropertyBlock m_mpb;


    public bool InRange
    {
        get { return m_inRange; }
        set
        {
            m_inRange = value;
            // RefreshCrosshair();
        }
    }
    bool m_inRange;

    public bool Targeted
    {
        get { return m_targeted; }
        set
        {
            m_targeted = value;
            // RefreshCrosshair();
        }
    }
    bool m_targeted;

    protected override void Start()
    {
        base.Start();
        //m_crosshair = gameObject.GetComponentInChildren<GrabbableCrosshair>();
        //m_renderer = gameObject.GetComponent<Renderer>();
        //m_crosshairManager = FindObjectOfType<GrabManager>();
        //m_mpb = new MaterialPropertyBlock();
        //RefreshCrosshair();
        //m_renderer.SetPropertyBlock(m_mpb);
    }
}
 //    void RefreshCrosshair()
 //    {
 //        if (m_crosshair)
 //        {
 //            if (isGrabbed) m_crosshair.SetStat(GrabbableCrosshair.CrosshairState.Disabled);
 //            else if (!InRange) m_crosshair.SetStat(GrabbableCrosshair.CrosshairState.Disabled);
 //            else m_crosshair.SetState(Targeted ?GrabbableCrosshair.CrosshairState.Targeted :GrabbableCrosshair.CrosshairState.Enabled);
 //        }
 //        if (m_materialColorField != null)
 //        {
 //            m_renderer.GetPropertyBlock(m_mpb);
 //            if (isGrabbed || !InRange) m_mpb.SetColor(m_materialColorField,m_crosshairManager.OutlineColorOutOfRange);
 //            else if (Targeted) m_mpb.SetColor(m_materialColorField,m_crosshairManager.OutlineColorHighlighted);
 //            else m_mpb.SetColor(m_materialColorField,m_crosshairManager.OutlineColorInRange);
 //            m_renderer.SetPropertyBlock(m_mpb);
 //        }
 //    }
 //}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _UI = Defines.ECharacterUIIndex;

public class CharacterUIManager : MonoBehaviour
{
    [SerializeField] GameObject[] UI;

    private void Awake()
    {
        LoadUI(_UI.SELECT);
    }

    private void ShutUI()
    {
        foreach (GameObject ui in UI)
        {
            ui.SetActive(false);
        }
    }

    // ECharacterUIIndex를 매개변수로 받아, ui 오브젝트를 모두 비활성화한 후 인덱스에 해당하는 ui 오브젝트를 활성화한다.
    public void LoadUI(_UI ui)
    {
        ShutUI();
        UI[(int)ui].SetActive(true);
    }
}

using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    [SerializeField] protected GameObject[] _ui;

    protected void ShutUI()
    {
        foreach (GameObject ui in _ui)
        {
            ui.SetActive(false);
        }
    }

    /// <summary>
    /// int ui 인덱스를 매개변수로 받아, UI 오브젝트를 모두 비활성화한 후 인덱스에 해당하는 UI 오브젝트를 활성화한다.
    /// </summary>
    /// <param name="ui"></param>
    public void LoadUI(int ui)
    {
        ShutUI();
        _ui[ui].SetActive(true);
    }
}

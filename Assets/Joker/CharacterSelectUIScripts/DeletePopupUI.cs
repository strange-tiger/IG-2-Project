using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;



public class DeletePopupUI : PopupUI
{
    [SerializeField] Button _deleteButton;

    protected new void OnEnable()
    {
        base.OnEnable();
        _deleteButton.onClick.RemoveListener(DeleteCharacter);
        _deleteButton.onClick.AddListener(DeleteCharacter);
    }

    // 현재 캐릭터의 닉네임 받아와야 함
    private void DeleteCharacter()
    {
        MySqlSetting.DeleteRowByBase(EAccountInfoColumns.Nickname, "");
    }

    protected new void OnDisable()
    {
        base.OnDisable();
        _deleteButton.onClick.RemoveListener(DeleteCharacter);
    }
}

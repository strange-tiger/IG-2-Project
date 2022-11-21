using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using _CSV = Asset.ParseCSV.CSVParser;

public class InputTutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _conversationText;

    private List<string> _conversationList = new List<string>();
    private int _indexNum = -1;
    private bool _conversationEnd = true;
    private Coroutine ConversationCoroutine;
    void Start()
    {
        _conversationList = _CSV.ParseCSV("InputTutorial", _conversationList);

        for(int i = 0; i < _conversationList.Count; ++i)
        {
            Debug.Log(_conversationList[i]);
        }

    }

    private void Update()
    {
        if(_conversationEnd == false)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                StopCoroutine(ConversationCoroutine);
                _conversationText.text = _conversationList[_indexNum];
                _conversationEnd = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                _conversationText.text = null;
                ++_indexNum;
                _conversationEnd = false;

                ConversationCoroutine = StartCoroutine(ConversationPrint());

            }
        }
        Debug.Log(_conversationText.text);
        Debug.Log(_indexNum);
    }
    private IEnumerator ConversationPrint()
    {
        for(int i = 0; i < _conversationList[_indexNum].Length; ++i)
        {
            yield return new WaitForSeconds(0.1f);

            _conversationText.text += _conversationList[_indexNum][i];
        }
        _conversationEnd = true;

    }
}

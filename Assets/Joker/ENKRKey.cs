using UnityEngine;
using System.Collections;

namespace VRKeys
{

    /// <summary>
    /// ChangeLanguage key.
    /// </summary>
    public class ENKRKey : Key
    {

        public override void HandleTriggerEnter(Collider other)
        {
            keyboard.ChangeLanguage();

            ActivateFor(0.3f);
        }

        public override void UpdateLayout(Layout translation)
        {
            label.text = translation.backspaceButtonLabel;
        }
    }
}

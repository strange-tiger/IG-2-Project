namespace VRKeys.Layouts
{

    /// <summary>
    /// Korean language keyboard.
    /// </summary>
    public class Korean : Layout
    {
        public Korean()
        {
            placeholderMessage = "키를 눌러 타이핑을 시작하세요";

            spaceButtonLabel = "SPACE";

            enterButtonLabel = "ENTER";

            cancelButtonLabel = "CANCEL";

            shiftButtonLabel = "SHIFT";

            backspaceButtonLabel = "BACKSPACE";

            clearButtonLabel = "CLEAR";

            row1Keys = new string[] { "`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=" };

            row1Shift = new string[] { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+" };

            row2Keys = new string[] { "ㅂ", "ㅈ", "ㄷ", "ㄱ", "ㅅ", "ㅛ", "ㅕ", "ㅑ", "ㅐ", "ㅔ", "[", "]", "\\" };

            row2Shift = new string[] { "ㅃ", "ㅉ", "ㄸ", "ㄲ", "ㅆ", "ㅛ", "ㅕ", "ㅑ", "ㅒ", "ㅖ", "{", "}", "|" };

            row3Keys = new string[] { "ㅁ", "ㄴ", "ㅇ", "ㄹ", "ㅎ", "ㅗ", "ㅓ", "ㅏ", "ㅣ", ";", "'" };

            row3Shift = new string[] { "ㅁ", "ㄴ", "ㅇ", "ㄹ", "ㅎ", "ㅗ", "ㅓ", "ㅏ", "ㅣ", ":", "\"" };

            row4Keys = new string[] { "ㅋ", "ㅌ", "ㅊ", "ㅍ", "ㅠ", "ㅜ", "ㅡ", ",", ".", "?" };

            row4Shift = new string[] { "ㅋ", "ㅌ", "ㅊ", "ㅍ", "ㅠ", "ㅜ", "ㅡ", "<", ">", "/" };

            row1Offset = 0.16f;

            row2Offset = 0.08f;

            row3Offset = 0f;

            row4Offset = -0.08f;
        }
    }
}

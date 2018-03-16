using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 移动设备输入框的自适应组件
/// </summary>
public class WindowAdaptMobileKeyBoard : MonoBehaviour
{
    private InputField[] _inputField;
    private int _inputFieldCount;
    private Vector2 _adaptPanelOriginPos;
    private RectTransform _adaptPanelRt;
    private float RESOULUTION_HEIGHT = 1920f;
    private float _offsetHeight = 180f; 
    public void Register(GameObject attachRoot, InputField[] inputField)
    {
       _inputField = inputField;
       _inputFieldCount = inputField.Length;
       _adaptPanelRt = attachRoot.GetComponent<RectTransform>();
       _adaptPanelOriginPos = _adaptPanelRt.anchoredPosition;
       for (int i = 0; i < _inputField.Length; i++)
       {
           _inputField[i].onEndEdit.AddListener(OnEndEdit);
       }   
    }

    void Update()
    {
        for (int i = 0; i < _inputFieldCount; i++)
        {          
            if (_inputField[i].isFocused)
            {    
#if UNITY_ANDROID &&!UNITY_EDITOR
                 float keyboardHeight = AndroidGetKeyboardHeight() * RESOULUTION_HEIGHT / Screen.height + _offsetHeight;
                 if (keyboardHeight <= _adaptPanelOriginPos.y)
                    return;
                 _adaptPanelRt.anchoredPosition = Vector3.up * (keyboardHeight);
#elif UNITY_IPHONE&&!UNITY_EDITOR
                float keyboardHeight = IOSGetKeyboardHeight() * RESOULUTION_HEIGHT / Screen.height;
                if (keyboardHeight <= _adaptPanelOriginPos.y)
                    return;
               // TDebug.LogError("kerboadrdHeight:" + IOSGetKeyboardHeight() + "Screen.height " + Screen.height);
                _adaptPanelRt.anchoredPosition = Vector3.up * keyboardHeight;
#else
            //_adaptPanelRt.anchoredPosition = Vector3.up * 800;
#endif
            }
        }       
    }

    /// <summary>
    /// 结束编辑事件，TouchScreenKeyboard.isFocused为false的时候
    /// </summary>
    /// <param name="currentInputString"></param>
    private void OnEndEdit(string currentInputString)
    {
        _adaptPanelRt.anchoredPosition = _adaptPanelOriginPos;
    }

    /// <summary>
    /// 获取安卓平台上键盘的高度
    /// </summary>
    /// <returns></returns>
#if UNITY_ANDROID
    public float AndroidGetKeyboardHeight()
    {
        using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").
                Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

            using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
            {
                View.Call("getWindowVisibleDisplayFrame", Rct);
                return Screen.height - Rct.Call<int>("height");          
            }
        }
    }
#endif
#if UNITY_IPHONE
    public float IOSGetKeyboardHeight()
    {
        return TouchScreenKeyboard.area.height;
    }
#endif
}
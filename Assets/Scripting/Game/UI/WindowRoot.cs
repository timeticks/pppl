using UnityEngine;
using System.Collections;

public class WindowRoot : MonoBehaviour {

    public enum MoveAnimeType
    {
        None,
        Scale,
        Horizontal,
        Vertical,
        HorizontalBack,
        VerticalBack
    }
    public enum RotateAnimeType
    {
        None,
        Forward,
        ForwardBack,
        Reverse,
        ReverseBack
    }
    
    public GameObject CloseEventTarget;
    public MoveAnimeType RootMoveType;
    public RotateAnimeType RootRotateType;
    [SerializeField]
    [Range( 0.3f, 2.0f )]
    private float Time_open = 0.5f;
    [SerializeField]
    [Range( 0.3f, 2.0f )]
    private float Time_close = 0.5f;
    [SerializeField]
    private float TweenPosValue = 100;
    [SerializeField]
    private Vector3 TweenRotValue = new Vector3( 0, 0, 0);
    private Vector3 _startRotation = Vector3.zero;
    [HideInInspector]
    public bool IsOpen;

    public void RootAnime_Open( )
    {
        IsOpen = true;
        /*switch (RootMoveType)
        {
        case MoveAnimeType.None:
            break;
        case MoveAnimeType.Scale:
            iTween.ValueTo( gameObject, iTween.Hash( "from", new Vector3( 0.3f, 0.3f, 1 ), "to", Vector3.one, "easeType", iTween.EaseType.easeOutBack, "time", Time_open, "onupdate", "ScaleUpdate" ) );
            break;
        case MoveAnimeType.Horizontal:
            iTween.ValueTo( gameObject, iTween.Hash( "from", new Vector3( TweenPosValue, 0, 0 ), "to", Vector3.zero, "easeType", iTween.EaseType.easeOutQuint, "time", Time_open, "onupdate", "PositoinUpdate" ) );
            break;
        case MoveAnimeType.Vertical:
            iTween.ValueTo( gameObject, iTween.Hash( "from", new Vector3( 0, TweenPosValue, 0 ), "to", Vector3.zero, "easeType", iTween.EaseType.easeOutQuint, "time", Time_open, "onupdate", "PositoinUpdate" ) );
            break;
        case MoveAnimeType.HorizontalBack:
            iTween.ValueTo( gameObject, iTween.Hash( "from", new Vector3( TweenPosValue, 0, 0 ), "to", Vector3.zero, "easeType", iTween.EaseType.easeOutQuint, "time", Time_open, "onupdate", "PositoinUpdate" ) );
            break;
        case MoveAnimeType.VerticalBack:
            iTween.ValueTo( gameObject, iTween.Hash( "from", new Vector3( 0, TweenPosValue, 0 ), "to", Vector3.zero, "easeType", iTween.EaseType.easeOutQuint, "time", Time_open, "onupdate", "PositoinUpdate" ) );
            break;
        }

        _startRotation = transform.localEulerAngles;
        switch (RootRotateType)
        {
        case RotateAnimeType.None:
            break;
        case RotateAnimeType.Forward:
            iTween.RotateTo( gameObject, iTween.Hash( "rotation", TweenRotValue, "islocal", true, "easeType", iTween.EaseType.easeOutQuint, "time", Time_open ) );
            break;
        case RotateAnimeType.ForwardBack:
            iTween.RotateTo( gameObject, iTween.Hash( "rotation", TweenRotValue, "islocal", true, "easeType", iTween.EaseType.easeOutQuint, "time", Time_open ) );
            break;
        case RotateAnimeType.Reverse:
            iTween.RotateFrom( gameObject, iTween.Hash( "rotation", TweenRotValue, "islocal", true, "easeType", iTween.EaseType.easeOutQuint, "time", Time_open ) );
            break;
        case RotateAnimeType.ReverseBack:
            iTween.RotateFrom( gameObject, iTween.Hash( "rotation", TweenRotValue, "islocal", true, "easeType", iTween.EaseType.easeOutQuint, "time", Time_open ) );
            break;
        }*/
        transform.localPosition = Vector3.zero;
    }

    public void RootAnime_Close( )
    {
        IsOpen = false;
        /*switch (RootMoveType)
        {
        case MoveAnimeType.None:
            break;
        case MoveAnimeType.Scale:
            iTween.ValueTo( gameObject, iTween.Hash( "from", Vector3.one, "to", new Vector3( 0.5f, 0.5f, 1 ), "easeType", iTween.EaseType.easeInBack, "time", Time_close, "onupdate", "ScaleUpdate", "oncomplete", "DoCloseEvent", "oncompletetarget", CloseEventTarget ) );
            break;
        case MoveAnimeType.Horizontal:
            iTween.ValueTo( gameObject, iTween.Hash( "from", Vector3.zero, "to", new Vector3( -1 * TweenPosValue, 0, 0 ), "easeType", iTween.EaseType.easeOutQuint, "time", Time_close, "onupdate", "PositoinUpdate", "oncomplete", "DoCloseEvent", "oncompletetarget", CloseEventTarget ) );
            break;
        case MoveAnimeType.Vertical:
            iTween.ValueTo( gameObject, iTween.Hash( "from", Vector3.zero, "to", new Vector3( 0, -1 * TweenPosValue, 0 ), "easeType", iTween.EaseType.easeOutQuint, "time", Time_close, "onupdate", "PositoinUpdate", "oncomplete", "DoCloseEvent", "oncompletetarget", CloseEventTarget ) );
            break;
        case MoveAnimeType.HorizontalBack:
            iTween.ValueTo( gameObject, iTween.Hash( "from", Vector3.zero, "to", new Vector3( TweenPosValue, 0, 0 ), "easeType", iTween.EaseType.easeOutQuint, "time", Time_close, "onupdate", "PositoinUpdate", "oncomplete", "DoCloseEvent", "oncompletetarget", CloseEventTarget ) );
            break;
        case MoveAnimeType.VerticalBack:
            iTween.ValueTo( gameObject, iTween.Hash( "from", Vector3.zero, "to", new Vector3( 0, TweenPosValue, 0 ), "easeType", iTween.EaseType.easeOutQuint, "time", Time_close, "onupdate", "PositoinUpdate", "oncomplete", "DoCloseEvent", "oncompletetarget", CloseEventTarget ) );
            break;
        }

        switch (RootRotateType)
        {
        case RotateAnimeType.None:
            break;
        case RotateAnimeType.Forward:
            iTween.RotateAdd( gameObject, iTween.Hash( "amount", TweenRotValue, "islocal", true, "easeType", iTween.EaseType.easeOutQuint, "time", Time_close ) );
            break;
        case RotateAnimeType.ForwardBack:
            iTween.RotateTo( gameObject, iTween.Hash( "rotation", Vector3.zero, "islocal", true, "easeType", iTween.EaseType.easeOutQuint, "time", Time_close ) );
            break;
        case RotateAnimeType.Reverse:
            iTween.RotateAdd( gameObject, iTween.Hash( "amount", TweenRotValue, "islocal", true, "easeType", iTween.EaseType.easeOutQuint, "time", Time_close ) );
            break;
        case RotateAnimeType.ReverseBack:
            iTween.RotateTo( gameObject, iTween.Hash( "rotation", TweenRotValue, "islocal", true, "easeType", iTween.EaseType.easeOutQuint, "time", Time_close ) );
            break;
        }
        if (RootRotateType != RotateAnimeType.None)
        {
            StartCoroutine( ResetRotate( ) );
        }*/
        switch( RootMoveType )
        {
            case MoveAnimeType.None:
                break;
            case MoveAnimeType.Scale:
                transform.localPosition = Vector3.one;               
                break;
            case MoveAnimeType.Horizontal:
                transform.localPosition = new Vector3( -1 * TweenPosValue, 0, 0 );
                break;
            case MoveAnimeType.Vertical:
                transform.localPosition = new Vector3( 0, -1 * TweenPosValue, 0);
                break;
            case MoveAnimeType.HorizontalBack:
                transform.localPosition = new Vector3( TweenPosValue, 0, 0 );
                break;
            case MoveAnimeType.VerticalBack:
                transform.localPosition = new Vector3( 0, TweenPosValue, 0);
                break;
        }
        if( CloseEventTarget != null )
            CloseEventTarget.SendMessage( "DoCloseEvent", SendMessageOptions.DontRequireReceiver );
    }

    void PositoinUpdate( Vector3 _delta )
    {
        transform.localPosition = _delta;
    }
    void ScaleUpdate( Vector3 _delta )
    {
        transform.localScale = _delta;
    }
    void RotateUpdate( Vector3 _delta )
    {
        transform.localEulerAngles = _delta;
    }

    IEnumerator ResetRotate( )
    {
        yield return new WaitForSeconds( Time_close );
        transform.localEulerAngles = _startRotation;
    }
}

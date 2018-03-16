using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITimeCountDown {

    public enum TimeType
    {
        LCD
    }

    public TimeType m_TimeType = TimeType.LCD;
    
    internal Text m_Text;

    private string m_startFormat;
    private string m_endShow;

    private float _deltaTime = 0;
    private int _totalTime = 0;
    public int TotalTime
    {
        get
        {
            return _totalTime;
        }
    }


    public void UpdateTime( )
    {
        if( _totalTime > 0 )
        {
            if( Time.time - _deltaTime >= 1 )
            {
                _deltaTime = Time.time;
                _totalTime -= 1;
                ShowTime( );
            }
        }
    }

    public UITimeCountDown( Text _text )
    {
        m_Text = _text;
    }

    public void StartCountDown( int _totaltime, string _startFormat, string _endShow )
    {
        _deltaTime = Time.time;
        _totalTime = _totaltime;
        m_startFormat = _startFormat;
        m_endShow = _endShow;

        ShowTime( );
    }

    public void StopCountDown( )
    {
        _deltaTime = 0;
        _totalTime = 0;
        m_startFormat = string.Empty;
        m_endShow = string.Empty;
    }

    void ShowTime( )
    {
        if( _totalTime > 0 )
        {
            switch( m_TimeType )
            {
                case TimeType.LCD:
                    m_Text.text = string.Format( m_startFormat, TUtility.TimeSecondsToDayStr_LCD( _totalTime ) );
                    break;
            }            
        }
        else
        {
            m_Text.text = m_endShow;
        }
            
    }
}

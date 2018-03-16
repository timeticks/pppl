using UnityEngine;
using System.Collections;


/// <summary>
/// 二方向、四方向、八方向
/// </summary>
public static class DirClass
{
    /// <summary>
    /// 得到动画的方向
    /// </summary>
    public static int GetActionDir(DirNumType dirNum, Vector3 myPos, Vector3 targetPos)
    {
        return GetActionDir(dirNum, new Vector2(targetPos.x - myPos.x, targetPos.z - myPos.z));
    }
    public static int GetActionDir(DirNumType dirNum, Vector2 myPos, Vector2 targetPos)
    {
        return GetActionDir(dirNum, new Vector2(targetPos.x - myPos.x, targetPos.y - myPos.y));
    }
    public static Vector2 GetDirV2(Vector3 myPos, Vector3 targetPos)
    {
        return new Vector2(targetPos.x - myPos.x, targetPos.z - myPos.z).normalized;
    }
    public static int GetActionDir(DirNumType dirNum, Vector2 dir)
    {
        switch (dirNum)
        {
            case DirNumType.Two: return (int)GetActionDir_Two(dir);
            case DirNumType.Four: return (int)GetActionDir_Four(dir);
            case DirNumType.Eight: return (int)GetActionDir_Eight(dir);
        }
        return 0;
    }
    public static ActionFourDir GetActionDir_Four(Vector2 dir)
    {
        float angle = Vector2.Angle(Vector2.right, dir);
        if (dir.y < 0) angle += 180;
        int times = Mathf.FloorToInt(angle / 45f);
        switch (times)
        {
            case 7:
            case 0: return ActionFourDir.Right;
            case 1:
            case 2: return ActionFourDir.Up;
            case 3:
            case 4: return ActionFourDir.Left;
            case 5:
            case 6: return ActionFourDir.Down;
        }
        return ActionFourDir.Right;
    }
    public static ActionTwoDir GetActionDir_Two(Vector2 dir)
    {
        dir = dir.normalized;
        if (Vector2.Angle(dir, Vector2.right) < 90f) { return ActionTwoDir.Right; }
        else { return ActionTwoDir.Left; }
    }
    public static ActionEightDir GetActionDir_Eight(Vector2 dir)
    {
        float angle = Vector2.Angle(Vector2.right, dir);
        if (dir.y < 0) angle = 360-angle;
        int times = Mathf.FloorToInt(angle / 22.5f);
        switch (times)
        {
            case 15:
            case 0: return ActionEightDir.Right;
            case 1:
            case 2: return ActionEightDir.RightBack;
            case 3:
            case 4: return ActionEightDir.Back;
            case 5:
            case 6: return ActionEightDir.LeftBack;
            case 7:
            case 8: return ActionEightDir.Left;
            case 9:
            case 10: return ActionEightDir.LeftFront;
            case 11:
            case 12: return ActionEightDir.Front;
            case 13:
            case 14: return ActionEightDir.RightFront;
        }
        return ActionEightDir.Right;
    }

    public static ActionSixDir GetActionDir_SixVertical(Vector2 dir, ActionSixDir curDir)//根据方向，得到最近的垂直方向
    {
        ActionSixDir dirType = GetActionDir_Six(dir);
        Vector2 dir1 = new Vector2(-dir.y, dir.x);
        Vector2 dir2 = new Vector2(dir.y, -dir.x);
        ActionSixDir v1 = GetActionDir_Six(dir1);
        ActionSixDir v2 = GetActionDir_Six(dir2);
        int v1Offset = GetDirOffset_Six(v1, curDir);
        int v2Offset = GetDirOffset_Six(v2, curDir);
        return v1Offset > v2Offset ? v2 : v1;
    }
    public static Vector2 GetActionDirV2_Six(ActionSixDir dir)
    {
        switch (dir)
        {
            case ActionSixDir.LeftFront: return new Vector2(-1, -1).normalized;
            case ActionSixDir.Left: return new Vector2(-1, 0).normalized;
            case ActionSixDir.LeftBack: return new Vector2(-1, 1).normalized;
            case ActionSixDir.RightBack: return new Vector2(1, 1).normalized;
            case ActionSixDir.Right: return new Vector2(1, 0).normalized;
            case ActionSixDir.RightFront: return new Vector2(1, -1).normalized;
        }
        return new Vector2(-1, 0);
    }
    public static ActionSixDir GetActionDir_Six(Vector2 dir)//根据方向，得到六方形方向类型
    {
        Vector2 offsetDir = dir.normalized;
        if (offsetDir.x > 0.8f) { return ActionSixDir.Right; }
        else if (offsetDir.x < -0.8f) { return ActionSixDir.Left; }

        if (offsetDir.y >= 0)
        {
            if (offsetDir.x > 0f) { return ActionSixDir.RightBack; }
            else { return ActionSixDir.LeftBack; }
        }
        else
        {
            if (offsetDir.x > 0f) { return ActionSixDir.RightFront; }
            else{ return ActionSixDir.LeftFront; }
        }
    }
    public static int GetDirOffset_Six(ActionSixDir dir1 , ActionSixDir dir2)//两方向的差
    { 
        int o = Mathf.Abs((int)dir2 - (int)dir1);
        return Mathf.Min(o, 6 - o);
    }
    public static ActionSixDir SetDirOffset_Six(ActionSixDir dir , int off)
    {
        int d = (int)dir + off;
        if (d >= (int)ActionSixDir.Max) d -= (int)ActionSixDir.Max;
        else if (d > 0) d += (int)ActionSixDir.Max;
        return (ActionSixDir)d;
    }

    public static int GetDirImageIndex(DirNumType dirNum , int dirType)  //得到各方向，在图片中的行数值
    {
        switch (dirNum)
        {
            case DirNumType.Two: return 0;
            case DirNumType.Four:
                ActionFourDir fourDir = (ActionFourDir)dirType;
                switch (fourDir)
	            {
                    case ActionFourDir.Down: return 0;
                    case ActionFourDir.Left: return 1;
                    case ActionFourDir.Right: return 2;
                    case ActionFourDir.Up: return 4;
                    default: return 0;
	            }
            case DirNumType.Six:
                ActionSixDir sixDir = (ActionSixDir)dirType;
                switch (sixDir)
	            {
                    case ActionSixDir.RightFront:
                    case ActionSixDir.LeftFront: return 1;
                    case ActionSixDir.LeftBack:
                    case ActionSixDir.RightBack: return 3;
                    case ActionSixDir.Right:
                    case ActionSixDir.Left: return 2;
                    default: return 0;
	            }
            case DirNumType.Eight:
                ActionEightDir dirEight = (ActionEightDir)dirType;
                switch (dirEight)
	            {
                    case ActionEightDir.Front: return 0;
                    case ActionEightDir.RightFront:
                    case ActionEightDir.LeftFront: return 1;
                    case ActionEightDir.Right:
                    case ActionEightDir.Left: return 2;
                    case ActionEightDir.LeftBack:
                    case ActionEightDir.RightBack: return 3;
                    case ActionEightDir.Back: return 4;
                    default: return 0;
	            }
        }
        return 0;
    }


}





public enum ActionFourDir:byte
{
    [EnumDesc("下")]
    Down = 0,   //朝屏幕下方
    [EnumDesc("左")]
    Left,
    [EnumDesc("右")]
    Right,
    [EnumDesc("上")]
    Up,
    Max
}

public enum ActionTwoDir : byte
{
    Left = 0,
    Right,

    Max
}

public enum ActionEightDir : byte
{
    Front = 0,
    LeftFront, //朝屏幕下方
    Left,
    LeftBack,  //朝屏幕上方
    Back = 4,
    RightFront,
    Right,
    RightBack,

    Max
}

public enum ActionSixDir : byte
{
    LeftFront = 0,  //朝屏幕下方
    Left = 1,
    LeftBack = 2,
    RightBack = 3,
    Right      = 4,
    RightFront  = 5,
    

    Max
}

public enum DirNumType : byte  //两方向、四方向、六方向、八方向
{
    Two,
    Four,
    Six,
    Eight
}


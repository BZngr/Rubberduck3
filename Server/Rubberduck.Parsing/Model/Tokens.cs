﻿namespace Rubberduck.Parsing.Model;

public static class Tokens
{
    public static readonly string Abs = "Abs";
    public static readonly string AddressOf = "AddressOf";
    public static readonly string And = "And";
    public static readonly string Any = "Any";
    public static readonly string As = "As";
    public static readonly string Asc = "Asc";
    public static readonly string Attribute = "Attribute";
    public static readonly string Base = "Base";
    public static readonly string Beep = "Beep";
    public static readonly string Binary = "Binary";
    public static readonly string Boolean = "Boolean";
    public static readonly string ByRef = "ByRef";
    public static readonly string Byte = "Byte";
    public static readonly string ByVal = "ByVal";
    public static readonly string Call = "Call";
    public static readonly string Case = "Case";
    public static readonly string CBool = "CBool";
    public static readonly string CByte = "CByte";
    public static readonly string CCur = "CCur";
    public static readonly string CDate = "CDate";
    public static readonly string CDbl = "CDbl";
    public static readonly string CDec = "CDec";
    public static readonly string ChDir = "ChDir";
    public static readonly string ChDrive = "ChDrive";
    public static readonly string Chr = "Chr";
    public static readonly string ChrB = "ChrB";
    public static readonly string ChrW = "ChrW";
    public static readonly string CInt = "CInt";
    public static readonly string CLng = "CLng";
    public static readonly string CLngLng = "CLngLng";
    public static readonly string CLngPtr = "CLngPtr";
    public static readonly string Close = "Close";
    public static readonly string Command = "Command";
    public static readonly string CommentMarker = "'";
    public static readonly string Compare = "Compare";
    public static readonly string Const = "Const";
    public static readonly string Cos = "Cos";
    public static readonly string CSng = "CSng";
    public static readonly string CStr = "CStr";
    public static readonly string CurDir = "CurDir";
    public static readonly string Currency = "Currency";
    public static readonly string CVar = "CVar";
    public static readonly string CVErr = "CVErr";
    public static readonly string Data = "Data";
    public static readonly string Date = "Date";
    public static readonly string DateValue = "DateValue";
    public static readonly string Day = "Day";
    public static readonly string Debug = "Debug";
    public static readonly string Decimal = "Decimal";
    public static readonly string Declare = "Declare";
    public static readonly string DefBool = "DefBool";
    public static readonly string DefByte = "DefByte";
    public static readonly string DefCur = "DefCur";
    public static readonly string DefDate = "DefDate";
    public static readonly string DefDbl = "DefDbl";
    public static readonly string DefInt = "DefInt";
    public static readonly string DefLng = "DefLng";
    public static readonly string DefLngLng = "DefLngLng";
    public static readonly string DefLngPtr = "DefLngptr";
    public static readonly string DefObj = "DefObj";
    public static readonly string DefSng = "DefSng";
    public static readonly string DefStr = "DefStr";
    public static readonly string DefVar = "DefVar";
    public static readonly string Dim = "Dim";
    public static readonly string Dir = "Dir";
    public static readonly string Do = "Do";
    public static readonly string DoEvents = "DoEvents";
    public static readonly string Double = "Double";
    public static readonly string Each = "Each";
    public static readonly string Else = "Else";
    public static readonly string ElseIf = "ElseIf";
    public static readonly string Empty = "Empty";
    public static readonly string End = "End";
    public static readonly string Enum = "Enum";
    public static readonly string Environ = "Environ";
    public static readonly string EOF = "EOF";
    public static readonly string Eqv = "Eqv";
    public static readonly string Erase = "Erase";
    public static readonly string Err = "Err";
    public static readonly string Error = "Error";
    public static readonly string Event = "Event";
    public static readonly string Exit = "Exit";
    public static readonly string Exp = "Exp";
    public static readonly string Explicit = "Explicit";
    public static readonly string False = "False";
    public static readonly string Fix = "Fix";
    public static readonly string For = "For";
    public static readonly string Format = "Format";
    public static readonly string FreeFile = "FreeFile";
    public static readonly string Friend = "Friend";
    public static readonly string Function = "Function";
    public static readonly string Get = "Get";
    public static readonly string Global = "Global";
    public static readonly string GoSub = "GoSub";
    public static readonly string GoTo = "GoTo";
    public static readonly string Hex = "Hex";
    public static readonly string Hour = "Hour";
    public static readonly string If = "If";
    public static readonly string Imp = "Imp";
    public static readonly string Implements = "Implements";
    public static readonly string In = "In";
    public static readonly string Input = "Input";
    public static readonly string InputB = "InputB";
    public static readonly string InputBox = "InputBox";
    public static readonly string InStr = "InStr";
    public static readonly string Int = "Int";
    public static readonly string Integer = "Integer";
    public static readonly string Is = "Is";
    public static readonly string IsDate = "IsDate";
    public static readonly string IsEmpty = "IsEmpty";
    public static readonly string IsNull = "IsNull";
    public static readonly string IsNumeric = "IsNumeric";
    public static readonly string Join = "Join";
    public static readonly string Kill = "Kill";
    public static readonly string LBound = "LBound";
    public static readonly string LCase = "LCase";
    public static readonly string Left = "Left";
    public static readonly string LeftB = "LeftB";
    public static readonly string Len = "Len";
    public static readonly string LenB = "LenB";
    public static readonly string Let = "Let";
    public static readonly string Like = "Like";
    public static readonly string Line = "Line";
    public static readonly string LineContinuation = " _";
    public static readonly string Lock = "Lock";
    public static readonly string LOF = "LOF";
    public static readonly string Long = "Long";
    public static readonly string LongLong = "LongLong";
    public static readonly string LongPtr = "LongPtr";
    public static readonly string Loop = "Loop";
    public static readonly string LSet = "LSet";
    public static readonly string LTrim = "LTrim";
    public static readonly string Me = "Me";
    public static readonly string Mid = "Mid";
    public static readonly string MidB = "MidB";
    public static readonly string Minute = "Minute";
    public static readonly string MkDir = "MkDir";
    public static readonly string Mod = "Mod";
    public static readonly string Month = "Month";
    public static readonly string MsgBox = "MsgBox";
    public static readonly string New = "New";
    public static readonly string Next = "Next";
    public static readonly string Not = "Not";
    public static readonly string Nothing = "Nothing";
    public static readonly string Now = "Now";
    public static readonly string Null = "Null";
    public static readonly string Object = "Object";
    public static readonly string Oct = "Oct";
    public static readonly string On = "On";
    public static readonly string Open = "Open";
    public static readonly string Option = "Option";
    public static readonly string Optional = "Optional";
    public static readonly string Or = "Or";
    public static readonly string Output = "Output";
    public static readonly string ParamArray = "ParamArray";
    public static readonly string Preserve = "Preserve";
    public static readonly string Print = "Print";
    public static readonly string Private = "Private";
    public static readonly string Property = "Property";
    public static readonly string Public = "Public";
    public static readonly string Put = "Put";
    public static readonly string RaiseEvent = "RaiseEvent";
    public static readonly string Random = "Random";
    public static readonly string Randomize = "Randomize";
    public static readonly string Read = "Read";
    public static readonly string ReDim = "ReDim";
    public static readonly string Rem = "Rem";
    public static readonly string Resume = "Resume";
    public static readonly string Return = "Return";
    public static readonly string RSet = "RSet";
    public static readonly string Right = "Right";
    public static readonly string RightB = "RightB";
    public static readonly string RmDir = "RmDir";
    public static readonly string Rnd = "Rnd";
    public static readonly string RTrim = "RTrim";
    public static readonly string Second = "Second";
    public static readonly string Seek = "Seek";
    public static readonly string Select = "Select";
    public static readonly string Set = "Set";
    public static readonly string Shared = "Shared";
    public static readonly string Shell = "Shell";
    public static readonly string Sin = "Sin";
    public static readonly string Single = "Single";
    public static readonly string Sng = "Sng";
    public static readonly string Space = "Space";
    public static readonly string Spc = "Spc";
    public static readonly string Split = "Split";
    public static readonly string Sqr = "Sqr";
    public static readonly string Static = "Static";
    public static readonly string Step = "Step";
    public static readonly string Stop = "Stop";
    public static readonly string Str = "Str";
    public static readonly string StrConv = "StrConv";
    public static readonly string String = "String";
    public static readonly string StrPtr = "StrPtr";
    public static readonly string Sub = "Sub";
    public static readonly string Then = "Then";
    public static readonly string Time = "Time";
    public static readonly string To = "To";
    public static readonly string Trim = "Trim";
    public static readonly string True = "True";
    public static readonly string Type = "Type";
    public static readonly string TypeName = "TypeName";
    public static readonly string TypeOf = "TypeOf";
    public static readonly string UBound = "UBound";
    public static readonly string UCase = "UCase";
    public static readonly string Unlock = "Unlock";
    public static readonly string Until = "Until";
    public static readonly string Val = "Val";
    public static readonly string Variant = "Variant";
    public static readonly string vbBack = "vbBack";
    public static readonly string vbCr = "vbCr";
    public static readonly string vbCrLf = "vbCrLf";
    public static readonly string vbFormFeed = "vbFormFeed";
    public static readonly string vbLf = "vbLf";
    public static readonly string vbNewLine = "vbNewLine";
    public static readonly string vbNullChar = "vbNullChar";
    public static readonly string vbNullString = "vbNullString";
    public static readonly string vbTab = "vbTab";
    public static readonly string vbVerticalTab = "vbVerticalTab";
    public static readonly string WeekDay = "WeekDay";
    public static readonly string Wend = "Wend";
    public static readonly string While = "While";
    public static readonly string Width = "Width";
    public static readonly string With = "With";
    public static readonly string WithEvents = "WithEvents";
    public static readonly string Write = "Write";
    public static readonly string XOr = "Xor";
    public static readonly string Year = "Year";

    public static readonly string EndSub = $"{End} {Sub}";
    public static readonly string EndEnum = $"{End} {Enum}";
    public static readonly string EndFunction = $"{End} {Function}";
    public static readonly string EndProperty = $"{End} {Property}";
    public static readonly string EndSelect = $"{End} {Select}";
    public static readonly string EndType = $"{End} {Type}";
    public static readonly string EndWith = $"{End} {With}";
    public static readonly string EndIf = $"{End} {If}";
    public static readonly string ForEach = $"{For} {Each}";
    public static readonly string PropertyGet = $"{Property} {Get}";
    public static readonly string PropertyLet = $"{Property} {Let}";
    public static readonly string PropertySet = $"{Property} {Set}";
}

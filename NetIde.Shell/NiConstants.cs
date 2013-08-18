using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NetIde.Shell
{
    public static class NiConstants
    {
        public const int OLE_E_OLEVERB = -2147221504;
        public const int OLE_E_ADVF = -2147221503;
        public const int OLE_E_ENUM_NOMORE = -2147221502;
        public const int OLE_E_ADVISENOTSUPPORTED = -2147221501;
        public const int OLE_E_NOCONNECTION = -2147221500;
        public const int OLE_E_NOTRUNNING = -2147221499;
        public const int OLE_E_NOCACHE = -2147221498;
        public const int OLE_E_BLANK = -2147221497;
        public const int OLE_E_CLASSDIFF = -2147221496;
        public const int OLE_E_CANT_GETMONIKER = -2147221495;
        public const int OLE_E_CANT_BINDTOSOURCE = -2147221494;
        public const int OLE_E_STATIC = -2147221493;
        public const int OLE_E_PROMPTSAVECANCELLED = -2147221492;
        public const int OLE_E_INVALIDRECT = -2147221491;
        public const int OLE_E_WRONGCOMPOBJ = -2147221490;
        public const int OLE_E_INVALIDHWND = -2147221489;
        public const int OLE_E_NOT_INPLACEACTIVE = -2147221488;
        public const int OLE_E_CANTCONVERT = -2147221487;
        public const int OLE_E_NOSTORAGE = -2147221486;
        public const int DISP_E_UNKNOWNINTERFACE = -2147352575;
        public const int DISP_E_MEMBERNOTFOUND = -2147352573;
        public const int DISP_E_PARAMNOTFOUND = -2147352572;
        public const int DISP_E_TYPEMISMATCH = -2147352571;
        public const int DISP_E_UNKNOWNNAME = -2147352570;
        public const int DISP_E_NONAMEDARGS = -2147352569;
        public const int DISP_E_BADVARTYPE = -2147352568;
        public const int DISP_E_EXCEPTION = -2147352567;
        public const int DISP_E_OVERFLOW = -2147352566;
        public const int DISP_E_BADINDEX = -2147352565;
        public const int DISP_E_UNKNOWNLCID = -2147352564;
        public const int DISP_E_ARRAYISLOCKED = -2147352563;
        public const int DISP_E_BADPARAMCOUNT = -2147352562;
        public const int DISP_E_PARAMNOTOPTIONAL = -2147352561;
        public const int DISP_E_BADCALLEE = -2147352560;
        public const int DISP_E_NOTACOLLECTION = -2147352559;
        public const int DISP_E_DIVBYZERO = -2147352558;
        public const int DISP_E_BUFFERTOOSMALL = -2147352557;
        public const int S_FALSE = 1;
        public const int S_OK = 0;
        public const int E_OUTOFMEMORY = -2147024882;
        public const int E_INVALIDARG = -2147024809;
        public const int E_FAIL = -2147467259;
        public const int E_NOINTERFACE = -2147467262;
        public const int E_NOTIMPL = -2147467263;
        public const int E_UNEXPECTED = -2147418113;
        public const int E_POINTER = -2147467261;
        public const int E_HANDLE = -2147024890;
        public const int E_ABORT = -2147467260;
        public const int E_ACCESSDENIED = -2147024891;
        public const int E_PENDING = -2147483638;

        public const string TextEditor = "a123b744-ee17-46aa-a53f-3bd2e4d423ac";
        public const string DiffViewer = "29200565-53e3-4bbd-8f2e-bd9ef7062837";
        public const string TextBuffer = "31d8aa3d-d8ed-4e7a-867c-6f388994d503";
        public const string TextLines = "271e6847-c239-46ce-b535-7021ddf4c3e6";

        public const string LanguageServiceXml = "e4a6478a-c4b6-445a-bd65-5a89211e50d7";
        public const string LanguageServiceHtml = "181c6a04-80de-44d8-818b-cea443d71005";
        public const string LanguageServiceCppNet = "6fbea381-d063-4a55-b5a6-a6d61c186153";
        public const string LanguageServiceBat = "5b28d613-00a9-4697-ae78-0680de952fcf";
        public const string LanguageServiceCoco = "1e02d59a-37da-4951-a35c-aa51b8e76bb4";
        public const string LanguageServicePhp = "477a791b-b33b-4e64-9d42-3644da77a66f";
        public const string LanguageServiceCSharp = "26957167-4dde-451e-bd42-97631453fc9f";
        public const string LanguageServicePatch = "27d35a94-97df-4410-a473-6cd2bbed1fcf";
        public const string LanguageServiceBoo = "63583ef9-376c-4a75-b24d-794baf2e2740";
        public const string LanguageServiceDefault = "fb78e920-63a6-4a9a-ad19-d4e0058face8";
        public const string LanguageServiceVbNet = "3220e7e0-6674-42d2-bc79-ad423f4c3f39";
        public const string LanguageServiceTex = "850269ee-09e8-4312-b838-e01ca6439c99";
        public const string LanguageServiceAspXHtml = "f4ccef91-7f9a-4d40-8666-32e246926f31";
        public const string LanguageServiceJavaScript = "827ff0fa-c40c-4097-a936-a4f3d356eb07";
        public const string LanguageServiceJava = "9a3f512d-9160-4025-a075-bfce8273accb";

        public enum Severity
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal
        }
    }
}

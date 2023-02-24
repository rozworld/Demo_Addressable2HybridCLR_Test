using System.Net;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using UnityEditor;
using UnityEngine.Networking;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

public static class ToolUnit
{
    //平台路径示例
    // #if UNITY_EDITOR
    // string filepath = Application.dataPath +"/StreamingAssets"+"/my.xml";
    // #elif UNITY_IPHONE
    // string filepath = Application.dataPath +"/Raw"+"/my.xml";
    // #elif UNITY_ANDROID
    // string filepath = "jar:file://" + Application.dataPath + "!/assets/"+"/my.xml;
    // #endif

    //手机号正则1
    public const string cell_phoneNumberStr1 = @"/^1(3[0-9]|4[01456879]|5[0-35-9]|6[2567]|7[0-8]|8[0-9]|9[0-35-9])\d{8}$/";
    //手机号正则2
    public const string cell_phoneNumberStr2 = @"^1[34578]\\d{9}$";

    /// <summary>
    /// 时间格式转换
    /// </summary>
    /// <param name="dateTimeString">所需转换的格式</param>
    /// <param name="newDataTimeFormat">目标格式</param>
    /// <returns></returns>
    public static string TimeStringToFromat(string dateTimeString, string newDataTimeFormat)
    {
        System.DateTime dateTime = System.DateTime.Parse(dateTimeString);
        return dateTime.ToString(newDataTimeFormat);
    }

    /// <summary>
    /// 字符串加密 MD5
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string EncryptString(string data)
    {
        //将字符串以UTF-8格式转为byte数组
        byte[] resultBytes = Encoding.UTF8.GetBytes(data);
        //创建一个MD5的对象
        MD5 md5 = new MD5CryptoServiceProvider();
        //调用MD5的ComputeHash方法将字节数组加密
        byte[] outPut = md5.ComputeHash(resultBytes);
        StringBuilder hashString = new StringBuilder();
        //最后把加密后的字节数组转为字符串
        for (int i = 0; i < outPut.Length; i++)
        {
            hashString.Append(Convert.ToString(outPut[i], 16).PadLeft(2, '0'));
        }
        return hashString.ToString();
        //Debug.Log(data);
    }

    public static bool isInt(string str)
    {
        //^([+-]?)表示加减号只能出现在字符串开头且只有一位
        ///d*表示后面可以有多个或一个十进制数
        //$表示字符串结尾
        return Regex.IsMatch(str, @"^([+-]?)/d*$");//返回只能以正负号开头的整数
    }
    public static bool isUnInt(string str)
    {
        //^([+-]?)表示加减号只能出现在字符串开头且只有一位
        ///d*表示后面可以有多个或一个十进制数
        //$表示字符串结尾
        return Regex.IsMatch(str, @"^/d*$");//返回整数
    }
    public static bool isEmail(string str)
    {
        //邮件格式是字符串@字符串.字符串(最后的字符串限制为1到3位)
        return Regex.IsMatch(str, @"^([/w]*)([@]?)([/w]*)([.]?)([/w]{1,3})$");
    }
    public static bool checkFloat(string svalue)
    {
        //检查值是否为浮点数字,5位小数
        return Regex.IsMatch(svalue, @"^(/d*)([.]{0,1})(/d{0,5})$");
    }
    public static bool isNumeric(string str)
    {
        //判断是否是数值，有小数点
        return Regex.IsMatch(str, @"^([+-]?)/d*[.]?/d*$");
    }
    public static bool IsValidEmail(string strIn)
    {
        //判断有效的电子邮件格式
        return Regex.IsMatch(strIn, @"^([/w-/.]+)@((/[[0-9]{1,3}/.[0-9]{1,3}/.[0-9]{1,3}/.)|(([/w-]+/.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(/]?)$");
    }

    public static bool checkString(string svalue)
    {
        if (Regex.IsMatch(svalue, @"^([0-9]{1,})(.*)([0-9]{1,})$"))
        {
            //如果表达式头尾是数字
            //在检查是否匹配运算符是不是加减乘除，如果不是返回真
            if (Regex.IsMatch(svalue, @"(([0-9]{1,})([/+/-/*//]{2,})([0-9]{1,}))|(([0-9]{1,})([^/+/-/*//]{1,})([0-9]{1,}))"))
            {
                //表示表达式不合法
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    #region  用户号码等各项验证方法集合
    //验证电话号码
    public static bool IsTelephone(string str_telephone)
    {
        return Regex.IsMatch(str_telephone.ToString(), @"^(\d{3,4}-)?\d{6,8}$");
        //return Regex.IsMatch(str_telephone.ToString(), cell_phoneNumberStr);
    }
    //验证手机号码
    public static bool IsHandset(string str_handset)
    {
        return Regex.IsMatch(str_handset, "^1[34578]\\d{9}$");
    }

    //验证18位身份证号
    public static bool IsIDcardBy18(string str_idcard)
    {
        long n = 0;
        if (long.TryParse(str_idcard.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(str_idcard.Replace('x', '0').Replace('X', '0'), out n) == false)
        {
            return false;
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(str_idcard.Remove(2)) == -1)
        {
            return false;
        }
        string birth = str_idcard.Substring(6, 8).Insert(6, "-").Insert(4, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
        {
            return false;
        }
        string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
        string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
        char[] Ai = str_idcard.Remove(17).ToCharArray();
        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
        }
        int y = -1;
        Math.DivRem(sum, 11, out y);
        if (arrVarifyCode[y] != str_idcard.Substring(17, 1).ToLower())
        {
            return false;
        }
        return true;
    }

    //验证15位身份证号
    public static bool IsIDcardBy15(string str_idcard)
    {
        long n = 0;
        if (long.TryParse(str_idcard, out n) == false || n < Math.Pow(10, 14))
        {
            return false;
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(str_idcard.Remove(2)) == -1)
        {
            return false;
        }
        string birth = str_idcard.Substring(6, 6).Insert(4, "-").Insert(2, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
        {
            return false;
        }
        return true;
    }

    //验证数字
    public static bool IsNumber(string str_number)
    {
        return Regex.IsMatch(str_number, @"^[0-9]*$");
    }

    //验证邮编
    public static bool boolIsPostalcode(string str_postalcode)
    {
        return Regex.IsMatch(str_postalcode, @"^\d{6}$");
    }

    #endregion

    #region 文件相关操作

    //static FileInfo fileInfo;

    /// <summary>
    /// 返回带扩展名的文件名
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileName(string filePath)
    {
        return System.IO.Path.GetFileName(filePath); //返回带扩展名的文件名
    }

    /// <summary>
    /// 返回不带扩展名的文件名
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileNameWithoutExtension(string filePath)
    {
        return System.IO.Path.GetFileNameWithoutExtension(filePath); //返回不带扩展名的文件名
    }

    /// <summary>
    /// 返回文件所在目录
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetDirectoryName(string filePath)
    {
        return System.IO.Path.GetDirectoryName(filePath); //返回文件所在目录
    }


    /// <summary>
    /// 更改文件名 注意：是文件全路径
    /// </summary>
    /// <param name="srcFileName">原文件名  注意：是文件全路径</param>
    /// <param name="destFileName">更改文件名  注意：是文件全路径</param>
    public static bool ChangeFileName(string srcFileName, string destFileName)
    {
        return MoveFile(srcFileName, destFileName);
        //System.IO.File.Move(srcFileName,destFileName);
    }

    /// <summary>
    /// 移动文件
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="targetPath"></param>
    public static bool MoveFile(string sourcePath, string targetPath)
    {
        bool isMove = false;
        if (Path.IsPathFullyQualified(targetPath))
        {
            var fileInfo = new FileInfo(sourcePath);
            fileInfo.MoveTo(targetPath);
            isMove = true;
        }
        return isMove;
        //File.Move(sourcePath, targetPath);
    }

    /// <summary>
    /// 根据路径创建文件夹
    /// </summary>
    /// <param name="path"></param>
    public static void CreateDirectory(string path)
    {
        //创建文件夹路径
        //path = Application.dataPath + "/video";
        //创建文件夹路径（/../代表Application.dataPath 路径的上一级目录，/../../表示上两级目录）
        //path = Application.dataPath + "/../" + "video";
        Debug.Log(path);
        //判断文件夹路径是否存在
        if (!Directory.Exists(path))
        {
            //创建
            Directory.CreateDirectory(path);
        }
        //刷新
        //AssetDatabase.Refresh();   //using UnityEditor;
    }

    /// <summary>
    /// 创建文件夹同时创建文件 .txt
    /// </summary>
    /// <param name="directoryPath"> directoryPath = Application.dataPath + "/../" + "video"</param>
    /// <param name="filePath">directoryPath + "/path.txt"</param>
    public static void CreateDirectoryAndFile(string directoryPath, string filePath)
    {
        //创建文件夹路径（/../代表Application.dataPath 路径的上一级目录）
        //directoryPath = Application.dataPath + "/../" + "video";
        //filePath = directoryPath + "/path.txt";
        Debug.Log(directoryPath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        // FileInfo fileInfo = new FileInfo(filePath);
        // if(!fileInfo.Exists)
        //     fileInfo.Create();
        if (!File.Exists(filePath))
            File.Create(filePath);

    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="path"></param>
    public static void CreateFile(string path)
    {
        // fileInfo = new FileInfo(path);

        // if(!fileInfo.Exists)
        // {
        //     fileInfo.CreateText();
        // }

        if (!File.Exists(path))
            File.Create(path);
    }

    /// <summary>
    /// 删除指定路径文件夹
    /// </summary>
    /// <param name="directoryPath"></param>
    public static void DeleteDirectoryAndFile(string directoryPath)
    {
        //directoryPath = Application.dataPath + "/../" + "Video";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        try
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            foreach (FileSystemInfo item in files)
            {
                if (item is DirectoryInfo)//判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(item.FullName);
                    subdir.Delete(true);//删除子目录和文件
                }
                else
                {
                    File.Delete(item.FullName);//删除指定文件
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// 删除指定路径文件
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteFile(string path)
    {
        try
        {
            // fileInfo = new FileInfo(path);
            // if(fileInfo.Exists)
            // {
            //     fileInfo.Delete();
            // }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Data);
        }



    }

    /// <summary>
    /// 读取文件转为string
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string ReadFile(string filePath)
    {
        string str = "";
        try
        {
            StreamReader sr = new StreamReader(filePath);
            //如果内容为空，返回为-1
            if (sr.Peek() != -1)
            {
                str = sr.ReadToEnd();
                sr.Close();
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log($"error = {ex}");
        }
        return str;
    }

    /// <summary>
    /// 获取路径下所有文件以及子文件夹中文件
    /// </summary>
    /// <param name="path">全路径根目录</param>
    /// <param name="FileList">存放所有文件的全路径</param>
    /// <returns></returns>
    public static List<string> GetFile(string path, List<string> FileList)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] fil = dir.GetFiles();
        DirectoryInfo[] dii = dir.GetDirectories();
        foreach (FileInfo f in fil)
        {
            //int size = Convert.ToInt32(f.Length);
            long size = f.Length;
            FileList.Add(f.FullName);//添加文件路径到列表中
        }
        //获取子文件夹内的文件列表，递归遍历
        foreach (DirectoryInfo d in dii)
        {
            GetFile(d.FullName, FileList);
        }
        return FileList;
    }

    /// <summary>
    /// 递归获取指定类型文件,包含子文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <param name="extName"></param>
    public static List<FileInfo> GetFiles(string path, string extName)
    {
        List<FileInfo> lst = new List<FileInfo>();
        try
        {
            string[] dir = Directory.GetDirectories(path); //文件夹列表
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] file = dirInfo.GetFiles();
            //FileInfo[] file = Directory.GetFiles(path); //文件列表
            if (file.Length != 0 || dir.Length != 0) //当前目录文件或文件夹不为空
            {
                foreach (FileInfo f in file) //显示当前目录所有文件
                {
                    if (extName.ToLower().IndexOf(f.Extension.ToLower()) >= 0)
                    {
                        lst.Add(f);
                    }
                }
                foreach (string d in dir)
                {
                    GetFiles(d, extName);//递归
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            //throw ex;
        }

        return lst;
    }

    /// <summary>
    /// 获取指定文件夹下 是否存在该名 并添加(Clone)  主要用于文件新建
    /// </summary>
    /// <param name="path">文件夹路径</param>
    /// <param name="fileName">文件名</param>
    /// <param name="ext">文件后缀</param>
    /// <returns></returns>
    public static string GetFileExist(string path, string fileName, string ext)
    {
        string name = GetFileNameWithoutExtension(fileName);
        try
        {
            while (true)
            {
                FileInfo info = new FileInfo(Path.Combine(path, fileName + ext));
                if (info.Exists)
                {
                    fileName += "(Clone)";
                }
                else
                {
                    name = fileName;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            //throw ex;
        }

        return name;
    }


    /// <summary>
    /// 写入文件内容
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    public static void WriteFileContent(string filePath, string content)
    {
        //ClearTxtFileContent(filePath);  //清空
        try
        {

            var sw = new StreamWriter(filePath);
            sw.WriteLine("");
            sw.WriteLine(content);
            sw.Close();
            // StreamWriter sw = new StreamWriter(filePath);
            // //sw.Write("");
            // sw.Write(content);
            // sw.Close();
        }
        catch (Exception ex)
        {
            Debug.Log($"error = {ex}");
        }
    }

    /// <summary>
    /// 异步写入文件内容
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    public static async void AsyncWriteFileContent(string filePath, string content)
    {
        try
        {
            StreamWriter sw = new StreamWriter(filePath, false);

            await sw.WriteLineAsync(content);
            sw.Close();
        }
        catch (Exception ex)
        {
            Debug.Log($"error = {ex}");
        }
    }

    /// <summary>
    /// 清空文件内容  txt
    /// </summary>
    /// <param name="filePath"></param>
    public static void ClearTxtFileContent(string filePath)
    {
        try
        {
            StreamWriter sw = new StreamWriter(filePath, false);
            sw.Write("");
            sw.Close();

        }
        catch (Exception ex)
        {
            Debug.Log($"error = {ex}");
        }
    }

    /// <summary>
    /// 接收到byte数组数据，转换为文件（例如txt文件，mp3文件，jpg文件）
    /// </summary>
    /// <param name="fileName">路径或名字</param>
    /// <param name="bytes"></param>
    /// <param name="length"></param>
    public static void ChangeByteToFile(string fileName, byte[] bytes, int length)
    {
        FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
        fs.Write(bytes, 0, length);
        fs.Close();
    }

    /// <summary>
    /// 把文件（例如txt文件，mp3文件，jpg文件）转为byte数组数据，发送给服务端
    /// </summary>
    /// <param name="fileName">路径或名字</param>
    /// <returns></returns>
    public static byte[] ChangeFileToByte(string fileName)
    {
        FileStream fs = new FileStream(fileName, FileMode.Open);
        //获取FileStream的长度，开辟对应的byte内存空间
        byte[] data = new byte[fs.Length];
        //因为data是数组，数组是引用类型，这里data作为参数，fs.Read运行后，外层的data也能获取到对应数据，所以这里可以return data数据
        fs.Read(data, 0, data.Length);
        fs.Close();
        return data;
    }


    /// <summary>
    /// 将文件发送给服务端(字节流形式) ——示例
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static IEnumerator UploadFileRequest(string filePath, string url, string token, Action<string> _action)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", ChangeFileToByte(filePath));
        //string token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMTMxNTk1NSIsInFpYW9odU5ld1VzZXIiOjAsInVzZXJfbmFtZSI6IjE4MjE4MjQyMzM3IiwibW9iaWxlIjoiMTgyMTgyNDIzMzciLCJoYXNSaWdodCI6MCwidmVyc2lvbiI6IiIsImF1dGhvcml0aWVzIjpbIk4vQSJdLCJjbGllbnRfaWQiOiJxaC10ZXNsYS1hcHAiLCJhdWQiOlsicWgtZWluc3RlaW4tcmVzdCIsInFoLXF1ZXRlbGV0LXJlc3RkYXRhIiwicWgtdGVzbGEtcmVzdCJdLCJzY29wZSI6WyJ0cnVzdCJdLCJuaWNrbmFtZSI6IueUqOaItzAzM0RDRjAiLCJhdXRob3JTdGF0dXMiOiIwIiwiZXhwIjoxNjU0MTM3NzYxLCJ1Y29kZSI6IjE2OTQxNzA2MDIiLCJqdGkiOiJhNzJmY2Y5Mi01NzUwLTQyMzktYjVlMi02Y2ViYzk0YjM4NTQifQ.P5STi7KuE78sjpDRIIIjDtmZ8sdv54yHRNDn0Dbmh0g";
        using (UnityWebRequest uwr = UnityWebRequest.Post(url, form))
        {
            uwr.SetRequestHeader("Authorization", token);
            yield return uwr.SendWebRequest();
            //if (uwr.isNetworkError || uwr.isHttpError) //过时
            if (uwr.result == UnityWebRequest.Result.DataProcessingError
                || uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("文件上传失败 = " + uwr.error);
            }
            else
            {
                Debug.Log("文件上传成功，结果 = " + uwr.downloadHandler.text);
                _action?.Invoke(uwr.downloadHandler.text);
            }
        }
    }



    /// <summary>
    /// StartCoroutine(DownloadAndSave(hhh, saveName, null));//保存文件。（路径，保存为名字）
    /// </summary>
    /// <param name="url"></param>
    /// <param name="name"></param>
    /// <param name="Finish"></param>
    /// <returns></returns>
    public static IEnumerator DownloadAndSave(string url, string name, Action<bool, string> Finish = null)//路径，名字
    {
        url = Uri.EscapeUriString(url);
        //url = "D:/Others/Dr/Project/U3D/1.3System/SqlV1/Assets/StreamingAssets/1-视频-cat";
        Debug.Log(url);
        string Loading = string.Empty;
        bool b = false;
        //WWW www = new WWW(url);
        UnityWebRequest www = new UnityWebRequest(url);
        if (www.error != null)
        {
            Debug.Log("error:" + www.error);
        }
        while (!www.isDone)
        {
            Debug.Log("没有完成");
            //Loading = (((int)(www.progress * 100)) % 100) + "%";
            Loading = (((int)(www.downloadProgress * 100)) % 100) + "%";
            if (Finish != null)
            {
                Finish(b, Loading);
            }
            yield return 1;
        }
        if (www.isDone)
        {
            Debug.Log("完成了");
            //Loading = "100%";
            //byte[] bytes = www.bytes;
            byte[] bytes = www.downloadHandler.data;
            b = SaveAssets(Application.streamingAssetsPath, name, bytes);
            if (Finish != null)
            {
                Finish(b, Loading);
            }
        }
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static bool SaveAssets(string path, string name, byte[] bytes)//（保存路径，名字）
    {
        Stream sw;
        FileInfo t = new FileInfo(path + "//" + name);//name需要为完整格式"DLAM.jpg"或者"DLAM.MP4"
        if (!t.Exists)
        {
            try
            {
                sw = t.Create();
                sw.Write(bytes, 0, bytes.Length);
                sw.Close();
                sw.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    #endregion

    #region 字符串操作

    /// <summary>
    /// 分割字符串 开始0 结束 ','  去掉 ',' 返回字符数组
    /// </summary>
    /// <param name="stringValue"></param>
    /// <returns></returns>
    public static string[] StringToStrings(string stringValue)
    {
        Debug.Log(stringValue);
        if (!string.IsNullOrEmpty(stringValue))
        {
            //var strArray = stringValue.TrimStart().TrimEnd(',').Split(',');
            var strArray = stringValue.Split(',');
            return strArray;
        }

        return null;
    }

    /// <summary>
    /// string To vector3  字符串转矢量(向量)
    /// </summary>
    /// <param name="stringValue"></param>
    /// <returns></returns>
    public static Vector3 StringToVector3(string stringValue)
    {
        Vector3 vectorValue = Vector3.zero;
        if (!string.IsNullOrEmpty(stringValue))
        {
            string[] xyz = stringValue.TrimStart('(').TrimEnd(')').Split(',');
            vectorValue.x = StringToFloat(xyz[0]);
            vectorValue.y = StringToFloat(xyz[1]);
            vectorValue.z = StringToFloat(xyz[2]);
        }


        return vectorValue;
    }

    /// <summary>
    /// string to float
    /// </summary>
    /// <param name="stringValue"></param>
    /// <returns></returns>
    public static float StringToFloat(string stringValue)
    {
        float floatValue = 0;
        try
        {
            floatValue = float.Parse(stringValue, CultureInfo.InvariantCulture);
            return floatValue;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// 去掉字符串中的数字
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string RemoveNumber(string key)
    {
        return System.Text.RegularExpressions.Regex.Replace(key, @"\d", "");
    }



    /// <summary>
    /// 去掉字符串中的非数字
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string RemoveNotNumber(string key)
    {
        return System.Text.RegularExpressions.Regex.Replace(key, @"[^\d]*", "");
    }

    /// <summary>
    /// 去掉字符串中指定字符
    /// </summary>
    /// <param name="key"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static string RemoveTargetString(string key, string target)
    {
        return System.Text.RegularExpressions.Regex.Replace(key, target, "");
    }

    #endregion
}

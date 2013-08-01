using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace MoCommon
{
   public class IsolatedStorageHelper
    {
       /// <summary>
       /// Save Key Value Pair object to local isolatestorage 
       /// </summary>
       /// <param name="key">Key</param>
       /// <param name="value">Value[Object]</param>
       public static void IsolatedStorageSaveObject(string key, object value)
       {
           IsolatedStorageSettings saveToLocalSetting = IsolatedStorageSettings.ApplicationSettings;
           if (CheckSaveValueIsExist(key))
               RemoveLocalSaveObject(key);
           saveToLocalSetting.Add(key, value);
           saveToLocalSetting.Save();
       }


       /// <summary>
       /// Check Save Key Value is exist 
       /// </summary>
       /// <param name="key">Key</param>
       /// <returns>Is Exist</returns>
       public static bool CheckSaveValueIsExist(string key)
       {
           bool isExist=false;
           IsolatedStorageSettings saveToLocalSetting = IsolatedStorageSettings.ApplicationSettings;
           if (saveToLocalSetting.Contains(key))
               isExist = true;
           return isExist;
       }



       /// <summary>
       /// Remove Save Key Value Pair from local
       /// </summary>
       /// <param name="key">key</param>
       public static void RemoveLocalSaveObject(string key)
       {
           IsolatedStorageSettings saveToLocalSetting = IsolatedStorageSettings.ApplicationSettings;
           if (CheckSaveValueIsExist(key))
               saveToLocalSetting.Remove(key);
           saveToLocalSetting.Save();
       }



       /// <summary>
       /// Read IsolateStorage Save Object
       /// </summary>
       /// <param name="key">Key</param>
       /// <returns>Value</returns>
       public static object ReadSaveObjectByKey(string key)
       {
           object readObj = null;
           IsolatedStorageSettings isolateStorageSet = IsolatedStorageSettings.ApplicationSettings;
           if (CheckSaveValueIsExist(key))
                readObj=isolateStorageSet[key];
           return readObj;
       }



       /// <summary>
       /// Check Currnet File in back directory is Exist
       /// </summary>
       /// <param name="filename">File Name</param>
       /// <returns>is exist</returns>
       public static bool CheckFileIsExist(string filename)
       {
           bool isExist = false;
           if (string.IsNullOrEmpty(filename))
               return isExist;

           string filePath = filename+".txt";
           IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
           if (isolatedStorageFile.FileExists(filePath))
               isExist = true;//file exist
           return isExist;
       }


   
       /// <summary>
       /// Write Object To Local IsolateStorage File
       /// </summary>
       /// <param name="filename">File Name</param>
       /// <param name="saveObject">Save Object</param>
       public static void WriteObjectToLocalFile(string filename,object saveObject)
       {
           if (string.IsNullOrEmpty(filename) || saveObject==null)
               return;           

           IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
           string filePath = filename + ".txt";
           if (CheckFileIsExist(filename))
               isolatedStorageFile.DeleteFile(filePath);

           using (IsolatedStorageFileStream writeStream = new IsolatedStorageFileStream(filePath, FileMode.OpenOrCreate, isolatedStorageFile))
           {
               using (StreamWriter streamWriter = new StreamWriter(writeStream))
               {
                   string jsonStr = JsonConvert.SerializeObject(saveObject);
                   streamWriter.Write(jsonStr);
               }
           }
       }


       /// <summary>
       /// Read Save File from local file
       /// </summary>
       /// <param name="filename">Filename</param>
       /// <returns>Json String</returns>
       public static string ReadSaveJsonFromFile(string filename)
       {
           string saveJsonStr = string.Empty;
           IsolatedStorageFile isolateStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
           string filePath = filename + ".txt";
           if (CheckFileIsExist(filename))
           {
               using (IsolatedStorageFileStream readStream = new IsolatedStorageFileStream(filePath, FileMode.Open, isolateStorageFile))
               {
                   readStream.Seek(0, SeekOrigin.Begin);
                   using (StreamReader streamReader = new StreamReader(readStream))
                   {
                       saveJsonStr = streamReader.ReadToEnd();
                   }
               }
           }     
           return saveJsonStr;
       }




      
  
    }
}

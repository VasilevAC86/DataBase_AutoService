using System;
using System.IO;

namespace DataBase_AutoService
{
    internal class FSWork // Чтобы база не задвайвалась
    {
        static public bool IsFileExist(string path)
        {
            bool result = false;
            if (File.Exists(path)) 
            { 
                result = true;
            }            
            return result;
        }
    }
}

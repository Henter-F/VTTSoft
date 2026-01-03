using System;
using System.Collections.Generic;
using System.ICore.Log;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace System.ICore.ConfigSave
{
    public static class AccessDatSerializer
    {
        public static object LockObject = new object();

        public static bool ObjectToDat(string filePath, object sourceObj)
        {
            lock (LockObject)
            {
                if (string.IsNullOrWhiteSpace(filePath) || sourceObj == null)
                {
                    return false;
                }

                string path = filePath.Substring(0, filePath.LastIndexOf("\\"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                try
                {
                    File.Delete(filePath);
                    using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.Serialize(fileStream, sourceObj);
                        fileStream.Close();
                    }
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public static object DatToObject(string filePath)
        {
            lock (LockObject)
            {
                object result = null;
                if (File.Exists(filePath))
                {
                    try
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            fileStream.Position = 0L;
                            result = binaryFormatter.Deserialize(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUitl.LogError(ex.ToString());
                        return null;
                    }

                    return result;
                }

                return null;
            }
        }

        public static T DatToObject<T>(string filePath)
        {
            return (T)DatToObject(filePath);
        }
    }
}

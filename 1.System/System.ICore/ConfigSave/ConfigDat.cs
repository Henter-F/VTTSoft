using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ICore.ConfigSave
{
    [Serializable]
    public class ConfigDat
    {
        private string folder = "DefaultFolder";

        private string name = string.Empty;

        [Browsable(false)]
        public string Folder
        {
            get
            {
                return folder;
            }
            set
            {
                folder = value;
            }
        }

        [Browsable(false)]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public bool Save()
        {
            try
            {
                return AccessDatSerializer.ObjectToDat(GetFilePath(), this);
            }
            catch
            {
                return false;
            }
        }

        public object Read()
        {
            try
            {
                string filePath = GetFilePath();
                if (File.Exists(filePath))
                {
                    return AccessDatSerializer.DatToObject(filePath);
                }

                Save();
                return AccessDatSerializer.DatToObject(filePath);
            }
            catch
            {
                return null;
            }
        }

        public T Read<T>()
        {
            try
            {
                return (T)Read();
            }
            catch
            {
                return default(T);
            }
        }

        public string GetFilePath()
        {
            try
            {
                return Environment.CurrentDirectory + "\\Config\\" + folder + "\\" + (string.IsNullOrEmpty(name) ? GetType().Name : name) + ".Dat";
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

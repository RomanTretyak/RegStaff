using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

namespace Common
{
    class cLogger
    {
        //для теста
        public static string DirectoryName = @"C:\Inetpub\wwwroot\";
        public static string FileNameMessage = @"Message.log";
        public static string FileNameError = @"Error.log";

        private static cLogger Flog = null;
        // возвращает указатель на самого себя это так чтобы не писать везде создание объекта этого класса
        public static cLogger getLogger()
        {
            if (Flog == null)
            {
                Flog = new cLogger();
                FileNameError = FileNameError.ToString().Replace(".log", "")
                    + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + ".log";
            }
            return Flog;
        }

        protected cLogger()
        {
        }

        public bool logError(string Amessage)
        {
            if (DirectoryName.Equals(""))
            { return false; }

            StreamWriter psw;

            try
            {
                psw = new StreamWriter(DirectoryName + FileNameError, true, Encoding.Default);
            }
            catch
            {
                return false;
            }

            try
            {
                psw.WriteLine(DateTime.Now.ToString() + " : " + Amessage);
            }
            catch
            {
                return false;
            }
            finally
            {
                psw.Close();
                psw.Dispose();
                psw = null;
            }
            return true;
        }

        public bool logMessage(string Amessage)
        {
            if (DirectoryName.Equals(""))
            { return false; }

            StreamWriter psw;

            try
            {
                psw = new StreamWriter(DirectoryName + FileNameMessage, true, Encoding.Default);
            }
            catch
            {
                return false;
            }

            try
            {
                if (Amessage.Equals(""))
                {
                    psw.WriteLine("");
                }
                else
                {
                    psw.WriteLine(DateTime.Now.ToString() + " : " + Amessage);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                psw.Close();
                psw.Dispose();
                psw = null;
            }
            return true;
        }
    }
}

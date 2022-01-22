using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilidades
{
    public static  class Encriptacion
    {
        public static int Iteraciones = 10000;
        public static byte[] key = new byte[32]
        {
      (byte) 233,
      (byte) 12,
      (byte) 240,
      (byte) 145,
      (byte) 69,
      (byte) 67,
      (byte) 138,
      (byte) 113,
      (byte) 49,
      (byte) 121,
      (byte) 4,
      (byte) 250,
      (byte) 235,
      (byte) 173,
      (byte) 17,
      (byte) 16,
      (byte) 90,
      (byte) 78,
      (byte) 147,
      (byte) 125,
      (byte) 8,
      (byte) 172,
      (byte) 11,
      (byte) 99,
      (byte) 56,
      (byte) 134,
      (byte) 150,
      (byte) 163,
      (byte) 79,
      (byte) 113,
      (byte) 22,
      (byte) 236
        };

        private static byte[] Encriptar(string strEncriptar, byte[] bytPK)
        {
            Rijndael rijndael = Rijndael.Create();
            rijndael.Mode = CipherMode.CBC;
            byte[] numArray1 = (byte[])null;
            try
            {
                rijndael.Key = bytPK;
                rijndael.GenerateIV();
                byte[] bytes = Encoding.UTF8.GetBytes(strEncriptar);
                byte[] numArray2 = rijndael.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
                numArray1 = new byte[rijndael.IV.Length + numArray2.Length];
                rijndael.IV.CopyTo((Array)numArray1, 0);
                numArray2.CopyTo((Array)numArray1, rijndael.IV.Length);
            }
            catch
            {
            }
            finally
            {
                rijndael.Clear();
            }
            return numArray1;
        }

        private static string Desencriptar(byte[] bytDesEncriptar, byte[] bytPK)
        {
            Rijndael rijndael = Rijndael.Create();
            byte[] numArray = new byte[rijndael.IV.Length];
            byte[] inputBuffer = new byte[bytDesEncriptar.Length - rijndael.IV.Length];
            string empty = string.Empty;
            try
            {
                rijndael.Key = bytPK;
                Array.Copy((Array)bytDesEncriptar, (Array)numArray, numArray.Length);
                Array.Copy((Array)bytDesEncriptar, numArray.Length, (Array)inputBuffer, 0, inputBuffer.Length);
                rijndael.IV = numArray;
                empty = Encoding.UTF8.GetString(rijndael.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch
            {
            }
            finally
            {
                rijndael.Clear();
            }
            return empty;
        }

        public static byte[] Encriptar(string strEncriptar, string strPK = null) => string.IsNullOrEmpty(strPK) ? Encriptacion.Encriptar(strEncriptar, new PasswordDeriveBytes(Encriptacion.key, (byte[])null).GetBytes(32)) : Encriptacion.Encriptar(strEncriptar, new PasswordDeriveBytes(strPK, (byte[])null).GetBytes(32));

        public static string Desencriptar(byte[] bytDesEncriptar, string strPK = null) => string.IsNullOrEmpty(strPK) ? Encriptacion.Desencriptar(bytDesEncriptar, new PasswordDeriveBytes(Encriptacion.key, (byte[])null).GetBytes(32)) : Encriptacion.Desencriptar(bytDesEncriptar, new PasswordDeriveBytes(strPK, (byte[])null).GetBytes(32));

        public static string EncriptarContraseña(string strPassword, string strSalt)
        {
            if (string.IsNullOrEmpty(strPassword))
                throw new Exception("password no puede estar vacio");
            if (!string.IsNullOrEmpty(strSalt))
            {
                if (strSalt.Length > 40)
                    throw new Exception("El Salt no puede ser mayor a 40 Carácteres");
                Encriptacion.Iteraciones = 0;
                foreach (char ch in strSalt)
                    Encriptacion.Iteraciones += (int)ch;
            }
            string s = strPassword + strSalt;
            SHA256Managed shA256Managed = new SHA256Managed();
            byte[] numArray = (byte[])null;
            try
            {
                numArray = Encoding.UTF8.GetBytes(s);
                for (int index = 0; index <= Encriptacion.Iteraciones - 1; ++index)
                    numArray = shA256Managed.ComputeHash(numArray);
            }
            finally
            {
                shA256Managed.Clear();
            }
            return Convert.ToBase64String(numArray);
        }

        public static string ObtenerValorSplit(string[] parametros, string key)
        {
            string[] strArray = ((IEnumerable<string>)parametros).Where<string>((Func<string, bool>)(x => x.Contains(key))).Select<string, string[]>((Func<string, string[]>)(x => x.Split('='))).FirstOrDefault<string[]>();
            return strArray?[1].Substring(0, strArray[1].Length - 1);
        }
    }
}

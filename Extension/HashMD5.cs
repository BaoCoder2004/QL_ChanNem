﻿using System.Security.Cryptography;
using System.Text;
namespace BTL_TKWeb.Extension
{
    public static class HashMD5
    {
        public static string ToMD5(this string input)
        {
            MD5CryptoServiceProvider md5 =new MD5CryptoServiceProvider();
            byte[] bHash=md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sbHash= new StringBuilder();
            foreach (byte b in bHash) sbHash.Append(String.Format("{0:x2}",b));
            return sbHash.ToString();
        }
    }
}

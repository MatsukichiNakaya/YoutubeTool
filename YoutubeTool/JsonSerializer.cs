// 必須ライブラリ
// System.Runtime.Serialization.dll

// Project.Text - Text.cs

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Project.Serialization.Json
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonSerializser
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="src"></param>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //public static Boolean Save<T>(T src, String file) where T : class
        //{

        //    return true;
        //}

        /// <summary>
        /// Jsonファイルを読み取ってシリアライズ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Load<T>(String filePath) where T : class
        {
            return ConvertToClass<T>(IO.TextFile.Read(filePath, Encoding.UTF8));
        }

        /// <summary>
        /// JsonData文字列から指定のクラスへ変換する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static T ConvertToClass<T>(String jsonData) where T : class
        {
            var result = default(T);
            if (jsonData == null) { return result; }

            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var memorystream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData)))
            {
                result = (T)serializer.ReadObject(memorystream);
            }

            return result;
        }
    }
}
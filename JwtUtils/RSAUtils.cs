using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Zeeko.UtilsPack;

namespace JwtUtils
{
    public static class RsaUtils
    {
        /// <summary>
        /// 从本地文件中读取用来签发 Token 的 RSA Key
        /// </summary>
        /// <param name="filePath">存放密钥的文件夹路径</param>
        /// <param name="withPrivate"></param>
        /// <param name="keyParameters"></param>
        /// <returns></returns>
        public static bool TryGetKeyParameters(string filePath, bool withPrivate, out RSAParameters keyParameters)
        {
            string filename = withPrivate ? "key.json" : "key.public.json";
            filePath = Path.Combine(filePath, filename);
            keyParameters = default(RSAParameters);
            if (File.Exists(filePath) == false) return false;
            keyParameters = JsonConvert.DeserializeObject<RsaParameterStorage>(File.ReadAllText(filePath)).Map().To<RSAParameters>();
            return true;
        }

        /// <summary>
        /// 生成并保存 RSA 公钥与私钥
        /// </summary>
        /// <param name="filePath">存放密钥的文件夹路径</param>
        /// <returns></returns>
        public static RSAParameters GenerateAndSaveKey(string filePath)
        {
            RSAParameters publicKeys, privateKeys;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    privateKeys = rsa.ExportParameters(true);
                    publicKeys = rsa.ExportParameters(false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            File.WriteAllText(Path.Combine(filePath, "key.json"), privateKeys.ToJsonString());
            File.WriteAllText(Path.Combine(filePath, "key.public.json"), publicKeys.ToJsonString());
            return privateKeys;
        }

        class RsaParameterStorage
        {
            public byte[] D { get; set; }
            public byte[] DP { get; set; }
            public byte[] DQ { get; set; }
            public byte[] Exponent { get; set; }
            public byte[] InverseQ { get; set; }
            public byte[] Modulus { get; set; }
            public byte[] P { get; set; }
            public byte[] Q { get; set; }
        }

        static string ToJsonString(this RSAParameters parameters)
        {
            var content = parameters.Map().To<RsaParameterStorage>();
            return JsonConvert.SerializeObject(content);
        }

        //public static RSAParameters ReadParametersFromJson(string json)
        //{
        //    var storage = JsonConvert.DeserializeObject<RsaParameterStorage>(json);
        //    return new RSAParameters
        //    {
        //        D = storage.D,
        //        DP = storage.DP,
        //        DQ = storage.DQ,

        //    }
        //}
    }
}

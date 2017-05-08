using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace JwtUtils
{
    public static class RSAUtils
    {
        /// <summary>
        /// 从本地文件中读取用来签发 Token 的 RSA Key
        /// </summary>
        /// <param name="filePath">存放密钥的文件夹路径</param>
        /// <param name="keyParameters"></param>
        /// <returns></returns>
        public static bool TryGetKeyParameters(string filePath, out RSAParameters keyParameters)
        {
            keyParameters = default(RSAParameters);
            if (File.Exists(filePath) == false) return false;
            keyParameters = JsonConvert.DeserializeObject<RSAParameters>(File.ReadAllText(Path.Combine(filePath, "key.json")));
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
            File.WriteAllText(Path.Combine(filePath, "key.json"), JsonConvert.SerializeObject(privateKeys));
            File.WriteAllText(Path.Combine(filePath, "key.public.json"), JsonConvert.SerializeObject(publicKeys));
            return privateKeys;
        }
    }
}

using System;
using System.IO;
using System.Linq;

namespace YH.Etms.Settlement.Api.Infrastructure.Configuration
{
    /// <summary>
    /// 这个类用来查找web项目的根路径
    /// 单元测试（查看视图）和实体框架核心命令行命令（查找连接字符串）
    /// </summary>
    public static class WebContentDirectoryFinder
    {
        public static string CalculateContentRootFolder()
        {
            var assemblyDirectoryPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            if (assemblyDirectoryPath == null)
            {
                throw new Exception("未找到程序集 YH.Etms.Settlement.Api.dll");
            }

            var directoryInfo = new DirectoryInfo(assemblyDirectoryPath);
            //递归向上找csproj所在文件夹
            while (!DirectoryContains(directoryInfo.FullName, ".csproj"))
            {
                if (directoryInfo.Parent == null)
                    throw new Exception("未找到项目文件 YH.Etms.Settlement.Api.csproj");
                directoryInfo = directoryInfo.Parent;
            }
            return directoryInfo.FullName;
        }

        private static bool DirectoryContains(string directory, string extensionName)
        {
            return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetExtension(filePath), extensionName));
        }
    }
}

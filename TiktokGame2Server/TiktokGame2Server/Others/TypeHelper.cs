using System;
using System.Linq;
using System.Reflection;

public static class TypeHelper
{
    public static object CreateInstanceByClassName(string className, object[] ctorArgs)
    {
        // 遍历所有已加载程序集
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // 查找以 className 结尾的类型（防止命名空间不同）
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == className);
            if (type != null)
            {
                return Activator.CreateInstance(type, ctorArgs);
            }
        }
        throw new Exception($"未找到类型: {className}");
    }
}
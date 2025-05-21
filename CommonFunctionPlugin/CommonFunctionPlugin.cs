using System;
using System.Collections.Generic;
using System.Windows;
using PluginEntry;

public class CommonFunctionPlugin
{
    public Dictionary<string, Func<PluginParameter[], object>> FunctionDict { get; set; } = new Dictionary<string, Func<PluginParameter[], object>>();

    public void Load()
    {
        // 注册功能
        FunctionDict.Add("Load", Load);
    }

    public object Run(string functionKey, params PluginParameter[] parameters)
    {
        if (FunctionDict.ContainsKey(functionKey))
        {
            try
            {
                return FunctionDict[functionKey].Invoke(parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行功能出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        return null;
    }


    private object Load(PluginParameter[] parameters)
    {
        return null;
    }

}

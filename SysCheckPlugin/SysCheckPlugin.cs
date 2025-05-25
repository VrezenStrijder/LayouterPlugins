using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using PluginEntry;

namespace Layouter.Plugins.SysCheckPlugin
{
    public class SysCheckPlugin : IPlugin
    {
        public Dictionary<string, Func<PluginParameter[], object>> FunctionDict { get; set; } = new Dictionary<string, Func<PluginParameter[], object>>();

        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        private PerformanceCounter swapCounter;
        private bool isRegister = false;

        public string Name => "系统资源监控";


        public void Register()
        {
            if (isRegister)
            {
                return;
            }

            FunctionDict.Add("CPU使用率", GetCPUUsage);
            FunctionDict.Add("内存使用率", GetRamUsage);
            FunctionDict.Add("缓存使用率", GetSwapUsage);

            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
                swapCounter = new PerformanceCounter("Paging File", "% Usage", "_Total");
                isRegister = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化性能计数器失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private object GetCPUUsage(PluginParameter[] parameters)
        {
            return $"{Math.Round(cpuCounter.NextValue(), 2)}";
        }

        private object GetRamUsage(PluginParameter[] parameters)
        {
            return $"{Math.Round(ramCounter.NextValue(), 2)}";
        }

        private object GetSwapUsage(PluginParameter[] parameters)
        {
            return $"{Math.Round(swapCounter.NextValue(), 2)}";
        }

        public void Unregister()
        {
            FunctionDict.Clear();
            cpuCounter?.Dispose();
            ramCounter?.Dispose();
            swapCounter?.Dispose();
            isRegister = false;
        }

        public Dictionary<string, List<PluginParameter>> GetParameterDescriptions()
        {
            return null;
        }
    }
}
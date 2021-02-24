using System;
using System.Diagnostics;
using System.Management.Automation;

namespace UnifyEditor_Setup.CloudAPI
{
    public class PowerShellHelper
    {
        /// <summary>
        /// 执行脚本,OOS
        /// </summary>
        public static PSExcuteResult RunOOSPS()
        {
            try
            {
                string PSPolicy = "Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force;& ";
                string rootPath = AppDomain.CurrentDomain.BaseDirectory;
                string check_vm_ps = $"{rootPath}ps1\\OOS_Setup.ps1 -root '{rootPath}'";
                return RunScript(PSPolicy + check_vm_ps);
            }
            catch (Exception ex)
            {
                return new PSExcuteResult() { STATE = "Error", DATA = "Exception：" + ex.Message };
            }
        }

        /// <summary>
        /// 执行脚本,OOS
        /// </summary>
        public static PSExcuteResult RunCreateIISPS(string siteName, int sitePort)
        {
            try
            {
                string PSPolicy = "Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force;& ";
                string rootPath = AppDomain.CurrentDomain.BaseDirectory;
                string check_vm_ps = $"{rootPath}ps1\\CreateIIS.ps1 -Name '{siteName}' -Port '{sitePort}' -root '{rootPath}'";
                return RunScript(PSPolicy + check_vm_ps);
            }
            catch (Exception ex)
            {
                return new PSExcuteResult() { STATE = "Error", DATA = "Exception：" + ex.Message };
            }
        }

        /// <summary>
        /// 执行脚本,创建场
        /// </summary>
        public static PSExcuteResult RunCreateFarmPS(string IntetnalURL, string ExternalUrl, string CertificateName)
        {
            try
            {
                string PSPolicy = "Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force;& ";
                string rootPath = AppDomain.CurrentDomain.BaseDirectory;
                string check_vm_ps = $"{rootPath}ps1\\CreateFarm.ps1 -IntetnalURL '{IntetnalURL}' -ExternalUrl '{ExternalUrl}' -CertificateName '{CertificateName}' -root '{rootPath}'";
                return RunScript(PSPolicy + check_vm_ps);
            }
            catch (Exception ex)
            {
                return new PSExcuteResult() { STATE = "Error", DATA = "Exception：" + ex.Message };
            }
        }

        /// <summary>
        /// 执行脚本,安装IIS
        /// </summary>
        public static PSExcuteResult Run2012R2IISPS()
        {
            try
            {
                string PSPolicy = "Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force;& ";
                string check_vm_ps1 = AppDomain.CurrentDomain.BaseDirectory + "ps1\\2012_R2_IIS_Setup.ps1";
                return RunScript(PSPolicy + check_vm_ps1);
            }
            catch (Exception ex)
            {
                return new PSExcuteResult() { STATE = "Error", DATA = "Exception：" + ex.Message };
            }
        }

        /// <summary>
        /// 执行脚本,安装IIS
        /// </summary>
        public static PSExcuteResult Run2016IISPS()
        {
            try
            {
                string PSPolicy = "Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force;& ";
                string check_vm_ps1 = AppDomain.CurrentDomain.BaseDirectory + "ps1\\2016_IIS_Setup.ps1";
                return RunScript(PSPolicy + check_vm_ps1);
            }
            catch (Exception ex)
            {
                return new PSExcuteResult() { STATE = "Error", DATA = "Exception：" + ex.Message };
            }
        }

        /// <summary>
        /// 执行脚本,重启计算机
        /// </summary>
        public static PSExcuteResult RunPS2RestartPC()
        {
            try
            {
                string PSPolicy = "Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force;& ";
                string check_vm_ps1 = AppDomain.CurrentDomain.BaseDirectory + "ps1\\RestartPC.ps1";
                return RunScript(PSPolicy + check_vm_ps1);
            }
            catch (Exception ex)
            {
                return new PSExcuteResult() { STATE = "Error", DATA = "Exception：" + ex.Message };
            }
        }

        private static PSExcuteResult RunScript(string scripts)
        {
            Process process = null;
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = @"powershell.exe",
                    Arguments = scripts,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process = Process.Start(startInfo);

                var errors = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(errors))
                {
                    return new PSExcuteResult() { STATE = "ERROR", DATA = errors };
                }

                process.WaitForExit(3600000); // 一个小时
                return new PSExcuteResult() { STATE = "OK" };
            }
            catch (Exception ex)
            {
                return new PSExcuteResult() { STATE = "ERROR", DATA = "Exception：" + ex.Message };
            }
            finally
            {
                if (!process.HasExited)
                {
                    if (process.Responding)
                        process.CloseMainWindow();
                    else
                        process.Kill();
                }
                if (process != null)
                {
                    process.Close();
                    process.Dispose();
                }
            }
        }

        /// <summary>
        /// 运行ps1脚本，返回最终json格式字符串
        /// </summary>
        /// <param name="scriptText">ps1脚本内容</param>
        /// <returns></returns>
        private static PSExcuteResult RunCMDScript(string scriptText)
        {
            PowerShell PS = PowerShell.Create();
            try
            {
                PS.AddScript(scriptText);
                var results = PS.Invoke();

                string result = string.Empty;
                if (PS.Streams.Error != null && PS.Streams.Error.Count > 0)
                {
                    foreach (var error in PS.Streams.Error)
                    {
                        result += "Error: " + error.ToString() + "\r\n";
                    }
                }

                for (int i = 0; i < results.Count; i++)
                {
                    result += "OK: " + results[i] + "\r\n";
                }
                return new PSExcuteResult() { STATE = "OK", DATA = result };
            }
            catch (Exception ex)
            {
                return new PSExcuteResult() { STATE = "ERROR", DATA = "Exception：" + ex.Message };
            }
            finally
            {
                PS.Dispose();
            }
        }

    }
}

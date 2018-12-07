﻿using BearBuildTool.Projects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using static BearBuildTool.Projects.Build;

namespace BearBuildTool.Windows
{
    class VCBuildTools:Tools.BuildTools
    {
        string VCToolPath;
        string CCompiler;
        string Linker;
        string LibraryLinker;
        string ConsoleOut;
        static bool FindKey(string key, string val, out string path)
        {
            string str = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\" + key, val, null) as string;
            if (!String.IsNullOrEmpty(str))
            {
                path = str;
                return true;
            }
            str = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + key, val, null) as string;
            if (!String.IsNullOrEmpty(str))
            {
                path = str;
                return true;
            }
            str = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Wow6432Node\\" + key, val, null) as string;
            if (!String.IsNullOrEmpty(str))
            {
                path = str;
                return true;
            }
            str = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\" + key, val, null) as string;
            if (!String.IsNullOrEmpty(str))
            {
                path = str;
                return true;
            }
            path = null;
            return false;
        }
        private string GetVS2017Path()
        {
            string path;
            if(FindKey("Microsoft\\VisualStudio\\SxS\\VS7", "15.0",out path)==false)
            {
                throw new Exception("Visual Studio 2017 неустановлена.");
            }
            return path;
        }
        private string GetVC2017Path()
        {
            string path;
            if (FindKey("Microsoft\\VisualStudio\\SxS\\VC7", "15.0", out path) == true)
            {
                if (File.Exists(Path.Combine(path, "Auxiliary", "Build", "Microsoft.VCToolsVersion.default.txt")) == false)
                {
                    throw new Exception("С++ 14.1  неустановлен.");
                }
                string version1 = File.ReadAllText(Path.Combine(path, "Auxiliary", "Build", "Microsoft.VCToolsVersion.default.txt")).Trim();
                return Path.Combine(path, "Tools", "MSVC", version1);
                
            }
            path=GetVS2017Path();
            path += "VC\\";
            if(File.Exists(Path.Combine(path, "Auxiliary", "Build", "Microsoft.VCToolsVersion.default.txt"))==false)
            {
                throw new Exception("С++ 14.1  неустановлен.");
            }
            string version = File.ReadAllText(Path.Combine(path, "Auxiliary", "Build", "Microsoft.VCToolsVersion.default.txt")).Trim();
            return Path.Combine(path, "Tools", "MSVC", version);
            
        }
        private void FindUniversalCRT(out string UniversalCRTDir, out string UniversalCRTVersion)
        {

            string[] RootKeys =
            {
                "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows Kits\\Installed Roots",
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows Kits\\Installed Roots",
                "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows Kits\\Installed Roots",
                "HKEY_CURRENT_USER\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows Kits\\Installed Roots",
            };

            List<DirectoryInfo> IncludeDirs = new List<DirectoryInfo>();
            foreach (string RootKey in RootKeys)
            {
                string IncludeDirString = Registry.GetValue(RootKey, "KitsRoot10", null) as string;
                if (IncludeDirString != null)
                {
                    DirectoryInfo IncludeDir = new DirectoryInfo(Path.Combine(IncludeDirString, "include"));
                    if (IncludeDir.Exists)
                    {
                        IncludeDirs.AddRange(IncludeDir.EnumerateDirectories());
                    }
                }
            }

            DirectoryInfo LatestIncludeDir = IncludeDirs.OrderBy(x => x.Name).LastOrDefault(n => n.Name.All(s => (s >= '0' && s <= '9') || s == '.') && Directory.Exists(n.FullName + "\\ucrt"));
            if (LatestIncludeDir == null)
            {
                UniversalCRTDir = null;
                UniversalCRTVersion = null;
            }
            else
            {
                UniversalCRTDir = LatestIncludeDir.Parent.Parent.FullName;
                UniversalCRTVersion = LatestIncludeDir.Name;
            }
        }
        private static string FindNetFxSDKExtensionInstallationFolder()
        {
            string[] Versions;
            Versions = new string[] { "4.6", "4.6.1", "4.6.2" };

            foreach (string Version in Versions)
            {
                string Result = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SDKs\NETFXSDK\" + Version, "KitsInstallationFolder", null) as string
                            ?? Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft SDKs\NETFXSDK\" + Version, "KitsInstallationFolder", null) as string;
                if (Result != null)
                {
                    return Result.TrimEnd('\\');
                }
            }

            return string.Empty;
        }
        private static string FindWindowsSDKInstallationFolder()
        {
            string Version = "v8.1";

            var Result =
                    Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Microsoft SDKs\Windows\" + Version, "InstallationFolder", null)
                ?? Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft SDKs\Windows\" + Version, "InstallationFolder", null)
                ?? Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Wow6432Node\Microsoft\Microsoft SDKs\Windows\" + Version, "InstallationFolder", null);

            if (Result == null)
            {
                throw new Exception(String.Format("Windows SDK {0} неустановлен.", Version));
            }

            return (string)Result;
        }
        public VCBuildTools()
        {
            string VCPath = GetVC2017Path();
            string UniversalCRTDir;
            string VerisonCRTDir;
            string WindowsSDKDir;
            VCToolPath = null;
            {

                if (Config.Global.Platform == Config.Platform.Win64)
                {
                    string testPath = Path.Combine(VCPath, "bin", "HostX64", "x64", "cl.exe");
                    if (File.Exists(testPath))
                    {
                        VCToolPath = testPath;
                    }
                    else
                        throw new Exception("Нет x64 битного компилятора");

                }
                else
                {


                    string testPath = Path.Combine(VCPath, "bin", "HostX86", "x86", "cl.exe");
                    if (File.Exists(testPath))
                    {
                        VCToolPath = testPath;
                    }
                    else
                        throw new Exception("Нет x86 битного компилятора");

                }
            }
            VCToolPath = Path.GetDirectoryName(VCToolPath);
            CCompiler = Path.Combine(VCToolPath, "cl.exe");
            Linker = Path.Combine(VCToolPath, "link.exe");
            LibraryLinker = Path.Combine(VCToolPath, "lib.exe");
            WindowsSDKDir = FindWindowsSDKInstallationFolder();
            FindUniversalCRT(out UniversalCRTDir,out VerisonCRTDir);

            string Paths = Environment.GetEnvironmentVariable("PATH") ?? "";
            if (!Paths.Split(';').Any(x => String.Compare(x, VCToolPath, true) == 0))
            {
                Paths = VCToolPath + ";" + Paths;
                Environment.SetEnvironmentVariable("PATH", Paths);
            }

            List<string> includes = new List<string>();
            string path = Path.Combine(VCPath, "include");
            if (Directory.Exists(path))
            {
                includes.Add(path);
            }
            path = Path.Combine(VCPath, "atlmfc", "include") ;
            if (Directory.Exists(path))
            {
                includes.Add(path);
            }
            path = Path.Combine(VCPath, "atlmfc", "include") ;
            if (Directory.Exists(path))
            {
                includes.Add(path);
            }
            if (!String.IsNullOrEmpty(UniversalCRTDir) && !String.IsNullOrEmpty(VerisonCRTDir))
            {
                includes.Add(Path.Combine(UniversalCRTDir, "include", VerisonCRTDir, "ucrt"));
            }
            string NetFxSDKExtensionDir = FindNetFxSDKExtensionInstallationFolder();
            if (!String.IsNullOrEmpty(NetFxSDKExtensionDir))
            {
                includes.Add(Path.Combine(NetFxSDKExtensionDir, "include", "um")); // 2015
            }
            includes.Add(Path.Combine(WindowsSDKDir, "include", "shared")); // 2015
            includes.Add(Path.Combine(WindowsSDKDir, "include", "um")); // 2015
            includes.Add(Path.Combine(WindowsSDKDir, "include", "winrt")); // 2015
            string ExistingIncludePaths = Environment.GetEnvironmentVariable("INCLUDE");
            if (ExistingIncludePaths != null)
            {
                includes.AddRange(ExistingIncludePaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }

            Environment.SetEnvironmentVariable("INCLUDE", String.Join(";", includes));

            List<string> LibraryPaths = new List<string>();

            if (Config.Global.Platform == Config.Platform.Win32)
            {

                string StdLibraryDir = Path.Combine(VCPath, "lib", "x86");
                if (Directory.Exists( StdLibraryDir))
                {
                    LibraryPaths.Add(StdLibraryDir);
                }
                StdLibraryDir = Path.Combine(VCPath, "atlmfc", "lib", "x86");
                if (Directory.Exists(StdLibraryDir))
                {
                    LibraryPaths.Add(StdLibraryDir);
                }
            }
            else
            {
                string StdLibraryDir = Path.Combine(VCPath, "lib", "x64");
                if (Directory.Exists(StdLibraryDir))
                {
                    LibraryPaths.Add(StdLibraryDir);
                }
                StdLibraryDir = Path.Combine(VCPath, "atlmfc", "lib", "x64");
                if (Directory.Exists(StdLibraryDir))
                {
                    LibraryPaths.Add(StdLibraryDir);
                }
            }

            if (!string.IsNullOrEmpty(NetFxSDKExtensionDir))
            {
                if (Config.Global.Platform == Config.Platform.Win32)
                {
                    LibraryPaths.Add(Path.Combine(NetFxSDKExtensionDir, "lib", "um", "x86"));
                }
                else
                {
                    LibraryPaths.Add(Path.Combine(NetFxSDKExtensionDir, "lib", "um", "x64"));
                }
            }
            if (Config.Global.Platform == Config.Platform.Win32)
            {
                LibraryPaths.Add(Path.Combine(WindowsSDKDir, "lib", "winv6.3", "um", "x86"));
            }
            else
            {
                LibraryPaths.Add(Path.Combine(WindowsSDKDir, "lib", "winv6.3", "um", "x64"));

            }
            if (!String.IsNullOrEmpty(UniversalCRTDir) && !String.IsNullOrEmpty(VerisonCRTDir))
            {
                if (Config.Global.Platform == Config.Platform.Win32)
                {
                    LibraryPaths.Add(Path.Combine(UniversalCRTDir, "lib", VerisonCRTDir, "ucrt", "x86"));
                }
                else
                {
                    LibraryPaths.Add(Path.Combine(UniversalCRTDir, "lib", VerisonCRTDir, "ucrt", "x64"));
                }
            }
            // Add the existing library paths
            string ExistingLibraryPaths = Environment.GetEnvironmentVariable("LIB");
            if (ExistingLibraryPaths != null)
            {
                LibraryPaths.AddRange(ExistingLibraryPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
            Environment.SetEnvironmentVariable("LIB", String.Join(";", LibraryPaths));


        }
        public override void BuildStaticLibrary(List<string> objs, List<string> libs, List<string> libsPath, string outStaticLib)
        {
            string Arguments = "";
            Arguments += "/NOLOGO ";
            if (Config.Global.Configure == Config.Configure.Release)
                Arguments += "/LTCG ";
            if (Config.Global.Platform == Config.Platform.Win64)
                Arguments += "/MACHINE:x64 ";
            else
                Arguments += "/MACHINE:x86 ";
            foreach (string libpath in libsPath)
            {
                Arguments += String.Format("/LIBPATH:\"{0}\" ", libpath);
            }
            Arguments += String.Format("/OUT:\"{0}\" ", outStaticLib);
            
            List<string> listObject = new List<string>();
            
            foreach (string obj in objs)
            {
                listObject.Add(String.Format("\"{0}\"", obj));
            }
            foreach (string lib in libs)
            {
                listObject.Add(String.Format("\"{0}\"", lib));
            }
            File.WriteAllLines(outStaticLib + ".txt", listObject);
            Arguments += "@" + outStaticLib + ".txt" + " ";

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = LibraryLinker;
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WorkingDirectory = VCToolPath;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            ConsoleOut = "";
            process.Start();
            process.BeginOutputReadLine();
            process.OutputDataReceived += Process_OutputDataReceived;
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                System.Console.WriteLine("-------------------------ОТЧЁТ ОБ ОШИБКАХ-------------------------");

                System.Console.WriteLine(ConsoleOut);

                System.Console.WriteLine("-----------------------------------------------------------------");
                throw new Exception(String.Format("Ошибка компиляции {0}", process.ExitCode));
            }
        }
        public override void BuildObject(List<string> LInclude, List<string> LDefines, string pch, string pchH, bool createPCH, string source, string obj, BuildType buildType)
        {
           
            string Arguments = "";
            Arguments += "/c ";
            Arguments += "/GS ";
            if (!Config.Global.WithoutWarning)
            {
                Arguments += "/W4 ";
                Arguments += "/WX ";
            }
            else
            {
                Arguments += "/W0 ";
                Arguments += "/WX- ";
            }
            Arguments += "/Zc:wchar_t  ";
            Arguments += "/Zi  ";
            Arguments += "/Gm- ";
            Arguments += "/Zc:inline  ";
            Arguments += "/fp:precise ";
            Arguments += "/errorReport:prompt ";
            Arguments += "/Zc:forScope ";
            Arguments += "/Gd  ";
            Arguments += "/FC   ";
            Arguments += "/nologo ";
            Arguments += "/diagnostics:classic  ";
            Arguments += "/sdl- ";
            
 
            switch (Config.Global.Configure)
            {
                case Config.Configure.Debug:
                    Arguments += "/JMC ";
                    Arguments += "/Od ";
                    Arguments += "/RTC1 ";
                    Arguments += "/MDd  ";
                    Arguments += "/EHsc ";
                    break;
                case Config.Configure.Mixed:
                    Arguments += "/Gy ";
                    Arguments += "/O2 ";
                    Arguments += "/Oy- ";
                    Arguments += "/MD ";
                    Arguments += "/EHsc ";
                    break;
                case Config.Configure.Release:
                    Arguments += "/EHsc ";
                    Arguments += "/GL ";
                    Arguments += "/Gy ";
                    Arguments += "/Ox ";
                    Arguments += "/Ob2 ";
                    Arguments += "/GT ";
                    Arguments += "/Oy ";
                    Arguments += "/Oi ";
                    Arguments += "/MD ";
                    Arguments += "/Ot ";
     
                    Arguments += "/Gd ";
                    Arguments += "/Oi ";
                    Arguments += "/Oi ";
                    Arguments += "/Ot ";
                    
                    break;
            };
            Arguments += "/analyze- ";
            Arguments += "/Zc:inline ";
            Arguments += String.Format("\"{0}\" ", source);
            foreach (string define in LDefines)
            {
                Arguments += String.Format("/D \"{0}\" ", define);
            }
            foreach (string include in LInclude)
            {
                Arguments += String.Format("/I\"{0}\" ", include);
            }
            Arguments += String.Format("/Fo\"{0}\" ", obj);
            Arguments += String.Format("/Fd\"{0}\" ", Path.Combine( Path.GetDirectoryName(obj), "vc141.pdb"));
            if (pch != null)
            {
                if (createPCH)
                {
                    Arguments += String.Format("/Yc\"{0}\" ", pchH);
                }
                else
                {
                    Arguments += String.Format("/Yu\"{0}\" ", pchH);
                }
                Arguments += String.Format("/Fp\"{0}\" ", pch);
            }
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = CCompiler;
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WorkingDirectory = VCToolPath;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            ConsoleOut = "";
            process.Start();
            process.BeginOutputReadLine();

            process.OutputDataReceived +=Process_OutputDataReceived;
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                System.Console.WriteLine("-------------------------ОТЧЁТ ОБ ОШИБКАХ-------------------------");

                System.Console.WriteLine(ConsoleOut);

                System.Console.WriteLine("-----------------------------------------------------------------");
                throw new Exception(String.Format("Ошибка компиляции {0}", process.ExitCode));
            }
        }
        public override void BuildDynamicLibrary(List<string> objs, List<string> libs, List<string> libsPath, string outDynamicLib, string outStaticLib)
        {
            string Arguments = "";
            Arguments += "/DLL ";
            Arguments += "/MANIFEST:NO ";
            Arguments += "/NOLOGO ";
            if (Config.Global.Configure != Config.Configure.Release)
                Arguments += "/DEBUG ";
            Arguments += "/errorReport:prompt ";
            if (Config.Global.Platform == Config.Platform.Win64)
                Arguments += "/MACHINE:x64 ";
            else
                Arguments += "/MACHINE:x86 ";
            if (Config.Global.Configure == Config.Configure.Debug)
            {
                Arguments += "/OPT:NOREF ";
                Arguments += "/OPT:NOICF ";
            }
            else
            {
                Arguments += "/OPT:REF ";
            }
            Arguments += "/INCREMENTAL:NO ";
            foreach (string libpath in libsPath)
            {
                Arguments += String.Format("/LIBPATH:\"{0}\" ", libpath);
            }
            List<string> listObject = new List<string>();

            foreach (string obj in objs)
            {
                listObject.Add(String.Format("\"{0}\"", obj));
            }
            foreach (string lib in libs)
            {
                listObject.Add(String.Format("\"{0}\"", lib));
            }
            File.WriteAllLines(outStaticLib + ".txt", listObject);
            Arguments += "@" + outStaticLib + ".txt" + " ";
            Arguments += String.Format("/IMPLIB:\"{0}\" ", outStaticLib);
            Arguments += String.Format("/OUT:\"{0}\" ", outDynamicLib);
            Arguments += String.Format("/PDB:\"{0}\"", Path.Combine(Path.GetDirectoryName(outDynamicLib), Path.GetFileNameWithoutExtension(outDynamicLib) + ".pdb"));
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Linker;
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WorkingDirectory = VCToolPath;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            ConsoleOut = "";
            process.Start();
            process.BeginOutputReadLine();
            process.OutputDataReceived += Process_OutputDataReceived;
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                System.Console.WriteLine("-------------------------ОТЧЁТ ОБ ОШИБКАХ-------------------------");

                System.Console.WriteLine(ConsoleOut);

                System.Console.WriteLine("-----------------------------------------------------------------");
                throw new Exception(String.Format("Ошибка компиляции {0}", process.ExitCode));
            }
        }
        public override void BuildExecutable(List<string> objs, List<string> libs, List<string> libsPath, string Executable, string outStaticLib, bool Console)
        {
            string Arguments = "";
            Arguments += "/MANIFEST:NO ";
            Arguments += "/NOLOGO ";
            if(Config.Global.Configure!=Config.Configure.Release)
            Arguments += "/DEBUG ";
            Arguments += "/errorReport:prompt ";
            if(Config.Global.Platform==Config.Platform.Win64)
                Arguments += "/MACHINE:x64 ";
            else
                Arguments += "/MACHINE:x86 ";
            if (Console)
                Arguments += "/SUBSYSTEM:CONSOLE ";
            else
                Arguments += "/SUBSYSTEM:WINDOWS ";
            if (Config.Global.Configure == Config.Configure.Debug)
            {
                Arguments += "/OPT:NOREF ";
                Arguments += "/OPT:NOICF ";
            }
            else
            {
                Arguments += "/OPT:REF ";
            }
            Arguments += "/INCREMENTAL:NO ";
            foreach (string libpath in libsPath)
            {
                Arguments += String.Format("/LIBPATH:\"{0}\" ", libpath);
            }
            List<string> listObject=new List<string>();

            foreach (string obj in objs)
            {
                listObject.Add(String.Format("\"{0}\"", obj));
            }
            foreach (string lib in libs)
            {
                listObject.Add(String.Format("\"{0}\"", lib));
            }
            File.WriteAllLines(outStaticLib + ".txt", listObject);
            Arguments += "@" + outStaticLib + ".txt" + " ";
             Arguments += String.Format("/IMPLIB:\"{0}\" ", outStaticLib);
            Arguments += String.Format("/OUT:\"{0}\" ", Executable);
            Arguments += String.Format("/PDB:\"{0}\"",Path.Combine(Path.GetDirectoryName( Executable), Path.GetFileNameWithoutExtension(Executable)+".pdb"));
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Linker;
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WorkingDirectory = VCToolPath;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            ConsoleOut = "";
            process.Start();
            process.BeginOutputReadLine();
            process.OutputDataReceived += Process_OutputDataReceived;
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                System.Console.WriteLine("-------------------------ОТЧЁТ ОБ ОШИБКАХ-------------------------");

                System.Console.WriteLine(ConsoleOut);
                
                System.Console.WriteLine("-----------------------------------------------------------------");
                throw new Exception(String.Format("Ошибка компиляции {0}", process.ExitCode));
            }
        }
        public override void SetDefines(List<string> LDefines,string OutFile, BuildType buildType)
        {
            base.SetDefines(LDefines, OutFile, buildType);
            switch (buildType)
            {
                case BuildType.ConsoleExecutable:
                    LDefines.Add("_CONSOLE");
                    break;
                case BuildType.Executable:
                    LDefines.Add("_WINDOWS");
                    break;
                case BuildType.StaticLibrary:
                    LDefines.Add("_LIB");
                    break;
                case BuildType.DynamicLibrary:
                    LDefines.Add("_USRDLL");
                    LDefines.Add("_WINDOWS");
                    break;
            }
            switch (Config.Global.Configure)
            {
                case Config.Configure.Debug:
                    LDefines.Add("_DEBUG");
                    LDefines.Add("DEBUG");
                    break;
                case Config.Configure.Mixed:
                    LDefines.Add("MIXED");
                    LDefines.Add("DEBUG");
                    break;
                case Config.Configure.Release:
                    LDefines.Add("NDEBUG");
                    break;
            }
            switch (Config.Global.Platform)
            {
                case Config.Platform.Win32:
                    LDefines.Add("WINDOWS");
                    LDefines.Add("WIN32");
                    LDefines.Add("X32");
                    break;
                case Config.Platform.Win64:
                    LDefines.Add("WINDOWS");
                    LDefines.Add("X64");
                    break;
            }
       
        }
        public override void SetLibraries(List<string> libs, BuildType buildType)
        {
            libs.Add("kernel32.lib");
            libs.Add("user32.lib");
            libs.Add("gdi32.lib");
            libs.Add("winspool.lib");
            libs.Add("comdlg32.lib");
            libs.Add("advapi32.lib");
            libs.Add("shell32.lib");
            libs.Add("ole32.lib");
            libs.Add("oleaut32.lib");
            libs.Add("uuid.lib");
            libs.Add("odbc32.lib");
            libs.Add("odbccp32.lib");
        }
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                ConsoleOut+=e.Data+"\n";
            }
        }
    }
}

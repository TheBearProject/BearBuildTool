﻿using System;
using System.Collections.Generic;
using System.IO;
using BearBuildTool.Projects;

namespace BearBuildTool.VisualCode
{
    public class VCProjectFile
    {
        bool General = false;
        string Name;
        string ProjectDirectory;
        string VCDirectory;
        GenerateProjectFile GenerateProject;
        public VCProjectFile(string name, string general_name)
        {
            General = name == general_name;
            Name = name;
            ProjectDirectory = Path.Combine(Config.Global.IntermediatePath,"..", "VisualCode",general_name, name);
            if(!Directory.Exists(ProjectDirectory))
            {
                Directory.CreateDirectory(ProjectDirectory); 
            }
        
            GenerateProject = new GenerateProjectFile();
            GenerateProject.RegisterProject(name);

            VCDirectory = Path.Combine(GenerateProject.MapProjects[Name].ProjectPath, ".vscode");
            if (!Directory.Exists(VCDirectory))
            {
                Directory.CreateDirectory(VCDirectory);
            }
        }
        private void GenerateCPPInfoLinux(string name,ref List<string> projectFile,  string [] dop_defines)
        {
            var project = GenerateProject.MapProjects[Name];
 
            projectFile.Add("\t\t{");
            projectFile.Add(String.Format( "\t\t\t\"name\": \"{0}\",", name));
            projectFile.Add("\t\t\t\"includePath\": [");
            {
                int i = 1;
                List<string> list = project.Include.ToList();
                foreach (var inc in list)
                {
                    string var1 = String.Format("\t\t\t\"{0}\"", inc);
                    if (list.Count != i) var1 += ",";
                    projectFile.Add(var1);
                    i++;
                }
            }
            projectFile.Add("\t\t\t],");

            projectFile.Add("\t\t\t\"defines\": [");
            {
                int i = 1;
                List<string> list = project.Defines.ToList();
                list.AddRange(dop_defines);
                if (!Config.Global.ANSI)
                {
                    list.Add("_UNICODE");
                    list.Add("UNICODE");
                }
                foreach (var inc in list)
                {
                    string var1 = String.Format("\t\t\t\"{0}\"", inc);
                    if (list.Count != i) var1 += ",";
                    projectFile.Add(var1);
                    i++;
                }
            }
            projectFile.Add("\t\t\t],");
            projectFile.Add("\t\t\t\"compilerPath\": \"/usr/bin/gcc\",");
            projectFile.Add("\t\t\t\"cStandard\": \"c11\",");
            projectFile.Add("\t\t\t\"cppStandard\": \"c++14\",");
            projectFile.Add("\t\t\t\"intelliSenseMode\": \"gcc-x64\"");
            projectFile.Add("\t\t}");
    
        }
        private void GenerateTaskLinux( ref List<string> projectFile, string cmd_name, string name, string configure, string platform)
        {
            projectFile.Add("\t\t{");
            projectFile.Add(String.Format("\t\t\t\"label\": \"{0}\",", cmd_name));
            projectFile.Add("\t\t\t\"type\": \"shell\",");
            projectFile.Add("\t\t\t\"command\": \"mono\",");
            projectFile.Add("\t\t\t\"args\": [");
            projectFile.Add(String.Format("\t\t\t\t\"{0}\", \"{1}\",\"{2}\",\"{3}\"",Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName),name, configure, platform));
            projectFile.Add("\t\t\t],");
            projectFile.Add("\t\t}");
        }
        private void GenerateLaunchLinux(ref List<string> projectFile, string configure,string configure_exe)
        {
            projectFile.Add("\t\t{");
            projectFile.Add(String.Format("\t\t\t\"name\": \"{0}\",",configure));
            projectFile.Add("\t\t\t\"type\": \"cppdbg\",");
            projectFile.Add("\t\t\t\"request\": \"launch\",");
            if(String.IsNullOrEmpty(configure_exe))
            {
                projectFile.Add(String.Format("\t\t\t\"program\": \"{0}\",", Path.Combine(Config.Global.BinariesPlatformPath, Name )));

            }
            else
            {
                projectFile.Add(String.Format("\t\t\t\"program\": \"{0}\",", Path.Combine(Config.Global.BinariesPlatformPath, Name +"_" + configure_exe)));

            }
            projectFile.Add("\t\t\t\"args\": [],");
            projectFile.Add("\t\t\t\"stopAtEntry\": false,");
            projectFile.Add(String.Format("\t\t\t\"cwd\": \"{0}\",", Config.Global.BinariesPlatformPath));
            projectFile.Add("\t\t\t\"environment\":");
            projectFile.Add("\t\t\t[");
            projectFile.Add("\t\t{");
            projectFile.Add("\t\t\t\t\t\t\"Name\":  \"LD_LIBRARY_PATH\",");
            projectFile.Add(String.Format("\t\t\t\t\t\t\"Value\": \"{0}\",", Config.Global.BinariesPlatformPath));
            projectFile.Add("\t\t\t\t}");
            projectFile.Add("\t\t\t],");
            projectFile.Add("\t\t\t\"externalConsole\": true,");
            projectFile.Add("\t\t\t\"MIMode\": \"gdb\",");
            projectFile.Add("\t\t\t\"miDebuggerPath\": \"/usr/bin/gdb\",");
            projectFile.Add("\t\t\t\"setupCommands\": [");
            projectFile.Add("\t\t\t\t{");
            projectFile.Add("\t\t\t\t\t\"description\": \"Enable pretty-printing for gdb\",");
            projectFile.Add("\t\t\t\t\t\"text\": \"-enable-pretty-printing\",");
            projectFile.Add("\t\t\t\t\t\"ignoreFailures\": true");
            projectFile.Add("\t\t\t\t}");
            projectFile.Add("\t\t\t],");
            projectFile.Add(String.Format("\t\t\t\"preLaunchTask\": \"build {0}\"", configure.ToLower()));
            projectFile.Add("\t\t");
            projectFile.Add("\t\t}");
        }
        public void Write()
        {
            {
                List<string> projectFile = new List<string>();
                projectFile.Add("{");
                projectFile.Add("\t\"folders\": [");
                projectFile.Add("\t\t{");
                projectFile.Add(String.Format("\t\t\t\"path\": \"{0}\"", GenerateProject.MapProjects[Name].ProjectPath));
                projectFile.Add("\t\t}");
                projectFile.Add("\t],");
                projectFile.Add("\t\"settings\": {");
                projectFile.Add("\t\t\"files.associations\": {");
                projectFile.Add("\t\t\t\"iostream\": \"cpp\"");
                projectFile.Add("\t\t}");
                projectFile.Add("\t}");
                projectFile.Add("}");
                File.WriteAllLines(Path.Combine(ProjectDirectory, Name + ".code-workspace"), projectFile);
            }
            if(Config.Global.Platform==Config.Platform.Linux)
            {
                {
                    List<string> projectFile = new List<string>();
                    projectFile.Add("{");
                    projectFile.Add("\t\"configurations\": [");
                    GenerateCPPInfoLinux("Linux-Debug", ref projectFile, new string[] { "LINUX", "_LINUX64", "X64", "DEBUG", "_DEBUG" });
                    projectFile[projectFile.Count - 1] += ",";
                    GenerateCPPInfoLinux("Linux-Mixed", ref projectFile, new string[] { "LINUX", "_LINUX64", "X64", "DEBUG", "MIXED" });
                    projectFile[projectFile.Count - 1] += ",";
                    GenerateCPPInfoLinux("Linux-Release", ref projectFile, new string[] { "LINUX", "_LINUX64", "X64", "NDEBUG" });
                    projectFile.Add("\t],");
                    projectFile.Add("\t\"version\": 4");
                    projectFile.Add("}");
                    File.WriteAllLines(Path.Combine(VCDirectory, "c_cpp_properties.json"), projectFile);
                }
                if(General)
                {
                    {
                        List<string> projectFile = new List<string>();
                        projectFile.Add("{");
                        projectFile.Add("\t\"version\": \"2.0.0\",");
                        projectFile.Add("\t\"tasks\": [");
                        GenerateTaskLinux(ref projectFile, "build debug", Name, "Debug", "Linux");
                        projectFile[projectFile.Count - 1] += ",";
                        GenerateTaskLinux(ref projectFile, "build mixed", Name, "Mixed", "Linux");
                        projectFile[projectFile.Count - 1] += ",";
                        GenerateTaskLinux(ref projectFile, "build release", Name, "Release", "Linux");
                        projectFile.Add("\t]");
                        projectFile.Add("}");
                        File.WriteAllLines(Path.Combine(VCDirectory, "tasks.json"), projectFile);
                    }
                    {
                        List<string> projectFile = new List<string>();
                        projectFile.Add("{");
                        projectFile.Add("\t\"version\": \"0.2.0\",");
                        projectFile.Add("\t\"configurations\": [");
                        GenerateLaunchLinux(ref projectFile, "Debug","debug");
                        projectFile[projectFile.Count - 1] += ",";
                        GenerateLaunchLinux(ref projectFile, "Mixed","mixed");
                        projectFile[projectFile.Count - 1] += ",";
                        GenerateLaunchLinux(ref projectFile, "Release","");
                        projectFile.Add("\t]");
                        projectFile.Add("}");
                        File.WriteAllLines(Path.Combine(VCDirectory, "launch.json"), projectFile);
                    }

                }

            }

            else
            {
                
            }
        }
    }
}

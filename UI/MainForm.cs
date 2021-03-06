﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BearBuildTool.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            if (Config.Global.IsWindows)
            {
                comboBoxPlatform.Items.Clear();
                comboBoxPlatform.Items.Add(Config.Platform.Clang.ToString());
                comboBoxPlatform.Items.Add(Config.Platform.Win64.ToString());
                comboBoxPlatform.Items.Add(Config.Platform.Win32.ToString());
                comboBoxPlatform.Items.Add(Config.Platform.MinGW.ToString());
               
               
            }
            else
            {
                comboBoxPlatform.Items.Add(Config.Platform.Linux.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBoxPlatform.SelectedIndex = 0;
            comboBoxConfigure.SelectedIndex = 0;
            comboBoxTranslator.SelectedIndex = 0;
            InitializeList();
            checkBoxDevVersion.Checked = Config.Global.DevVersion;
        }

        private void InitializeList()
        {
            foreach (string name in Config.Global.ExecutableMap[Config.Global.Platform][Config.Global.Configure][Config.Global.DevVersion].Keys)
            {
                listBoxProject.Items.Add(name);
            }

        }

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            if (listBoxProject.SelectedIndex >= 0)
            {
                string name = listBoxProject.SelectedItem as string;
                if (!string.IsNullOrEmpty(name))
                {
                    Tools.FileSystem.Clear();
                    Config.Global.Project = name;
                    SetPlatform();
                    SetConfigure();
                    try
                    {
                        BearBuildTool.CompileProject();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("-------------------------Критическая ошибка при сборке-------------------------");
                        Console.Write(ex.ToString());
                        Console.WriteLine("-------------------------------------------------------------------------------");
                    }
                }
            }
        }

        private void SetConfigure()
        {
            if (comboBoxConfigure.SelectedIndex < 0) throw new Exception();
            string configure = comboBoxConfigure.SelectedItem as string;
            if (String.IsNullOrEmpty(configure)) throw new Exception();
            Config.Global.SetConfigure(configure);
        }


        private void SetPlatform()
        {
            if (comboBoxPlatform.SelectedIndex < 0) throw new Exception();
            string platform = comboBoxPlatform.SelectedItem as string;
            if (String.IsNullOrEmpty(platform)) throw new Exception();
            Config.Global.SetPlatform(platform);
        }

        private void buttonClean_Click(object sender, EventArgs e)
        {

            if (listBoxProject.SelectedIndex >= 0)
            {
                string name = listBoxProject.SelectedItem as string;
                if (!string.IsNullOrEmpty(name))
                {
                    Tools.FileSystem.Clear();
                    Config.Global.Project = name;
                    SetPlatform();
                    SetConfigure();
                    BearBuildTool.ClaenProject();
                }
            }
        }

        private void buttonRebuild_Click(object sender, EventArgs e)
        {
            if (listBoxProject.SelectedIndex >= 0)
            {
                string name = listBoxProject.SelectedItem as string;
                if (!string.IsNullOrEmpty(name))
                {
                    Tools.FileSystem.Clear();
                    Config.Global.Project = name;
                    SetPlatform();
                    SetConfigure();
                    try
                    {
                        BearBuildTool.ClaenProject();
                        BearBuildTool.CompileProject();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("-------------------------Критическая ошибка при сборке-------------------------");
                        Console.Write(ex.ToString());
                        Console.WriteLine("-------------------------------------------------------------------------------");
                    }
                }
            }
        }

        private void buttonGenerateProject_Click(object sender, EventArgs e)
        {
            if (listBoxProject.SelectedIndex >= 0)
            {
                string name = listBoxProject.SelectedItem as string;
                if (!string.IsNullOrEmpty(name))
                {
                    switch (comboBoxTranslator.SelectedIndex)
                    {
                        case 0:
                            BearBuildTool.GenerateProjectFileVS(name);
                            break;
                        case 1:
                            BearBuildTool.GenerateProjectFileVC(name);
                            break;
                    }
                }
            }
        }

        private void VSProjectManagerStripMenuItem_Click(object sender, EventArgs e)
        {
            VisualStudio.VSProjectManager vSProjectManager = new VisualStudio.VSProjectManager();
            vSProjectManager.ShowDialog();
        }

        private void mSVCToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxCompiller_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void compilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisualStudio.CompilersSetting mVSC = new VisualStudio.CompilersSetting();
            mVSC.ShowDialog();
        }

        private void путьКДиректорииПроектовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Config.Global.BasePath = fbd.SelectedPath;
                    Config.Global.SaveConfig();
                    Application.Exit();
                }

            }
        }

        private void checkBoxDevVersion_CheckedChanged(object sender, EventArgs e)
        {
            Config.Global.DevVersion = checkBoxDevVersion.Checked;
            Config.Global.SaveConfig();
        }
    }
}

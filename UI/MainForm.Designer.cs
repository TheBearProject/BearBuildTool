﻿namespace BearBuildTool.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.GroupBox groupBox3;
            this.listBoxProject = new System.Windows.Forms.ListBox();
            this.comboBoxConfigure = new System.Windows.Forms.ComboBox();
            this.comboBoxPlatform = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTranslator = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonGenerateProject = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VSProjectManagerStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.путьКДиректорииПроектовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonBuild = new System.Windows.Forms.Button();
            this.buttonRebuild = new System.Windows.Forms.Button();
            this.buttonClean = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.checkBoxDevVersion = new System.Windows.Forms.CheckBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.listBoxProject);
            groupBox1.Location = new System.Drawing.Point(3, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(201, 428);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Проекты";
            // 
            // listBoxProject
            // 
            this.listBoxProject.FormattingEnabled = true;
            this.listBoxProject.Location = new System.Drawing.Point(6, 19);
            this.listBoxProject.Name = "listBoxProject";
            this.listBoxProject.Size = new System.Drawing.Size(189, 394);
            this.listBoxProject.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.checkBoxDevVersion);
            groupBox2.Controls.Add(this.comboBoxConfigure);
            groupBox2.Controls.Add(this.comboBoxPlatform);
            groupBox2.Controls.Add(this.label3);
            groupBox2.Controls.Add(this.label2);
            groupBox2.Location = new System.Drawing.Point(211, 27);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(205, 97);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Кофигурации";
            // 
            // comboBoxConfigure
            // 
            this.comboBoxConfigure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConfigure.FormattingEnabled = true;
            this.comboBoxConfigure.Items.AddRange(new object[] {
            "Debug",
            "Mixed",
            "Release"});
            this.comboBoxConfigure.Location = new System.Drawing.Point(86, 46);
            this.comboBoxConfigure.Name = "comboBoxConfigure";
            this.comboBoxConfigure.Size = new System.Drawing.Size(113, 21);
            this.comboBoxConfigure.TabIndex = 2;
            // 
            // comboBoxPlatform
            // 
            this.comboBoxPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlatform.FormattingEnabled = true;
            this.comboBoxPlatform.Items.AddRange(new object[] {
            "Win32",
            "Win64"});
            this.comboBoxPlatform.Location = new System.Drawing.Point(74, 19);
            this.comboBoxPlatform.Name = "comboBoxPlatform";
            this.comboBoxPlatform.Size = new System.Drawing.Size(125, 21);
            this.comboBoxPlatform.TabIndex = 1;
            this.comboBoxPlatform.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlatform_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Конфигурация:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Платформа:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(this.comboBoxTranslator);
            groupBox3.Controls.Add(this.label4);
            groupBox3.Controls.Add(this.buttonGenerateProject);
            groupBox3.Location = new System.Drawing.Point(212, 188);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(205, 73);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "Транслятор";
            // 
            // comboBoxTranslator
            // 
            this.comboBoxTranslator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTranslator.FormattingEnabled = true;
            this.comboBoxTranslator.Items.AddRange(new object[] {
            "Microsoft Visual Studio",
            "Microsoft Visual Code"});
            this.comboBoxTranslator.Location = new System.Drawing.Point(74, 17);
            this.comboBoxTranslator.Name = "comboBoxTranslator";
            this.comboBoxTranslator.Size = new System.Drawing.Size(125, 21);
            this.comboBoxTranslator.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Платформа:";
            // 
            // buttonGenerateProject
            // 
            this.buttonGenerateProject.Location = new System.Drawing.Point(7, 43);
            this.buttonGenerateProject.Name = "buttonGenerateProject";
            this.buttonGenerateProject.Size = new System.Drawing.Size(192, 23);
            this.buttonGenerateProject.TabIndex = 8;
            this.buttonGenerateProject.Text = "Сгенерировать проект транслятор";
            this.buttonGenerateProject.UseVisualStyleBackColor = true;
            this.buttonGenerateProject.Click += new System.EventHandler(this.buttonGenerateProject_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(426, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VSProjectManagerStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.toolsToolStripMenuItem.Text = "Инструменты";
            // 
            // VSProjectManagerStripMenuItem
            // 
            this.VSProjectManagerStripMenuItem.Name = "VSProjectManagerStripMenuItem";
            this.VSProjectManagerStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.VSProjectManagerStripMenuItem.Text = "Менаджер проектов VS";
            this.VSProjectManagerStripMenuItem.Click += new System.EventHandler(this.VSProjectManagerStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compilerToolStripMenuItem,
            this.путьКДиректорииПроектовToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.settingsToolStripMenuItem.Text = "Настройки";
            // 
            // compilerToolStripMenuItem
            // 
            this.compilerToolStripMenuItem.Name = "compilerToolStripMenuItem";
            this.compilerToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.compilerToolStripMenuItem.Text = "Компилятор";
            this.compilerToolStripMenuItem.Click += new System.EventHandler(this.compilerToolStripMenuItem_Click);
            // 
            // путьКДиректорииПроектовToolStripMenuItem
            // 
            this.путьКДиректорииПроектовToolStripMenuItem.Name = "путьКДиректорииПроектовToolStripMenuItem";
            this.путьКДиректорииПроектовToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.путьКДиректорииПроектовToolStripMenuItem.Text = "Путь к директории проектов";
            this.путьКДиректорииПроектовToolStripMenuItem.Click += new System.EventHandler(this.путьКДиректорииПроектовToolStripMenuItem_Click);
            // 
            // buttonBuild
            // 
            this.buttonBuild.Location = new System.Drawing.Point(211, 130);
            this.buttonBuild.Name = "buttonBuild";
            this.buttonBuild.Size = new System.Drawing.Size(125, 23);
            this.buttonBuild.TabIndex = 6;
            this.buttonBuild.Text = "Собрать";
            this.buttonBuild.UseVisualStyleBackColor = true;
            this.buttonBuild.Click += new System.EventHandler(this.buttonBuild_Click);
            // 
            // buttonRebuild
            // 
            this.buttonRebuild.Location = new System.Drawing.Point(212, 159);
            this.buttonRebuild.Name = "buttonRebuild";
            this.buttonRebuild.Size = new System.Drawing.Size(205, 23);
            this.buttonRebuild.TabIndex = 4;
            this.buttonRebuild.Text = "Пересобрать";
            this.buttonRebuild.UseVisualStyleBackColor = true;
            this.buttonRebuild.Click += new System.EventHandler(this.buttonRebuild_Click);
            // 
            // buttonClean
            // 
            this.buttonClean.Location = new System.Drawing.Point(341, 130);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(76, 23);
            this.buttonClean.TabIndex = 7;
            this.buttonClean.Text = "Очистить";
            this.buttonClean.UseVisualStyleBackColor = true;
            this.buttonClean.Click += new System.EventHandler(this.buttonClean_Click);
            // 
            // checkBoxDevVersion
            // 
            this.checkBoxDevVersion.AutoSize = true;
            this.checkBoxDevVersion.Location = new System.Drawing.Point(7, 74);
            this.checkBoxDevVersion.Name = "checkBoxDevVersion";
            this.checkBoxDevVersion.Size = new System.Drawing.Size(137, 17);
            this.checkBoxDevVersion.TabIndex = 6;
            this.checkBoxDevVersion.Text = "Версия разрабодчика";
            this.checkBoxDevVersion.UseVisualStyleBackColor = true;
            this.checkBoxDevVersion.CheckedChanged += new System.EventHandler(this.checkBoxDevVersion_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(426, 456);
            this.Controls.Add(groupBox3);
            this.Controls.Add(this.buttonClean);
            this.Controls.Add(this.buttonBuild);
            this.Controls.Add(this.buttonRebuild);
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "BearBuildTool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxProject;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ComboBox comboBoxPlatform;
        private System.Windows.Forms.ComboBox comboBoxConfigure;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonBuild;
        private System.Windows.Forms.Button buttonRebuild;
        private System.Windows.Forms.Button buttonClean;
        private System.Windows.Forms.Button buttonGenerateProject;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ComboBox comboBoxTranslator;
        private System.Windows.Forms.ToolStripMenuItem VSProjectManagerStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compilerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem путьКДиректорииПроектовToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxDevVersion;
    }
}
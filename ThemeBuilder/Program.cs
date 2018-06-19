﻿#region Namespace

using System;
using System.Windows.Forms;

using ThemeBuilder.Forms;

#endregion

namespace ThemeBuilder
{
    internal static class Program
    {
        #region Methods

        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        #endregion
    }
}
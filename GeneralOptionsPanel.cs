// 
// GeneralOptionsPanel.cs 
//   
// Author:
//       Levi Bard <levi@unity3d.com>
// 
// Copyright (c) 2010 Unity Technologies
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// 

using System;
using System.IO;

using MonoDevelop.Ide;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;

namespace MonoDevelop.Debugger.Soft.Unity
{
	/// <summary>
	/// Global options panel for Unity debugger
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class GeneralOptionsPanel : Gtk.Bin
	{
		private static readonly string internalPath = "/Contents/MacOS/Unity";
		
		public GeneralOptionsPanel ()
		{
			this.Build ();
			
			if (PropertyService.IsMac) {
				unityChooser.Action = Gtk.FileChooserAction.SelectFolder;
				unityChooser.Title = "Browse to the Unity app";
			}
			
			// Load defaults
			if (File.Exists (Util.UnityLocation)) {
				unityChooser.SetFilename (Util.UnityLocation.Replace(internalPath, string.Empty));
			} else {
				unityChooser.SetFilename (Environment.GetFolderPath (Environment.SpecialFolder.Personal));
			}
			launchCB.Active = Util.UnityLaunch;
		}

		/// <summary>
		/// Store selected properties
		/// </summary>
		public bool Store ()
		{
			Util.UnityLocation = unityChooser.Filename;
			if (PropertyService.IsMac) { Util.UnityLocation += internalPath; }
			Util.UnityLaunch = launchCB.Active;
			PropertyService.SaveProperties ();
			return true;
		}
		
		protected virtual void OnLaunchCBToggled (object sender, System.EventArgs e)
		{
			unityChooser.Sensitive = launchCB.Active;
		}
	}
	
	/// <summary>
	/// OptionsPanel wrapper for GeneralOptionsPanel
	/// </summary>
	public class GeneralOptionsPanelBinding : OptionsPanel
	{
		private GeneralOptionsPanel panel;
		
		public override Gtk.Widget CreatePanelWidget ()
		{
			panel = new GeneralOptionsPanel ();
			return panel;
		}
		
		public override void ApplyChanges ()
		{
			panel.Store ();
		}
	}
}


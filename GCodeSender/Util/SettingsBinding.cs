﻿using System.Windows.Data;

namespace GCodeSender.Util
{
	public class SettingBindingExtension : Binding
	{
		public SettingBindingExtension()
		{
			Initialize();
		}

		public SettingBindingExtension(string path)
			: base(path)
		{
			Initialize();
		}

		private void Initialize()
		{
			Source = Properties.Settings.Default;
			Mode = BindingMode.TwoWay;
		}
	}
}
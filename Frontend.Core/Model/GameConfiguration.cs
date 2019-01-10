using System.Collections.Generic;
using Caliburn.Micro;
using Frontend.Core.Model.Interfaces;

namespace Frontend.Core.Model
{
	public class GameConfiguration : PropertyChangedBase, IGameConfiguration
	{
		/// <summary>
		///     Backing store for the <see cref="AbsoluteSaveGamePath" /> property
		/// </summary>
		private string absoluteSaveGamePath;

		/// <summary>
		///     Backing field for the <see cref="SupportedMods" /> collection.
		/// </summary>
		private IList<IMod> supportedMods;

		/// <summary>
		///     Gets or sets the name.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		///     Gets or sets the name of the friendly.
		/// </summary>
		/// <value>
		///     The name of the friendly.
		/// </value>
		public string FriendlyName { get; set; }

		/// <summary>
		///     Gets or sets the save game extension.
		/// </summary>
		/// <value>
		///     The save game extension.
		/// </value>
		public string SaveGameExtension { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether [is installed].
		/// </summary>
		/// <value>
		///     <c>true</c> if [is installed]; otherwise, <c>false</c>.
		/// </value>
		public bool IsInstalled { get; set; }

		/// <summary>
		///     Gets or sets the version.
		/// </summary>
		/// <value>
		///     The version.
		/// </value>
		public string Version { get; set; }

		/// <summary>
		///     Gets or sets the name of the configuration file mod directory tag.
		/// </summary>
		/// <value>
		///     The name of the configuration file mod directory tag.
		/// </value>
		public string ModDirectoryTagName { get; set; }

		/// <summary>
		///     Gets or sets the absolute save game path
		/// </summary>
		public string AbsoluteSaveGamePath
		{
			get { return absoluteSaveGamePath; }

			set
			{
				if (absoluteSaveGamePath == value)
				{
					return;
				}

				absoluteSaveGamePath = value;
				NotifyOfPropertyChange(() => AbsoluteSaveGamePath);
			}
		}

		/// <summary>
		///     The current mod tag name (IE: What's the tag used to identify this in configuration.txt)
		/// </summary>
		public string CurrentModTagName { get; set; }

		public IList<IMod> SupportedMods
		{
			get { return supportedMods ?? (supportedMods = new List<IMod>()); }
		}

		/// <summary>
		///     A value indicating whether this game is configured to use mods
		/// </summary>
		public bool IsConfiguredToUseMods
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		///     The current mod
		/// </summary>
		public IMod CurrentMod { get; set; }
	}
}
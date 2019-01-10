﻿using System.Collections.Generic;

namespace Frontend.Core.Model.Interfaces
{
	public interface IGameConfiguration
	{
		/// <summary>
		///     Gets or sets the save game path.
		/// </summary>
		/// <value>
		///     The save game path.
		/// </value>
		string AbsoluteSaveGamePath { get; set; }

		/// <summary>
		///     Gets or sets the name.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		string Name { get; set; }

		/// <summary>
		///     Gets or sets the name of the friendly.
		/// </summary>
		/// <value>
		///     The name of the friendly.
		/// </value>
		string FriendlyName { get; set; }

		/// <summary>
		///     Gets or sets the save game extension.
		/// </summary>
		/// <value>
		///     The save game extension.
		/// </value>
		string SaveGameExtension { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether [is installed].
		/// </summary>
		/// <value>
		///     <c>true</c> if [is installed]; otherwise, <c>false</c>.
		/// </value>
		bool IsInstalled { get; set; }

		/// <summary>
		///     Gets or sets the version.
		/// </summary>
		/// <value>
		///     The version.
		/// </value>
		string Version { get; set; }

		/// <summary>
		///     Gets or sets the name of the configuration file mod directory tag.
		/// </summary>
		/// <value>
		///     The name of the configuration file mod directory tag.
		/// </value>
		string ModDirectoryTagName { get; set; }

		/// <summary>
		///     Gets or sets the name of the current mod tag.
		/// </summary>
		/// <value>
		///     The name of the current mod tag.
		/// </value>
		string CurrentModTagName { get; set; }

		/// <summary>
		///     Gets a list of supported mods, as read from Configuration.xml
		/// </summary>
		/// <value>
		///     The supported mods.
		/// </value>
		IList<IMod> SupportedMods { get; }

		/// <summary>
		///     Gets a value indicating whether this game is configured use mods. This depends on the information available in
		///     Configuration.xml
		/// </summary>
		bool IsConfiguredToUseMods { get; }

		/// <summary>
		///     Gets or sets the selected mod.
		/// </summary>
		/// <value>
		///     The selected mod.
		/// </value>
		IMod CurrentMod { get; set; }
	}
}
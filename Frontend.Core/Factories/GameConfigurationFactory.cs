using System.Linq;
using System.Xml.Linq;
using Caliburn.Micro;
using Frontend.Core.Helpers;
using Frontend.Core.Model;

namespace Frontend.Core.Factories
{
	public class GameConfigurationFactory : FactoryBase
	{
		public GameConfigurationFactory(IEventAggregator eventAggregator)
			: base(eventAggregator, "gameConfiguration")
		{
		}

		protected override T OnBuildElement<T>(XElement element)
		{
			var name = XElementHelper.ReadStringValue(element, "name");
			var friendlyName = XElementHelper.ReadStringValue(element, "friendlyName");
			var currentModTagName = XElementHelper.ReadStringValue(element, "currentModTagName", false);
			var supportedModsAsString = element.Descendants("supportedMod");

			var gameConfig = new GameConfiguration
			{
				Name = name,
				FriendlyName = friendlyName,
				CurrentModTagName = currentModTagName
			};

			// Dummy item so that the user can undo selecting a mod
			var dummyMod = new Mod { Name = "No mod", IsDummyItem = true };
			gameConfig.SupportedMods.Add(dummyMod);
			gameConfig.CurrentMod = dummyMod;

			// Add proper mods
			if (supportedModsAsString.Count() > 0)
			{
				supportedModsAsString.ForEach(
					 m => gameConfig.SupportedMods.Add(new Mod { Name = XElementHelper.ReadStringValue(m, "modName") }));
			}

			return gameConfig as T;
		}
	}
}
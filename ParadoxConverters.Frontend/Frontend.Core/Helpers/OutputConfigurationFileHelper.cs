﻿using Frontend.Core.Model.Interfaces;
using Frontend.Core.Model.Paths.Interfaces;
using Frontend.Core.Proxies;
using System.IO;
using System.Linq;
using System.Text;

namespace Frontend.Core.Helpers
{
    public class OutputConfigurationFileHelper : IOutputConfigurationFileHelper
    {
        IFileProxy fileProxy;

        public OutputConfigurationFileHelper(IFileProxy fileProxy)
        {
            this.fileProxy = fileProxy;
        }

        /// <summary>
        /// Constructs the string that will be saved to disk as the config file.
        /// </summary>
        /// <returns></returns>
        public string BuiltOutputString(IConverterSettings converterSettings, IDirectoryHelper directoryHelper)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(ReadTextFile(directoryHelper.GetFrontendWorkingDirectory() + "\\Docs\\license.txt"));

            sb.AppendLine("configuration =");
            sb.AppendLine("{");

            // TODO: Mod support
            // Output source and target game settings

            // Why the "where clause": Some entries in the requireditems list is only used by the frontend. 
            // Those that needs to go into configuration.txt has the "TagName" attribute check, the others don't.
            foreach (IRequiredFolder folder in converterSettings.RequiredItems.OfType<IRequiredFolder>().Where(f => !string.IsNullOrEmpty(f.TagName)))
            {
                sb.AppendLine("\t# " + folder.FriendlyName + ": " + folder.Description);
                sb.AppendLine("\t" + folder.TagName + " = \"" + folder.SelectedValue + "\"");
                sb.AppendLine();
            }

            // Preferences
            foreach (IPreferenceCategory category in converterSettings.Categories)
            {
                //sb.AppendLine();
                //sb.AppendLine("\t# " + category.FriendlyName);
                //sb.AppendLine();

                category.Preferences.ForEach(p => sb.AppendLine("\t" + p.ToString()));
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        public string ReadTextFile(string path)
        {
            if (!this.fileProxy.Exists(path))
            {
                return string.Empty;
            }

            var text = this.fileProxy.ReadAllText(path);

            return text;
        }
    }
}

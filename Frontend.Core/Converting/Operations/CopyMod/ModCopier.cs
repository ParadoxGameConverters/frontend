using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Frontend.Core.Common.Proxies;
using Frontend.Core.Helpers;
using Frontend.Core.Logging;
using Frontend.Core.Model.Interfaces;

namespace Frontend.Core.Converting.Operations.CopyMod
{
    public class ModCopier : IModCopier
    {
        private readonly IDirectoryCopyHelper directoryCopyHelper;
        private readonly IDirectoryHelper directoryHelper;
        private readonly IFileProxy fileProxy;
        private readonly IFolderProxy folderProxy;
        private readonly IMessageBoxProxy messageBoxProxy;
        private readonly ISaveGameNameTranslator nameTranslator;
        private readonly IConverterOptions options;

        public ModCopier(
            IConverterOptions options,
            IFileProxy fileProxy,
            IFolderProxy folderProxy,
            IDirectoryHelper directoryHelper,
            IMessageBoxProxy messageBoxProxy,
            IDirectoryCopyHelper directoryCopyHelper,
            ISaveGameNameTranslator nameTranslator)
        {
            this.options = options;
            this.fileProxy = fileProxy;
            this.folderProxy = folderProxy;
            this.directoryHelper = directoryHelper;
            this.messageBoxProxy = messageBoxProxy;
            this.directoryCopyHelper = directoryCopyHelper;
            this.nameTranslator = nameTranslator;
        }

        public OperationResult MoveModFileAndFolder()
        {
            var operationResult = new OperationResult();

            // NOTE: This method may be V2 specific. I'll have to talk to Idhrendur about the rules for how this is/will be handled in other converters. 
            // I'll figure out some generic way of handling any problems related to that when and if it occurs. 

            var targetGameModPathItem =
                options.CurrentConverter.RequiredItems.First(i => i.InternalTagName.Equals("targetGameModPath"));
            var desiredFileName =
                folderProxy.GetFileNameWithoutExtension(options.CurrentConverter.AbsoluteSourceSaveGame.SelectedValue) +
                options.CurrentConverter.TargetGame.SaveGameExtension;

				var activeConfiguration = options.CurrentConverter.Categories.First(c => c.FriendlyName == "Configuration");
				if (activeConfiguration != null)
				{
					var outputName = activeConfiguration.Preferences.FirstOrDefault(d => d.Name == "output_name");
					if (outputName != null)
					{
						var grabbedName = outputName.ToString();
						int index1 = grabbedName.IndexOf("\"");
						int index2 = grabbedName.LastIndexOf("\"");
						if (index2 - index1 > 1)
						{
							grabbedName = grabbedName.Substring(index1+1, index2 - index1 - 1);
							desiredFileName = grabbedName;
						}

						operationResult.LogEntries.Add(new LogEntry("Output name: " + desiredFileName, LogEntrySeverity.Info, LogEntrySource.UI));
					}
				}


         // This is savegame normalization. It strips accents in a fashion compatible with the converter.

         var normalizedString = desiredFileName.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
	            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
	            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
	            {
		            stringBuilder.Append(c);
	            }
            }
            desiredFileName = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            // Here we transliterate the name in case Normalize failed to do it's job (cyrillic letters won't normalize at all in frontend, unlike converter)
            // This produces questionmarks for non-normalizable characters, which we replace with zeroes, same as the converter does.

	         byte[] unicodeBytes = Encoding.Unicode.GetBytes(desiredFileName);
	         byte[] asciiBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);
	         desiredFileName = Encoding.ASCII.GetString(asciiBytes);
	         desiredFileName = desiredFileName.Replace('?', '0');
	         operationResult.LogEntries.Add(new LogEntry("Mod to be copied: ", LogEntrySeverity.Info, LogEntrySource.UI, desiredFileName));

	         var translatedSaveGameName = nameTranslator.TranslateName(desiredFileName);

            // Copy the newly created output mod to the target game mod directory. 
            // The mod consists of two things: One file and one folder named after the save. Ex: The folder "France144_11_11" and the file "France144_11_11.mod".
            var converterWorkingDirectory = directoryHelper.GetConverterWorkingDirectory(options.CurrentConverter);
            var outputModFolderSourcePath = folderProxy.Combine(converterWorkingDirectory, "output",
                translatedSaveGameName);
            var outputModFileSourcePath = folderProxy.Combine(converterWorkingDirectory, "output",
                (translatedSaveGameName + ".mod"));

            var expectedAbsoluteOutputModFolderTargetPath = folderProxy.Combine(targetGameModPathItem.SelectedValue,
                translatedSaveGameName);
            var expectedAbsoluteOutputModFileTargetPath = expectedAbsoluteOutputModFolderTargetPath + ".mod";

            var modFileExists = fileProxy.Exists(expectedAbsoluteOutputModFileTargetPath);
            var modFolderExists = folderProxy.Exists(expectedAbsoluteOutputModFolderTargetPath);

            if (modFileExists || modFolderExists)
            {
                var confirmationMessage =
                    string.Format("One or more parts of the output mod exists in {0} already. Overwrite?",
                        targetGameModPathItem.SelectedValue);
                var result = messageBoxProxy.Show(confirmationMessage, "Confirmation Required", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {
                    operationResult.State = OperationResultState.Warning;
                    return operationResult;
                }
            }

            try
            {
                fileProxy.Copy(outputModFileSourcePath, expectedAbsoluteOutputModFileTargetPath, true);
                directoryCopyHelper.DirectoryCopy(outputModFolderSourcePath, expectedAbsoluteOutputModFolderTargetPath,
                    true, true);

                operationResult.LogEntries.Add(new LogEntry("Mod copied to: ", LogEntrySeverity.Info, LogEntrySource.UI,
                    expectedAbsoluteOutputModFolderTargetPath));
            }
            catch (Exception e)
            {
                operationResult.LogEntries.Add(new LogEntry(e.Message, LogEntrySeverity.Error, LogEntrySource.UI));
                operationResult.State = OperationResultState.Error;
            }

            return operationResult;
        }
    }
}
using System.IO;

namespace Frontend.Core.Converting.Operations.ExtractSave
{
    public class CompressedSaveChecker : ICompressedSaveChecker
    {
        public bool IsCompressedSave(string path)
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var firstLine = reader.ReadLine();

                    if (firstLine.StartsWith("EU4binM"))
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
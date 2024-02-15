using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core.Localization
{
    public interface ICsvParser
    {
        internal Dictionary<string, List<string>> LoadFromPath(string path, Delimiter delimiter = Delimiter.Auto, Encoding encoding = null);
        internal Task<Dictionary<string, List<string>>> LoadFromPathAsync(string path, Delimiter delimiter = Delimiter.Auto, Encoding encoding = null);
        internal Dictionary<string, List<string>> Parse(string data, Delimiter delimiter);
    }
}
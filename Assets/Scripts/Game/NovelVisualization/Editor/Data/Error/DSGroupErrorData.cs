using System.Collections.Generic;

namespace Game.NovelVisualization.Editor
{
    public class DSGroupErrorData
    {
        public DSErrorData ErrorData { get; set; }
        public List<CustomGroup> Groups { get; set; }

        public DSGroupErrorData()
        {
            ErrorData = new DSErrorData();
            Groups = new List<CustomGroup>();
        }
    }
}
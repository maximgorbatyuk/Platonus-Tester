using System.Collections.Generic;

namespace Platonus_Tester.Model
{
    public class SourceFile
    {
        public string SourceText        { get; set;  }
        public List<Novacode.Image> Images { get; set;  }

        public SourceFile(string text, List<Novacode.Image> images)
        {
            SourceText = text;
            Images = images;
        }
    }
}
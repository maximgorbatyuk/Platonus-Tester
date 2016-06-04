using System.Collections.Generic;
using Image = System.Drawing.Image;

namespace Platonus_Tester.Model
{
    public class Question
    {
        public string AskQuestion { get; set; }
        public Image Picture { get; set; }
        public List<string> AnswerList { get; set; }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;


namespace APA2UI
{
    public class WordGen
    {
        public WordGen()
        {
        }
        public void iniitialize()
        {
            oword = new Word.Application();
            odoc = oword.Documents.Add(ref omissing, ref omissing, ref omissing, ref omissing);

        }

        public Word._Application oword;
        public Word._Document odoc;
        public object omissing = System.Reflection.Missing.Value;
        public object oendofdoc = "\\endofdoc";
        public object oStartofDoc = "\\StartOfDoc"; // start of it

    }
}

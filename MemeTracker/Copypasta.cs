using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MemeTracker
{
    public class Copypasta
    {
        private static Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        private static char[] Vowels = new char[] { 'a', 'e', 'i', 'o', 'u', 'y' };

        public string Base { get; private set; }

        public string FirstPart { get; private set; }
        public string SecondPart { get; private set; }

        public string MainEmoji { get; private set; }
        public string SecondEmoji { get; private set; }

        public string Enlongated { get; private set; }

        public string BaseRandomlyCapitalized
        {
            get
            {
                return rgx.Replace(Base, "").RandomlyCapitalize();
            }
        }

        public string TotalCopypasta
        {
            get
            {
                return MainEmoji + SecondEmoji + MainEmoji + SecondEmoji + MainEmoji + SecondEmoji + MainEmoji + SecondEmoji + MainEmoji + SecondEmoji +
                       FirstPart + " " + SecondPart + " " + FirstPart + " " + SecondPart + " " + MainEmoji + " thats " + SecondEmoji + " some " + FirstPart +
                       " " + MainEmoji + MainEmoji + " " + SecondPart + " right " + SecondEmoji + SecondEmoji + " there " + MainEmoji + MainEmoji + MainEmoji +
                       " right " + SecondEmoji + "there" + SecondEmoji + SecondEmoji + " if i do say so my self 😅i say so 😅thats what im talking about right" +
                       " there right there (chorus: ʳᶦᵍʰᵗ ᵗʰᵉʳᵉ) mMMMMᎷМ😂" + MainEmoji + MainEmoji + MainEmoji + Enlongated + MainEmoji + MainEmoji + MainEmoji +
                       SecondEmoji + MainEmoji + SecondEmoji + SecondEmoji + SecondEmoji + MainEmoji + MainEmoji + FirstPart + " " + SecondPart;
            }
        }

        public Copypasta(string src, string mainEmoji, string secondEmoji)
        {
            Base = src;
            string[] cleanbase = Base.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            FirstPart = rgx.Replace(cleanbase[0], "");
            SecondPart = rgx.Replace(cleanbase[0], "");
            if (cleanbase.Length > 1)
            {
                SecondPart = rgx.Replace(cleanbase[1], "");
            }

            MainEmoji = mainEmoji;
            SecondEmoji = secondEmoji;

            char firstVowel = FirstPart.First(d => Vowels.Contains(Char.ToLower(d)));

            Enlongated = FirstPart.Substring(0, FirstPart.IndexOf(firstVowel)).ToUpper();
            Enlongated += new String(firstVowel, Util.rand.Next(8, 20));
            Enlongated += FirstPart.Substring(FirstPart.IndexOf(firstVowel));

            Enlongated = Enlongated.RandomlyCapitalize();
        }
    }
}

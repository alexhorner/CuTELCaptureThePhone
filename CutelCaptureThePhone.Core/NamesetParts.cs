using System.Collections.ObjectModel;
using CutelCaptureThePhone.Core.Extensions;

namespace CutelCaptureThePhone.Core
{
    public class NamesetParts(List<string> aParts, List<string> bParts, List<string> cParts)
    {
        public ReadOnlyCollection<string> AParts { get; } = new(aParts.Select(a => a.ToLower()).ToList());
        public ReadOnlyCollection<string> BParts { get; } = new(bParts.Select(b => b.ToLower()).ToList());
        public ReadOnlyCollection<string> CParts { get; } = new(cParts.Select(c => c.ToLower()).ToList());

        public string GetNameFromNameset((int PartA, int PartB, int PartC) nameset)
        {
            string aPart;
            string bPart;
            string cPart;

            try
            {
                aPart = AParts[nameset.PartA];
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("PartA of nameset is not known", nameof(nameset));
            }
            
            try
            {
                bPart = BParts[nameset.PartB];
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("PartB of nameset is not known", nameof(nameset));
            }
            
            try
            {
                cPart = CParts[nameset.PartC];
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("PartC of nameset is not known", nameof(nameset));
            }

            return $"{aPart.ToUpperFirstLetter()} {bPart.ToUpperFirstLetter()} {cPart.ToUpperFirstLetter()}";
        }

        public (int PartA, int PartB, int PartC) GetNamesetFromName(string name)
        {
            name = name.ToLower();

            string[] nameParts = name.Split(' ');

            if (nameParts.Length != 3) throw new ArgumentException("Name must contain exactly 3 parts", nameof(name));

            int aPartIndex = AParts.IndexOf(nameParts[0]);
            if (aPartIndex < 0) throw new ArgumentException("First part of name is not known", nameof(name));
            
            int bPartIndex = BParts.IndexOf(nameParts[1]);
            if (bPartIndex < 0) throw new ArgumentException("Second part of name is not known", nameof(name));
            
            int cPartIndex = CParts.IndexOf(nameParts[2]);
            if (cPartIndex < 0) throw new ArgumentException("Third part of name is not known", nameof(name));

            return new(aPartIndex, bPartIndex, cPartIndex);
        }
    }
}
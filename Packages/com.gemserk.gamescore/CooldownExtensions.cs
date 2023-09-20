using System;
using System.Linq;
using Gemserk.Utilities;

namespace Game
{
    public static class CooldownExtensions
    {
        public static void FillRandom(this Cooldown cooldown)
        {
            cooldown.Reset();
            cooldown.Increase(UnityEngine.Random.Range(0, cooldown.Total));
        }
    }

    public static class TextUtilities
    {
        public static string RemoveCommentLines(string text)
        {
            var lines = text.Split(new string[]
            {
                "\r\n", "\r", "\n"
            }, StringSplitOptions.None);

            var removedCommentLines = lines.Where(l => !l.TrimStart().StartsWith("//")).ToList();
            return string.Join("\n", removedCommentLines);
        }
    } 
}
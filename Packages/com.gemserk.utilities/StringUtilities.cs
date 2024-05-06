namespace Gemserk.Utilities
{
    public static class StringUtilities
    {
        public static string[] SplitSearchText(string searchText, char separator = ' ')
        {
            string[] searchElements = null;
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(searchText))
                {
                    searchElements = searchText.Split(separator);
                }
            }
            return searchElements;
        }

        public static bool MatchAll(string text, string[] searchTexts)
        {
            if (searchTexts == null || searchTexts.Length == 0)
                return false;
            
            foreach (var searchText in searchTexts)
            {
                if (!text.ToLower().Contains(searchText.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool MatchAny(string text, string[] searchTexts)
        {
            if (searchTexts == null || searchTexts.Length == 0)
                return true;
            
            foreach (var searchText in searchTexts)
            {
                if (text.ToLower().Contains(searchText.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
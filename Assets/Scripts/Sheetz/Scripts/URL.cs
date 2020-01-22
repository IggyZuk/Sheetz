namespace Sheetz
{
    public static class URL
    {
        const string googleDocsURL = "https://docs.google.com/spreadsheets/d";

        public static string WithSheet(Sheet sheet)
        {
            return $"{googleDocsURL}/{sheet.DocID}/export?gid={sheet.ID}&format={sheet.Format.ToString().ToLower()}";
        }
    }
}
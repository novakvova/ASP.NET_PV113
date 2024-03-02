namespace Blog.Web.Helpers
{
    public static class UrlSlugMaker
    {
        public static string GenerateSlug(string text)
        {
            string urlSlug = text.ToLowerInvariant();

            char[] invalidChars = { '.', ',', ':', ';' };
            foreach (char invalidChar in invalidChars)
            {
                urlSlug = urlSlug.Replace(invalidChar.ToString(), string.Empty);
            }

            urlSlug = urlSlug.Replace(" ", "_");

            return urlSlug;
        }
    }
}

namespace MusicSmash.Controllers.Api
{
    public static class MagicStringResolver
    {

        public static string RedirectUri(string baseUri) => $"{baseUri}/callback";

    }
}

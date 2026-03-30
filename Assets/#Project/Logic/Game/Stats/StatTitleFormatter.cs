using System.Text;

namespace Crossfire.Workspace
{
    internal static class StatTitleFormatter
    {
        public static string Format(string titleKey)
        {
            if (string.IsNullOrWhiteSpace(titleKey))
                return string.Empty;

            var title = titleKey.EndsWith("Title")
                ? titleKey[..^"Title".Length]
                : titleKey;

            var builder = new StringBuilder(title.Length + 8);
            for (var i = 0; i < title.Length; i++)
            {
                var character = title[i];
                if (i > 0 && char.IsUpper(character) && !char.IsWhiteSpace(title[i - 1]))
                    builder.Append(' ');

                builder.Append(character);
            }

            return builder.ToString();
        }
    }
}

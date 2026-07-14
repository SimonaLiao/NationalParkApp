using NationalPark.Models;

namespace NationalPark.Controls
{
    internal static class FavoriteStateHelper
    {
        public const string FavoritedGlyph = "\uE00B";
        public const string NotFavoritedGlyph = "\uE006";

        public static string GetFavoriteIcon(bool isFavorite)
        {
            return isFavorite ? FavoritedGlyph : NotFavoritedGlyph;
        }

        public static string GetFavoriteActionText(bool isFavorite)
        {
            return isFavorite ? "Remove from wishlist" : "Add to wishlist";
        }

        public static void ToggleFavorite(NationalParkModel park)
        {
            park.IsFavorite = !park.IsFavorite;
        }
    }
}

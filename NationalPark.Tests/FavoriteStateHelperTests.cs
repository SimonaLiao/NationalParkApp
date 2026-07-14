using NationalPark.Controls;
using NationalPark.Models;

namespace NationalPark.Tests;

[TestClass]
public sealed class FavoriteStateHelperTests
{
    [TestMethod]
    public void ToggleFavorite_WhenParkIsNotFavorited_AddsItToWishlistAndUpdatesUiState()
    {
        var park = new NationalParkModel { IsFavorite = false };

        FavoriteStateHelper.ToggleFavorite(park);

        Assert.IsTrue(park.IsFavorite);
        Assert.AreEqual(FavoriteStateHelper.FavoritedGlyph, FavoriteStateHelper.GetFavoriteIcon(park.IsFavorite));
        Assert.AreEqual("Remove from wishlist", FavoriteStateHelper.GetFavoriteActionText(park.IsFavorite));
    }

    [TestMethod]
    public void ToggleFavorite_WhenParkIsFavorited_RemovesItFromWishlistAndUpdatesUiState()
    {
        var park = new NationalParkModel { IsFavorite = true };

        FavoriteStateHelper.ToggleFavorite(park);

        Assert.IsFalse(park.IsFavorite);
        Assert.AreEqual(FavoriteStateHelper.NotFavoritedGlyph, FavoriteStateHelper.GetFavoriteIcon(park.IsFavorite));
        Assert.AreEqual("Add to wishlist", FavoriteStateHelper.GetFavoriteActionText(park.IsFavorite));
    }
}

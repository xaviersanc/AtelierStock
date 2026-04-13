using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart;

namespace ShoppingCartTest;

[TestClass]
public class CartTest
{
    private static Cart CreateCart() => new();

    private static Article SingleArticle(Cart cart) => cart.Articles.Single();

    private static void AssertThrows<TException>(Action action) where TException : Exception
    {
        try
        {
            action();
            Assert.Fail($"Expected {typeof(TException).Name}.");
        }
        catch (TException)
        {
        }
        catch (Exception exception)
        {
            Assert.Fail($"Expected {typeof(TException).Name} but got {exception.GetType().Name}.");
        }
    }

    [TestMethod]
    public void Cart_Neuf_EstVideEtATotalNul()
    {
        var cart = CreateCart();

        Assert.IsTrue(cart.IsEmpty);
        Assert.AreEqual(0m, cart.TotalPrice);
        Assert.AreEqual(0, cart.Articles.Count());
    }

    [TestMethod]
    public void Add_NouvelArticle_AjouteUnArticleEtCalculeLeTotal()
    {
        var cart = CreateCart();

        var article = cart.Add("Pomme", 2, 3.50m);

        Assert.AreEqual("Pomme", article.ProductName);
        Assert.AreEqual(2, article.Quantity);
        Assert.AreEqual(3.50m, article.Price);
        Assert.AreEqual(7.00m, cart.TotalPrice);
        Assert.IsFalse(cart.IsEmpty);
        CollectionAssert.AreEquivalent(
            new[] { new Article("Pomme", 2, 3.50m) },
            cart.Articles.ToList());
    }

    [TestMethod]
    public void Add_MemeProduit_AgreggeLesQuantites()
    {
        var cart = CreateCart();
        cart.Add("Pomme", 2, 3.50m);

        var article = cart.Add("Pomme", 3, 3.50m);

        Assert.AreEqual(5, article.Quantity);
        Assert.AreEqual(17.50m, cart.TotalPrice);
        Assert.AreEqual(1, cart.Articles.Count());
        Assert.AreEqual(5, SingleArticle(cart).Quantity);
    }

    [TestMethod]
    public void TotalPrice_AvecPlusieursArticles_FaitLaSommeDesLignes()
    {
        var cart = CreateCart();
        cart.Add("Pomme", 2, 3.50m);
        cart.Add("Pain", 1, 2.00m);
        cart.Add("Lait", 4, 1.25m);

        Assert.AreEqual(14.00m, cart.TotalPrice);
    }

    [TestMethod]
    public void DecreaseArticleQuantity_QuandLaQuantiteEstSuperieureA1_RetireUneUnite()
    {
        var cart = CreateCart();
        cart.Add("Pomme", 2, 3.50m);

        cart.DecreaseArticleQuantity("Pomme");

        Assert.AreEqual(1, SingleArticle(cart).Quantity);
        Assert.AreEqual(3.50m, cart.TotalPrice);
    }

    [TestMethod]
    public void DecreaseArticleQuantity_QuandLaQuantiteEst1_SupprimeLArticleDuPanier()
    {
        var cart = CreateCart();
        cart.Add("Pomme", 1, 3.50m);

        cart.DecreaseArticleQuantity("Pomme");

        Assert.AreEqual(0, cart.Articles.Count());
        Assert.AreEqual(0m, cart.TotalPrice);
        Assert.IsTrue(cart.IsEmpty);
    }

    [TestMethod]
    public void Add_QuantiteInvalide_LeveUneException()
    {
        var cart = CreateCart();

        AssertThrows<ArgumentOutOfRangeException>(() => cart.Add("Pomme", 0, 3.50m));
        AssertThrows<ArgumentOutOfRangeException>(() => cart.Add("Pomme", -1, 3.50m));
    }

    [TestMethod]
    public void Add_PrixInvalide_LeveUneException()
    {
        var cart = CreateCart();

        AssertThrows<ArgumentOutOfRangeException>(() => cart.Add("Pomme", 2, 0m));
        AssertThrows<ArgumentOutOfRangeException>(() => cart.Add("Pomme", 2, -1m));
    }

    [TestMethod]
    public void DecreaseArticleQuantity_ArticleInconnu_LeveUneException()
    {
        var cart = CreateCart();

        AssertThrows<ArgumentException>(() => cart.DecreaseArticleQuantity("Inconnu"));
    }
}

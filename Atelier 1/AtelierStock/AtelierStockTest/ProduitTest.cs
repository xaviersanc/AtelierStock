namespace AtelierStock
{
    [TestClass]
    public class ProduitTest
    {
        private static Produit CreerProduit() => new("REF-001", "Stylo", 100.0m, 0.18m);

        [TestMethod]
        public void Initialiser_Produit_CopieLesDonneesEtDemarreEnRupture()
        {
            var produit = CreerProduit();

            Assert.AreEqual("REF-001", produit.Reference);
            Assert.AreEqual("Stylo", produit.Libelle);
            Assert.AreEqual(100.0m, produit.PrixAchat);
            Assert.AreEqual(118.0m, produit.PrixVente);
            Assert.AreEqual(0, produit.Stocks);
            Assert.IsTrue(produit.EstEnRupture);
        }

        [TestMethod]
        public void Initialiser_Produit_MargeZero_DonnePrixVenteIdentiqueAuPrixAchat()
        {
            var produit = new Produit("REF-002", "Cahier", 100.0m, 0.0m);

            Assert.AreEqual(100.0m, produit.PrixAchat);
            Assert.AreEqual(100.0m, produit.PrixVente);
        }

        [TestMethod]
        public void Rentrer_QuantitePositive_AugmenteLeStockEtSortDeRupture()
        {
            var produit = CreerProduit();

            produit.Rentrer(5);

            Assert.AreEqual(5, produit.Stocks);
            Assert.IsFalse(produit.EstEnRupture);
        }

        [TestMethod]
        public void Sortir_QuantiteInferieureOuEgaleAuStock_RetireExactementLaQuantiteDemandee()
        {
            var produit = CreerProduit();
            produit.Rentrer(10);

            var quantiteSortie = produit.Sortir(4);

            Assert.AreEqual(4, quantiteSortie);
            Assert.AreEqual(6, produit.Stocks);
            Assert.IsFalse(produit.EstEnRupture);
        }

        [TestMethod]
        public void Sortir_QuantiteEgaleAuStock_MetLeProduitEnRupture()
        {
            var produit = CreerProduit();
            produit.Rentrer(3);

            var quantiteSortie = produit.Sortir(3);

            Assert.AreEqual(3, quantiteSortie);
            Assert.AreEqual(0, produit.Stocks);
            Assert.IsTrue(produit.EstEnRupture);
        }

        [TestMethod]
        public void Sortir_QuantiteSuperieureAuStock_NeDoitPasGenererDeStockNegatifEtSignaleLaRupture()
        {
            var produit = CreerProduit();
            produit.Rentrer(2);

            var quantiteSortie = produit.Sortir(5);

            Assert.AreEqual(2, quantiteSortie);
            Assert.AreEqual(0, produit.Stocks);
            Assert.IsTrue(produit.EstEnRupture);
        }
    }
}
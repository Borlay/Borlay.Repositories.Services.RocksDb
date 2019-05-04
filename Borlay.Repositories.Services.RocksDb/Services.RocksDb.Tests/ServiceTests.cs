using Borlay.Arrays;
using Borlay.Repositories;
using Borlay.Serialization.Notations;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Borlay.Serialization.Converters;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Services.RocksDb.Tests
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public async Task SaveAndGet()
        {
            var serializer = new Serializer();
            serializer.LoadFromReference<ServiceTests>();

            var path = @"C:\rocks\dev\tsts\contacts";
            var userEntityRepository = serializer.CreateSecondaryService<Card>(path);

            var userId = ByteArray.New(32);
            var cardId = ByteArray.New(32);

            var watch = Stopwatch.StartNew();
            //for (int i = 0; i < 1000; i++)
            {

                await userEntityRepository.Save(userId, new Card()
                {
                    Id = cardId,
                    Holder = "PK",
                    Number = "123456",
                    CVS = "123",
                });

            }

            watch.Stop();

            var card = await userEntityRepository.Get(userId, cardId);

            Assert.AreEqual(cardId, card.Id);
            Assert.AreEqual("PK", card.Holder);
            Assert.AreEqual("123456", card.Number);
            Assert.AreEqual("123", card.CVS);
        }
    }

    [Data(19052)]
    public class Card : IEntity
    {
        [Include(0, true)]
        public ByteArray Id { get; set; }

        [Include(1, true)]
        public string Holder { get; set; }

        [Include(2, true)]
        public string Number { get; set; }

        [Include(3, true)]
        public int ExpYear { get; set; }

        [Include(4, true)]
        public int ExpMonth { get; set; }

        [Include(5, true)]
        public string CVS { get; set; }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TableIO.PerfTests
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestData()
        //{
        //    var list = new List<Model>();
        //    var rand = new Random();
        //    var chars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', '\n', '"', ',' };

        //    for (int i = 1; i <= 10000; i++)
        //    {
        //        var str = "" + chars[rand.Next(chars.Length)] + chars[rand.Next(chars.Length)]
        //            + chars[rand.Next(chars.Length)] + chars[rand.Next(chars.Length)]
        //            + chars[rand.Next(chars.Length)] + chars[rand.Next(chars.Length)];
        //        list.Add(new Model
        //        {
        //            Id = i,
        //            Name = $"NAME {i}:" + str,
        //            Price = i * 10,
        //            Remarks = $"REMARKS {i}:" + str
        //        });
        //    }

        //    using (var writer = new StreamWriter("data_rand_str_10000.csv"))
        //    {
        //        var w = new TableFactory().CreateCsvWriter<Model>(writer);
        //        w.Write(list);
        //    }
        //}
    }
}

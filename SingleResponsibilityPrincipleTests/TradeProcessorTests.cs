using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleResponsibilityPrinciple;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.Tests
{
    [TestClass()]
    public class TradeProcessorTests
    {
        private int CountDbRecords()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\jseibert\source\repos\cis-3285-asg-8-jackseibert\tradedatabase.mdf"";Integrated Security=True;Connect Timeout=30;"))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                string myScalarQuery = "SELECT COUNT(*) FROM trade";
                SqlCommand myCommand = new SqlCommand(myScalarQuery, connection);
                //myCommand.Connection.Open();
                int count = (int)myCommand.ExecuteScalar();
                connection.Close();
                return count;
            }
        }
    
    [TestMethod()]
    //As a user, I want to make sure my data that I enter is valid.
    public void TestNormalFile()
        {
            //C: \Users\jseibert\Source\Repos\cis - 3285 - asg - 8 - jackseibert4\SingleResponsibilityPrincipleTests\goodtrades.txt
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.goodtrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod()]
        //As a user, I want to make sure I don't accidentally enter invalid text.
        public void TestBadFile()
        {
            //C: \Users\jseibert\Source\Repos\cis - 3285 - asg - 8 - jackseibert4\SingleResponsibilityPrincipleTests\goodtrades.txt
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badtrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod()]
        //As a user, I want a cap on how many trades I can do at a time.
        public void TestMaximumFile()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.maximum.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod()]
        //As a user, I want to make sure I don't accidentally enter a negative amount.
        public void TestMinimumFile()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.negative.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

    }
}
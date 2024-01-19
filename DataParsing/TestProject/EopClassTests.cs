using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataParsing;
using System.Collections.Generic;
using System.Data;
using System;

namespace TestProject
{
    [TestClass]
    public class EopClassTests
    {
        [TestMethod]
        public void IsExistTest()
        {
            //Arrange
            Eop eop = new Eop();

            //Act
            bool result = eop.IsExist();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SaveToJsonTest()
        {
            //Arrange
            Eop eop = new Eop();

            //Act
            eop.SaveToJson();
            bool result = eop.IsExist();

            //Assert
            Assert.IsTrue(result);
        }
    }

}

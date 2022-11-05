using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.XUnitExample.CollectionTests
{
    #region Run sequencially
    public class TestClass1
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }

        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    }
    #endregion

    #region Run parallel
    public class TestClass2
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }
    }

    public class TestClass3
    {
        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    }
    #endregion

    #region Run Sequencially even in different classes
    [Collection("These test collection takes part of #1")]
    public class TestClass4
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }
    }

    [Collection("These test collection takes part of #1")]
    public class TestClass5
    {
        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    } 
    #endregion


}

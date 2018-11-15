using System;
using System.Data;
using Xunit;
using Moq;
using CoreWebServicePOC.repo;
using CoreWebServicePOC.core;
using FizzWare.NBuilder;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


namespace CoreWebServicePOC.repo.Tests
{
    public class ValuesRepoTests : SqlTestBase
    {
        private IValuesRepo _repo;
        public ValuesRepoTests()
        {
            _repo = new ValuesRepo(SqlQueryProviderMock.Object);
        }

        [Fact]
        public void ListAllAsync_queryReturnsEmptyList_returnsEmptyList()
        {
            //Arrange
            SetReaderValue(new List<Value>());

            //Act
            var result = _repo.GetAllAsync().Result;

            //Assert
            Assert.True(result.Count() == 0);
            VerifyExecuteSqlAsync("ValuesSqlConnection", "SELECT [id],[value] FROM [ValueDB].[dbo].[Value];");
        }

        [Fact]
        public void ListAllAsync_queryReturnsList_returnsList()
        {
            //Arrange
            SetReaderValue(new List<Value>
            {
                new Value
                {
                    id =1,
                    value = "value1"
                },
                new Value
                {
                    id =2,
                    value = "value2"
                }
            });
           
            //Act
            var result = _repo.GetAllAsync().Result;

            //Assert
            Assert.True(result.Count()==2);
            Assert.Contains(result, m => m.id == 1 &&
                                          m.value == "value1");
            Assert.Contains(result, m => m.id == 2 &&
                                          m.value == "value2");
            VerifyExecuteSqlAsync("ValuesSqlConnection", "SELECT [id],[value] FROM [ValueDB].[dbo].[Value];");
        }
    }
}

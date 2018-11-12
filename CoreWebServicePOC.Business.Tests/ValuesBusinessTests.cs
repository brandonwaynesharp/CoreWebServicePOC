using System;
using Xunit;
using Moq;
using CoreWebServicePOC.core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWebServicePOC.Business.Tests
{
    public class ValuesBusinessTests
    {
        Mock<IValuesRepo> mockValuesRepo;
        IValuesBusiness valuesBusiness;

        [Fact]
        public void GetReturnsValues()
        {
            //Arrange
            IList<Value> returnValue = new List<Value>
            {
                new Value { id = 1, value = "value1" },
                new Value { id = 2, value = "value2" },
                new Value { id = 3, value = "value3" },
            };

            mockValuesRepo = new Mock<IValuesRepo>(MockBehavior.Strict);
            mockValuesRepo.Setup(v => v.GetAllValues()).Returns(Task.FromResult(returnValue));

            valuesBusiness = new ValuesBusiness(mockValuesRepo.Object);

            //Act
            var result = valuesBusiness.Get();

            //Assert
            Assert.True(result.Result.Count == 3);
            Assert.True(result.Result[0].id == 1);
            Assert.True(result.Result[0].value == "value1");
            Assert.True(result.Result[1].id == 2);
            Assert.True(result.Result[1].value == "value2");
            Assert.True(result.Result[2].id == 3);
            Assert.True(result.Result[2].value == "value3");
        }
    }
}

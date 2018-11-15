using System.Collections.Generic;
using Moq;
using Xunit;

namespace CoreWebServicePOC.repo.Tests
{
    public abstract class SqlTestBase
    {
        protected Mock<ISqlQueryProvider> SqlQueryProviderMock;
        protected Mock<IQueryReader> QueryReaderMock;

        public SqlTestBase()
        {
            SqlQueryProviderMock = new Mock<ISqlQueryProvider>();
            QueryReaderMock = new Mock<IQueryReader>();

            SqlQueryProviderMock.Setup(mock => mock.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(QueryReaderMock.Object);
            SqlQueryProviderMock.Setup(mock => mock.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(QueryReaderMock.Object);
            SqlQueryProviderMock.Setup(mock => mock.ExecuteSqlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(QueryReaderMock.Object);
            SqlQueryProviderMock.Setup(mock => mock.ExecuteSqlAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(QueryReaderMock.Object);
        }

        protected void SetScalarValue<T>(T value)
        {
            SqlQueryProviderMock.Setup(mock => mock.ExecuteScalar<T>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(value);
            SqlQueryProviderMock.Setup(mock => mock.ExecuteScalarAsync<T>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(value);
            SqlQueryProviderMock.Setup(mock => mock.ExecuteScalarSqlAsync<T>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(value);
            SqlQueryProviderMock.Setup(mock => mock.ExecuteScalarSqlAsync<T>(It.IsAny<string>(), It.IsAny<string>(), null))
                .ReturnsAsync(value);
        }

        protected void SetReaderValue<T>(IEnumerable<T> values)
        {
            QueryReaderMock.Setup(mock => mock.Read<T>())
                .Returns(values);
        }

        protected void VerifyExecuteScalar<T>(string connectionName, string procedureName, object expectedParameters)
        {
            SqlQueryProviderMock.Verify(mock => mock.ExecuteScalar<T>(connectionName, procedureName,
                It.Is<object>(m => m.ArePropertiesEqual(expectedParameters, false))), Times.Once);
        }

        protected void VerifyExecuteScalarAsync<T>(string connectionName, string procedureName, object expectedParameters)
        {
            SqlQueryProviderMock.Verify(mock => mock.ExecuteScalarAsync<T>(connectionName, procedureName,
                It.Is<object>(m => m.ArePropertiesEqual(expectedParameters, false))), Times.Once);
        }

        protected void VerifyExecuteScalarSqlAsync<T>(string connectionName, string sql, object expectedParameters)
        {
            SqlQueryProviderMock.Verify(mock => mock.ExecuteScalarSqlAsync<T>(connectionName, sql,
                It.Is<object>(m => m.ArePropertiesEqual(expectedParameters, false))), Times.Once);
        }

        protected void VerifyExecuteNonQuery(string connectionName, string procedureName, object expectedParameters)
        {
            SqlQueryProviderMock.Verify(mock => mock.ExecuteNonQuery(connectionName, procedureName,
                It.Is<object>(m => m.ArePropertiesEqual(expectedParameters, false))), Times.Once);
        }

        protected void VerifyExecuteNonQueryAsync(string connectionName, string procedureName, object expectedParameters)
        {
            SqlQueryProviderMock.Verify(mock => mock.ExecuteNonQueryAsync(connectionName, procedureName,
                It.Is<object>(m => m.ArePropertiesEqual(expectedParameters, false))), Times.Once);
        }

        protected void VerifyExecuteNonQuerySqlAsync(string connectionName, string sql, object expectedParameters)
        {
            SqlQueryProviderMock.Verify(mock => mock.ExecuteNonQuerySqlAsync(connectionName, sql,
                It.Is<object>(m => m.ArePropertiesEqual(expectedParameters, false))), Times.Once);
        }

        protected void VerifyExecute(string connectionName, string procedureName, object expectedParameters)
        {
            SqlQueryProviderMock.Verify(mock => mock.Execute(connectionName, procedureName,
                It.Is<object>(m => m.ArePropertiesEqual(expectedParameters, false))), Times.Once);
        }

        protected void VerifyExecuteAsync(string connectionName, string procedureName, object expectedParameters)
        {
            SqlQueryProviderMock.Verify(mock => mock.ExecuteAsync(connectionName, procedureName,
                It.Is<object>(m => m.ArePropertiesEqual(expectedParameters, false))), Times.Once);
        }

        protected void VerifyExecuteSqlAsync(string connectionName, string sql, object expectedParameters)
        {
            SqlQueryProviderMock.Verify(mock => mock.ExecuteSqlAsync(connectionName, sql,
                It.Is<object>(m => m.ArePropertiesEqual(expectedParameters, false))), Times.Once);
        }

        protected void VerifyExecuteSqlAsync(string connectionName, string sql)
        {
            SqlQueryProviderMock.Verify(mock => mock.ExecuteSqlAsync(connectionName, sql), Times.Once);
        }
    }
}

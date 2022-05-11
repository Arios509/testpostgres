using Dapper;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Domain.Aggregate.User
{
    public class TestingUser
    {
        [Column(TypeName = "jsonb")]
        public Detail Detail { get; private set; }
       
        [Column(TypeName = "jsonb")]
        public Detail[] Details { get; private set; }
        public TestingUser() { }
        public TestingUser(Detail detail, Detail[] details = null)
        {
            Detail = detail;
            Details = new List<Detail>().ToArray();
        }
    }

    public class Detail
    {
        public string Name { get; }
        public Detail()
        {
            Name = "Test";
        }
    }

    public class JsonTypeHandler : SqlMapper.ITypeHandler
    {
        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public object Parse(Type destinationType, object value)
        {
            return JsonConvert.DeserializeObject(value as string, destinationType);
        }
    }
}

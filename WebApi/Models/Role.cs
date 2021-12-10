using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Role
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public IEnumerable<Member> Members { get; set; }
    }
}

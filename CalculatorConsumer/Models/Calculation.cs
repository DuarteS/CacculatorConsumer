using System.ComponentModel.DataAnnotations;

namespace CalculatorApi.Models
{
    public class Calculation
    {
        [Key]
        public Guid Id { get; set; }
        public string Expression { get; set; } = string.Empty;
        public double? Result { get; set; }
    }
}

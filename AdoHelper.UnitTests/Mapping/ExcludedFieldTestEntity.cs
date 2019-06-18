namespace AdoHelper.UnitTests.Mapping
{
    public class ExcludedFieldTestEntity
    {
        public int Id { get; set; }

        [NonMapped]
        public string TextField { get; set; }

        [NonMapped]
        public double FloatField { get; set; }

        [Field(Name = "NumericField")]
        public decimal Numeric { get; set; }

        [Field(Name = "IntegerField")]
        public long Integer { get; set; }
    }
}

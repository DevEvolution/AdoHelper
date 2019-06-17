namespace AdoHelper.UnitTests.Mapping
{
    public class NamedTestEntity
    {
        [Field(Name = "id")]
        public long Test_Id { get; set; }

        [Field(Name = "TextField")]
        public string Test_TextField { get; set; }

        [Field(Name = "FloatField")]
        public double Test_FloatField { get; set; }

        [Field(Name = "NumericField")]
        public decimal Test_NumericField { get; set; }

        [Field(Name = "IntegerField")]
        public long Test_IntegerField { get; set; }
    }
}

    namespace CodeAnalysis.Models
{
    public class SchemaStaticAnalysisResult
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Message { get; set; }
        public int Length { get; set; }
        public int NumberOfMethods { get; set; }
        public int NumberOfComments { get; set; }
        public int Complexity { get; set; }
        public bool FollowsNamingConventions { get; set; }
        public bool HasErrorHandling { get; set; }
        public bool HasCodeDuplication { get; set; }
        public bool HasGoodComments { get; set; }
        public bool HasConsistentFormatting { get; set; }
        public bool HasPrivateKeys { get; set; }
        public bool FollowsFileNameConventions { get; set; }

        public int Score { get; set; }
    }
}

namespace ProbeAssistedLeveler
{
    public class ProbeResult
    {
        public string CornerName { get; set; }
        public float Z { get; }
        public float Diff { get; set; }

        public ProbeResult(string cornerName, float z)
        {
            CornerName = cornerName;
            Z = z;
        }
    }
}

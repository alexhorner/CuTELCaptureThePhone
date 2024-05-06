namespace CutelCaptureThePhone.Core
{
    public class CaptureMessageRandomiser
    {
        private List<string>? _positives;
        private List<string>? _negatives;

        public void Configure(List<string> positives, List<string> negatives)
        {
            if (_positives is not null || _negatives is not null) throw new InvalidOperationException("Randomiser has already been configured");

            _positives = positives;
            _negatives = negatives;
        }

        public string GenerateNewPositive()
        {
            if (_positives is null) throw new InvalidOperationException("Randomiser has not been configured");
            
            int generatedPositiveIndex = Random.Shared.Next(0, _positives.Count);

            return _positives[generatedPositiveIndex];
        }
        
        public string GenerateNewNegative()
        {
            if (_negatives is null) throw new InvalidOperationException("Randomiser has not been configured");
            
            int generatedNegativeIndex = Random.Shared.Next(0, _negatives.Count);

            return _negatives[generatedNegativeIndex];
        }
    }
}
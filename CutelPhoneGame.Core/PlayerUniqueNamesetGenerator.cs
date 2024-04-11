namespace CutelPhoneGame.Core
{
    public class PlayerUniqueNamesetGenerator
    {
        public NamesetParts NamesetParts { get; private set; } = null!;

        private readonly SemaphoreSlim _generatorLock = new(1, 1);
        private List<(int PartA, int PartB, int PartC)>? _availableNamesets;

        public void Configure(NamesetParts parts, (int PartA, int PartB, int PartC)[] usedNamesets)
        {
            if (_availableNamesets is not null) throw new InvalidOperationException("Generator has already been configured");
            
            _availableNamesets = new List<(int PartA, int PartB, int PartC)>();

            for (int aIter = 0; aIter < parts.AParts.Count; aIter++)
            {
                for (int bIter = 0; bIter < parts.BParts.Count; bIter++)
                {
                    for (int cIter = 0; cIter < parts.CParts.Count; cIter++)
                    {
                        _availableNamesets.Add(new (aIter, bIter, cIter));
                    }
                }
            }

            _availableNamesets = _availableNamesets.Except(usedNamesets).ToList();

            NamesetParts = parts;
        }

        public (int PartA, int PartB, int PartC) GenerateNewNameset()
        {
            if (_availableNamesets is null) throw new InvalidOperationException("Generator has not been configured");
            
            _generatorLock.Wait();
            
            try
            {
                int generatedPinIndex = Random.Shared.Next(0, _availableNamesets.Count);

                (int PartA, int PartB, int PartC) generatedNameset = _availableNamesets[generatedPinIndex];
                
                _availableNamesets.RemoveAt(generatedPinIndex);

                return generatedNameset;
            }
            finally
            {
                _generatorLock.Release();
            }
        }

        public string GenerateNewName() => NamesetParts.GetNameFromNameset(GenerateNewNameset());

        public void ReleaseFailedNameset((int PartA, int PartB, int PartC) nameset)
        {
            if (_availableNamesets is null) throw new InvalidOperationException("Generator has not been configured");
            
            _generatorLock.Wait();
            
            try
            {
                if (!_availableNamesets.Contains(nameset)) _availableNamesets.Add(nameset);
            }
            finally
            {
                _generatorLock.Release();
            }
        }

        public void ReleaseFailedName(string name) => ReleaseFailedNameset(NamesetParts.GetNamesetFromName(name));
    }
}
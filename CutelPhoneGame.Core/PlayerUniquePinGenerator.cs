namespace CutelPhoneGame.Core
{
    public class PlayerUniquePinGenerator
    {
        private readonly SemaphoreSlim _generatorLock = new(1, 1);
        private List<uint>? _availablePins;

        public void Configure(int maxPin, uint[] usedPins)
        {
            if (_availablePins is not null) throw new InvalidOperationException("Generator has already been configured");
            
            _availablePins = new List<uint>();

            for (uint i = 1; i <= maxPin; i++) _availablePins.Add(i);

            _availablePins = _availablePins.Except(usedPins).ToList();
        }

        public uint GenerateNewPin()
        {
            if (_availablePins is null) throw new InvalidOperationException("Generator has not been configured");
            
            _generatorLock.Wait();
            
            try
            {
                int generatedPinIndex = Random.Shared.Next(0, _availablePins.Count);

                uint generatedPin = _availablePins[generatedPinIndex];
                
                _availablePins.RemoveAt(generatedPinIndex);

                return generatedPin;
            }
            finally
            {
                _generatorLock.Release();
            }
        }

        public void ReleaseFailedPin(uint pin)
        {
            if (_availablePins is null) throw new InvalidOperationException("Generator has not been configured");
            
            _generatorLock.Wait();
            
            try
            {
                if (!_availablePins.Contains(pin)) _availablePins.Add(pin);
            }
            finally
            {
                _generatorLock.Release();
            }
        }
    }
}
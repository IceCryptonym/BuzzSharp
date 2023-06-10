namespace BuzzSharp
{
    public enum LightState
    {
        On,
        Off,
        Unchanged
    }

    public static class LightStateExtensions
    {
        public static byte ToByteValue(this LightState state)
        {
            return state switch
            {
                LightState.On => 0xFF,
                LightState.Off => 0x00,
                _ => 0x0A
            };
        }
    }
}

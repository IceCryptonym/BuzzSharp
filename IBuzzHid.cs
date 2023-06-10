namespace BuzzSharp
{
    public interface IBuzzHid
    {
        void Read();
        
        void SetLight(int id, LightState state, LightState others);
        void SetLights(LightState light1, LightState light2, LightState light3, LightState light4);
        void SetAllLights(LightState state);

        bool IsAnyHandsetPressed();
    }
}
using HidApi;

namespace BuzzSharp
{
    public class BuzzHid : IBuzzHid, IDisposable
    {
        private Device device;
        public BuzzHandset[] Handsets { get; }

        public EventHandler<ButtonStatesToggledEventArgs>? OnButtonStatesToggled;
        
        public BuzzHid(Device device)
        {
            this.device = device;
            this.device.SetNonBlocking(true);
            Handsets = new BuzzHandset[4] {
                new BuzzHandset(1),
                new BuzzHandset(2),
                new BuzzHandset(3),
                new BuzzHandset(4)
            };

            SetAllLights(LightState.Unchanged);
        }

        public void Read()
        {
            var buffer = device.Read(5);
            if (buffer.Length == 0)
                return;

            var processedData = HandleHandsetsData(buffer);
            if (processedData.EventArgs != null)
            {
                processedData.EventArgs.Handset.States = processedData.States;
                OnButtonStatesToggled?.Invoke(this, processedData.EventArgs);
            }
        }

        private (ButtonStatesToggledEventArgs? EventArgs, ButtonStates States) HandleHandsetsData(ReadOnlySpan<byte> buffer)
        {
            var handset1States = new ButtonStates(
                ((buffer[2] & 0x01) != 0),
                ((buffer[2] & 0x02) != 0),
                ((buffer[2] & 0x04) != 0),
                ((buffer[2] & 0x08) != 0),
                ((buffer[2] & 0x10) != 0)
            );

            if (Handsets[0].States.GetToggled(out var toggledStates1, handset1States))
                return (new ButtonStatesToggledEventArgs(Handsets[0], toggledStates1), handset1States);

            var handset2States = new ButtonStates(
                ((buffer[2] & 0x20) != 0),
                ((buffer[2] & 0x40) != 0),
                ((buffer[2] & 0x80) != 0),
                ((buffer[3] & 0x01) != 0),
                ((buffer[3] & 0x02) != 0)
            );

            if (Handsets[1].States.GetToggled(out var toggledStates2, handset2States))
                return (new ButtonStatesToggledEventArgs(Handsets[1], toggledStates2), handset2States);

            var handset3States = new ButtonStates(
                ((buffer[3] & 0x04) != 0),
                ((buffer[3] & 0x08) != 0),
                ((buffer[3] & 0x10) != 0),
                ((buffer[3] & 0x20) != 0),
                ((buffer[3] & 0x40) != 0)
            );

            if (Handsets[2].States.GetToggled(out var toggledStates3, handset3States))
                return (new ButtonStatesToggledEventArgs(Handsets[2], toggledStates3), handset3States);

            var handset4States = new ButtonStates(
                ((buffer[3] & 0x80) != 0),
                ((buffer[4] & 0x01) != 0),
                ((buffer[4] & 0x02) != 0),
                ((buffer[4] & 0x04) != 0),
                ((buffer[4] & 0x08) != 0)
            );

            if (Handsets[3].States.GetToggled(out var toggledStates4, handset4States))
                return (new ButtonStatesToggledEventArgs(Handsets[3], toggledStates4), handset4States);

            return (null, ButtonStates.Default);
        }

        public void SetLight(int id, LightState state)
        {
            SetLight(id, state, LightState.Unchanged);
        }

        public void SetLight(int id, LightState state, LightState others)
        {
            switch (id)
            {
                case 1:
                    SetLights(state, others, others, others);
                    break;
                case 2:
                    SetLights(others, state, others, others);
                    break;
                case 3:
                    SetLights(others, others, state, others);
                    break;
                case 4:
                    SetLights(others, others, others, state);
                    break;
                default:
                    break;
            }
        }

        public void SetLights(LightState light1, LightState light2, LightState light3, LightState light4)
        {
            byte[] buffer = new byte[6];
            
            buffer[0] = 0x00;
            buffer[1] = 0x00;
            buffer[2] = light1.ToByteValue();
            buffer[3] = light2.ToByteValue();
            buffer[4] = light3.ToByteValue();
            buffer[5] = light4.ToByteValue();
            device.Write(buffer);
        }

        public void SetAllLights(LightState state)
        {
            SetLights(state, state, state, state);
        }

        public bool IsAnyHandsetPressed()
        {
            foreach (var handset in Handsets)
                if (handset.IsAnyButtonPressed())
                    return true;

            return false;
        }

        public void Dispose()
        {
            SetAllLights(LightState.Off);

            device.Dispose();
        }
    }
}
namespace BuzzSharp
{
    public record Button(Color Color, bool IsPressed);

    public record ButtonStates(bool IsRedPressed, bool IsYellowPressed, bool IsGreenPressed, bool IsOrangePressed, bool IsBluePressed)
    {
        public static readonly ButtonStates Default = new ButtonStates(false, false, false, false, false);

        internal bool GetToggled(out IReadOnlyCollection<Button> toggled, ButtonStates other)
        {
            var toggledList = new List<Button>();

            if (other.IsRedPressed != IsRedPressed) toggledList.Add(new Button(Color.Red, other.IsRedPressed));
            if (other.IsYellowPressed != IsYellowPressed) toggledList.Add(new Button(Color.Yellow, other.IsYellowPressed));
            if (other.IsGreenPressed != IsGreenPressed) toggledList.Add(new Button(Color.Green, other.IsGreenPressed));
            if (other.IsOrangePressed != IsOrangePressed) toggledList.Add(new Button(Color.Orange, other.IsOrangePressed));
            if (other.IsBluePressed != IsBluePressed) toggledList.Add(new Button(Color.Blue, other.IsBluePressed));
            
            toggled = toggledList.AsReadOnly();
            return (toggled.Count > 0);
        }
    }
}
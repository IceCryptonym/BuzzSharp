namespace BuzzSharp
{
    public class BuzzHandset
    {
        public int Id { get; init; }
        public ButtonStates States { get; internal set; }

        public BuzzHandset(int id)
        {
            Id = id;
            States = ButtonStates.Default;
        }

        public bool IsAnyButtonPressed()
        {
            return (States.IsRedPressed    ||
                    States.IsYellowPressed ||
                    States.IsGreenPressed  ||
                    States.IsOrangePressed ||
                    States.IsBluePressed);
        }
    }
}
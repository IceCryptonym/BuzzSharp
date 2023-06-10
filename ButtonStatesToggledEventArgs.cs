namespace BuzzSharp
{
    public class ButtonStatesToggledEventArgs : EventArgs
    {
        public BuzzHandset Handset { get; }
        public IReadOnlyCollection<Button> Toggled { get; }

        public ButtonStatesToggledEventArgs(BuzzHandset handset, IReadOnlyCollection<Button> toggled)
        {
            Handset = handset;
            Toggled = toggled;
        }
    }
}
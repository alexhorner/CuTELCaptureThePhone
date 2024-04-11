namespace CutelPhoneGame.Core.Attributes
{
    public class EnumDisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }
        
        public EnumDisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
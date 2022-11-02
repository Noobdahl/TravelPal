namespace TravelPal.PackingList
{
    public class TravelDocument : IPackingListItem
    {
        public string Name { get; set; }
        public bool IsRequired { get; set; }

        public TravelDocument(string name, bool isRequired)
        {
            Name = name;
            IsRequired = isRequired;
        }

        public string GetInfo()
        {
            if (IsRequired)
                return $"{Name} - Required!";
            else
                return $"{Name} - Not Required!";
        }
    }
}

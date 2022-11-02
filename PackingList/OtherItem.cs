namespace TravelPal.PackingList
{
    public class OtherItem : IPackingListItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public OtherItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public string GetInfo()
        {
            return $"{Quantity}x {Name}";
        }
    }
}

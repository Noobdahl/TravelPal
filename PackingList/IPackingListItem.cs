namespace TravelPal.PackingList
{
    public interface IPackingListItem
    {
        public string Name { get; set; }

        public string GetInfo();
    }
}

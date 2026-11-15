namespace AyuLanka.AMS.AMSWeb.Models.ResponseModels
{
    // Item modes of one item code, as returned by the user-management API (itemMode/GetItemModesByItemCodes)
    public class ItemModeLookupDto
    {
        public string ItemCode { get; set; }
        public List<ItemModeDto> ItemModes { get; set; } = new();
    }

    public class ItemModeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

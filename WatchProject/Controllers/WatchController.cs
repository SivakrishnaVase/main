using Microsoft.AspNetCore.Mvc;

namespace WatchProject.Controllers
{

[ApiController]
[Route("[controller]")]
public class WatchController : ControllerBase
{
    private readonly IDictionary<string, Watch> _watches;
    private readonly IDictionary<string, Discount> _discounts;

    public WatchController()
    {
        // Initialize watches and discounts
        _watches = new Dictionary<string, Watch>
        {
            ["001"] = new Watch { Id = "001", Price = 100 },
            ["002"] = new Watch { Id = "002", Price = 80 },
            // Add more watches
        };

        _discounts = new Dictionary<string, Discount>
        {
            ["001"] = new Discount { WatchId = "001", Quantity = 3, DiscountedPrice = 200 },
            // Define discounts for other watch IDs
        };
    }

    [HttpPost]
    public ActionResult Checkout([FromBody] List<string> watchIds)
    {
        if (watchIds == null || !watchIds.Any())
        {
            return BadRequest("No watches to checkout.");
        }

        var total = CalculateTotal(watchIds);
        return Ok(new { Price = total });
    }

     private decimal CalculateTotal(List<string> watchIds)
    {
        decimal total = 0;
        var groupedWatches = watchIds.GroupBy(id => id);

        foreach (var group in groupedWatches)
        {
            if (!_watches.TryGetValue(group.Key, out var watch))
            {
                // Handle the case where the watch ID is not found
                continue;
            }

            var quantity = group.Count();
            total += _discounts.TryGetValue(group.Key, out var discount) && quantity >= discount.Quantity
                ? (quantity / discount.Quantity) * discount.DiscountedPrice + (quantity % discount.Quantity) * watch.Price
                : quantity * watch.Price;
        }

        return total;
    }
}

public class Watch
{
    public string Id { get; set; }
    public decimal Price { get; set; }
}

public class Discount
{
    public string WatchId { get; set; }
    public int Quantity { get; set; }
    public decimal DiscountedPrice { get; set; }
}
}


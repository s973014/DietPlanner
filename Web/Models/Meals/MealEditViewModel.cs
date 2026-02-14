namespace Web.Models.Meals
{
    public class MealEditVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public List<MealProductEditVm> Ingredients { get; set; } = new();
    }

    public class MealProductEditVm
    {
        public Guid ProductId { get; set; }
        public float AmountInGrams { get; set; }
        public bool Remove { get; set; } = false;
    }

}

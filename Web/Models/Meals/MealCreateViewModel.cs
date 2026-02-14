namespace Web.Models.Meals
{
    public class MealCreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<MealIngredientViewModel> Ingredients { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{

    public class Nutrition
    {
        public float Calories { get; set; }
        public float Proteins { get; set; }
        public float Fats { get; set; }
        public float Carbs { get; set; }

        public Nutrition() { }

        public Nutrition(float calories, float proteins, float fats, float carbs)
        {
            Calories = calories;
            Proteins = proteins;
            Fats = fats;
            Carbs = carbs;
        }
        

        public static Nutrition CreateForTest(float calories, float proteins, float fats, float carbs)
        {
            return new Nutrition(calories, proteins, fats, carbs);
        }

        public static Nutrition operator +(Nutrition a, Nutrition b)
            => new(
                a.Calories + b.Calories,
                a.Proteins + b.Proteins,
                a.Fats + b.Fats,
                a.Carbs + b.Carbs
            );
    }


}

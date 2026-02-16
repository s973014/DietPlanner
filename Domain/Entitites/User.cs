using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitites
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }

        public float? Weight { get; private set; }
        public float? Height { get; private set; }
        public ActivityLevel? ActivityLevel { get; private set; }
        public GoalType? Goal { get; private set; }

        private readonly List<Allergy> _allergies = new();
        public IReadOnlyCollection<Allergy> Allergies => _allergies;

        private User() { }

        public User(string name, string email, string passwordHash, UserRole role)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        public void UpdateProfile(float weight, float height, ActivityLevel activity, GoalType goal)
        {
            Weight = weight;
            Height = height;
            ActivityLevel = activity;
            Goal = goal;
        }

        public void SetPassword(string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
                throw new ArgumentException("Пароль не может быть пустым", nameof(hashedPassword));

            PasswordHash = hashedPassword;
        }

        public void ClearAllergies()
        {
            _allergies.Clear();
        }

        public void AddAllergy(Allergy allergy)
        {
            if (!_allergies.Contains(allergy))
                _allergies.Add(allergy);
        }
    }
}

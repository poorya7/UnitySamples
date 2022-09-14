
using System.Collections.Generic;

// This code was part of the cookie maker project we did for Gaylord Resort hotels’ “Elf Save the Christmas” attraction, using Unity3d, C# and MQTT messaging. 
// Visitors would physically select cookie ingrediants (using RFID) and in the monitor, their completed cookie will show up. 
// Also look at the IngrediantsManager class. 
// video of the a visitor experiencing the project can be seen here (watch at time 5:40) : https://youtu.be/Z6vvgUuGKc8?t=336 

namespace XStudios
{
    public class Cookie
    {
        public enum Ingrediants { Flour = 1, Sugar = 2, Butter = 3, BakingSoda = 4, Spice = 5 };

        protected List<int> _flours;

        protected virtual void InitFlours()
        {
             _flours= new List<int>();
            _flours.AddRange(new int[] { 1, 2, 3 });
        }

        public Cookie()
        {
            InitFlours();
        }

        public bool Contains(int flour)
        {
            return _flours.Contains(flour);
        }

        public bool IsComplete(int input)
        {
            int sum = 0;
            foreach (int flour in _flours)
                sum |= flour;
            return input == sum;
        }

        public List<int> GetFlours()
        {
            return _flours;
        }
    }

    public class Sugar : Cookie
    {
        public Sugar() : base() { }

        protected override void InitFlours()
        {
            _flours = new List<int>();
            _flours.AddRange(new int[] { 1, 2, 3 });
        }
    }

    public class GingerBread : Cookie
    {
        public GingerBread() : base() { }
        protected override void InitFlours()
        {
            _flours = new List<int>();
            _flours.AddRange(new int[] { 1, 2, 5 });
        }
    }

    public class Chocolate : Cookie
    {
        public Chocolate() : base() { }
        protected override void InitFlours()
        {
            _flours = new List<int>();
            _flours.AddRange(new int[] { 2, 3, 5 });
        }
    }

    public class Oatmeal : Cookie
    {
        public Oatmeal() : base() { }
        protected override void InitFlours()
        {
            _flours = new List<int>();
            _flours.AddRange(new int[] { 1, 2, 4 });
        }
    }

    public class Shortbread : Cookie
    {
        public Shortbread() : base() { }
        protected override void InitFlours()
        {
            _flours = new List<int>();
            _flours.AddRange(new int[] { 2, 3, 4 });
        }
    }

    public class PeanutButter : Cookie
    {
        public PeanutButter() : base() { }
        protected override void InitFlours()
        {
            _flours = new List<int>();
            _flours.AddRange(new int[] { 1, 3, 5 });
        }
    }
}

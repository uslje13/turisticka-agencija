using System;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Comment : ISerializable
    {
        private int _id;
        private string _text;

        public int Id { get => _id; set => _id = value; }
        public string Text { get => _text; set => _text = value; }

        //User polje

        public Comment()
        {
            _id = -1;
            _text = string.Empty;
        }
        public Comment(string text)
        {
            _text = text;
        }

        public void FromCSV(string[] values)
        {
            _id = Convert.ToInt32(values[0]);
            _text = Convert.ToString(values[1]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                _id.ToString(),
                _text
            };
            return csvValues;
        }
    }
}

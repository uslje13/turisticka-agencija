using System;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Tour : ISerializable
    {
        private int _id;        //tour _id
        private string _name;
        private Location _location;
        private string _description;
        private string _language;
        private int _maxVisitors;
        private Location[] _checkpoints;
        private DateTime[] _dateStart;    //date and time when tour start 
        private int _duration;      //in hours





        //id|_name|_location|_description|_language|_maxVisitors|?|?|_duration
        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }

        public string[] ToCSV()
        {
            throw new NotImplementedException();
        }
    }
}

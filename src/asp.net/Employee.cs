using System;

namespace asp.net
{

    public class Employee
    {

        public Employee(string id, string name, string gender, DateTime birthDate, string department)
        {
            Id = id;
            Name = name;
            Gender = gender;
            BirthDate = birthDate;
            Department = department;
        }

        public string Id { get; }

        public string Name { get; }

        public string Gender { get; }

        public DateTime BirthDate { get; }

        public string Department { get; }

    }

}
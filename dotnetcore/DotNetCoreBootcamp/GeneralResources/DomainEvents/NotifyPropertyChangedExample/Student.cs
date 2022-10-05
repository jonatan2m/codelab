using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DomainEvents.NotifyPropertyChangedExample
{
    public class Student : INotifyPropertyChanged
    {
        public Student(string name, int registerNumber)
        {
            Name = name;
            RegisterNumber = registerNumber;
            _grade = 0;
        }

        public string Name { get; set; }

        public int RegisterNumber { get; set; }

        private int _grade;
        public int Grade
        {
            get { return _grade; }
            set
            {
                _grade = value;
                OnPropertyChanged(); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateGrade(int newGrade)
        {
            Grade = newGrade;
        }

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Better way using the CallerMemberName Attribute
        /// </summary>
        /// <param name="propName"></param>
        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

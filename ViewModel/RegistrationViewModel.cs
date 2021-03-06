﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaldenHospitalConsumer.Utilities;
using WaldenHospitalConsumer.View;
using WaldenHospitalConsumer.Model;
using WaldenHospitalConsumer.Model.Catalog;
using WaldenHospitalConsumer.CurrentEntities;

namespace WaldenHospitalConsumer.ViewModel
{
   public class RegistrationViewModel
    {
     
        public Patient Patient { get; set; }


        

        public  void Register(object s)
        {
            
            PatientCatalog PatientCatalog = new PatientCatalog();
            PatientCatalog.GetData(Patient);
            PatientCatalog.Post();
            Type type = typeof(PatientListView);
            FrameNavigation.ActivateFrameNavigation(type);
        }


        public void IsCheckedMale(object s)
        {
            Patient.Gender = "Male";
        }

        public void IsCheckedFemale(object s)
        {
            Patient.Gender = "Female";
        }


        public RegistrationViewModel()
        {
            DoLogOut = new RelayCommand(LogOut);
            DoShowListOfPatients = new RelayCommand(ShowListOfPatient);
            DoShowNewsView = new RelayCommand(ShowNewsView);
            DoIsCheckedMale = new RelayCommand(IsCheckedMale);
            DoIsCheckedFemale = new RelayCommand(IsCheckedFemale);
            DoRegister = new RelayCommand(Register);
            Patient = new Patient();

        }

        public RelayCommand DoRegister { get; set; }
        public RelayCommand DoIsCheckedMale { get; set; }
        public RelayCommand DoIsCheckedFemale { get; set; }
        public RelayCommand DoLogOut { get; set; }
        public RelayCommand DoShowListOfPatients { get; set; }
        public RelayCommand DoShowNewsView { get; set; }

        public void LogOut(object s)
        {
            Type type = typeof(LoginView);
            FrameNavigation.ActivateFrameNavigation(type);
        }

        public void ShowListOfPatient(object s)
        {
            Type type = typeof(PatientListView);
            FrameNavigation.ActivateFrameNavigation(type);
        }

        public void ShowNewsView(object s)
        {
            Type type = typeof(NewsView);
            FrameNavigation.ActivateFrameNavigation(type);
        }
    }
}

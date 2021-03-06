﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaldenHospitalConsumer.Handler;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.Web.Http;
using Newtonsoft.Json;
using WaldenHospitalConsumer.Utilities;
using WaldenHospitalConsumer.ViewModel;
using Windows.Web.Http.Headers;

namespace WaldenHospitalConsumer.Model.Catalog
{
    public class PatientCatalog : IRequestHttpHandler<Patient>
    {
        private const string Uri = "http://localhost:54174/api/patients";


        public ObservableCollection<Patient> Patients { get; set; }
        public Patient Patient { get; set; }

       

        public PatientCatalog()
        {
            Patient = new Patient();
            Patients = new ObservableCollection<Patient>();
            FetchAllData();
            
        }


        public async void FetchAllData()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = "";
                    Task task = Task.Run(async () =>
                    {
                        // sends GET request
                        // ReSharper disable once AccessToDisposedClosure
                        response = await client.GetStringAsync(new Uri(Uri));
                    });
                    // Wait
                    task.Wait();
                    // convert Json into Objects
                    if (response != null)
                    {
                        Patients = JsonConvert.DeserializeObject<ObservableCollection<Patient>>(response);
                        //call on property change interface.
                    }
                }
                catch (Exception ex)
                {
                    var messageDialog = new MessageDialog(ex.Message);
                    await messageDialog.ShowAsync();
                }
            }
        }

        

        public async void Post()
        {

            Patient Patient12 = new Patient
            {
                Cpr = Patient.Cpr,
                Name = Patient.Name,
                LastName = Patient.LastName,
                Gender = Patient.Gender,
                BirthTime = DateTimeOffsetAndTimeSetToDateTime(Patient.BirthTime)
            };

            
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
                    //We need to convert new object firstly into json format and then into json string form.
                    var jsonStr = JsonConvert.SerializeObject(Patient12);
                    var content = new HttpStringContent(jsonStr, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
                    HttpResponseMessage response = null;
                    Task task = Task.Run(async () =>
                    {
                        //Here we send a post request.
                        response = await client.PostAsync(new Uri(Uri), content);
                    });
                    task.Wait();
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        throw new Exception("Patient already exist!");
                    }
                    //id response successed.
                    if(response.IsSuccessStatusCode)
                    {
                        string jsonFormat = await response.Content.ReadAsStringAsync();
                        var newPatient = JsonConvert.DeserializeObject<Patient>(jsonFormat);
                        string patient = $"Cpr:{newPatient.Cpr}, Name:{newPatient.Name}, Last Name:{newPatient.LastName}, Gender:{newPatient.Gender}, BirthTime:{newPatient.BirthTime}";
                        var messageDialog = new MessageDialog("Congratulations New Patient has been added correctly." + patient);
                        await messageDialog.ShowAsync();
                    }
                    else
                    {
                        if(response.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            var JsonError = await response.Content.ReadAsStringAsync();
                            var messageDialog = new MessageDialog(JsonError);
                            await messageDialog.ShowAsync();
                        }
                    }
                }
            
           
           

        }

        public static DateTime DateTimeOffsetAndTimeSetToDateTime(DateTimeOffset date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }




        public void GetData(Patient pat)
        {
            Patient = pat;
        }
    }
}


